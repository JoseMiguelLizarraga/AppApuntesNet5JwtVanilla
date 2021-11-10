using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Models;
using Mappings;

namespace Services
{
    public class TemaService : ITemaService
    {

        private AppApuntesNet5Context db;

        public TemaService(AppApuntesNet5Context context)
        {
            this.db = context;
        }

        public async Task<List<ApuntesTema>> Listar()
        {
            return await db.ApuntesTemas.ToListAsync();
        }

        public async Task<ApuntesTema> BuscarPorId(int id)
        {
            var v = (from c in db.ApuntesTemas
            .Include(i => i.Categoria)
            .Include(i => i.ApuntesDetalleTemas)
                     where c.Id == id
                     select c);

            return await v.Cast<ApuntesTema>().FirstOrDefaultAsync();
        }

        public async Task<DataTableDTO> LlenarDataTableApuntesTema(ApuntesTema apuntesTema, int inicio, int registrosPorPagina)
        {
            if (apuntesTema == null) apuntesTema = new ApuntesTema();  // En caso de que sea nulo se inicializa 

            IQueryable<ApuntesTema> v = (from a in db.ApuntesTemas
                .Include(c => c.Categoria)
                                         select a);

            if (apuntesTema.Categoria != null)
                v = v.Where(a => a.CategoriaId == apuntesTema.Categoria.Id);

            if (!string.IsNullOrEmpty(apuntesTema.Titulo))
                v = v.Where(a => a.Titulo.Contains(apuntesTema.Titulo));

            int totalRegistros = v.Count();
            v = v.OrderBy(x => x.Id).Skip(inicio).Take(registrosPorPagina);

            List<ApuntesTema> lista = await v.ToListAsync();

            return new DataTableDTO()
            {
                RecordsFiltered = totalRegistros,
                RecordsTotal = totalRegistros,
                Data = lista.Select(Mapper.ToDTO).ToList()
            };
        }

        public async Task<Select2DTO> LlenarSelect2(string busqueda, int registrosPorPagina, int numeroPagina, int idApuntesCategoria)
        {
            object dataSalida = null;
            int cantidadRegistros = 0;

            IQueryable<ApuntesTema> consulta = (from a in db.ApuntesTemas select a);

            if (idApuntesCategoria != 0)
                consulta = consulta.Where(a => a.CategoriaId == idApuntesCategoria);

            if (!string.IsNullOrEmpty(busqueda))
                consulta = consulta.Where(a => a.Titulo.Contains(busqueda));

            cantidadRegistros = consulta.Count();
            consulta.Skip((numeroPagina - 1) * registrosPorPagina).Take(registrosPorPagina);

            List<ApuntesTema> lista = await consulta.ToListAsync();
            dataSalida = lista.Select(a => new { id = a.Id, text = a.Titulo }).ToList();

            return new Select2DTO() { Total = cantidadRegistros, Results = dataSalida };
        }

        public async Task<(ApuntesTema, string)> Guardar(ApuntesTema apuntesTema)
        {
            if (!ValidarApuntesTema(apuntesTema, out string error))
                return (null, error);

            apuntesTema.CategoriaId = apuntesTema.Categoria.Id;
            apuntesTema.Categoria = null;

            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (apuntesTema.ApuntesDetalleTemas != null && apuntesTema.ApuntesDetalleTemas.Count > 0)
                    {
                        foreach (ApuntesDetalleTema detalle in apuntesTema.ApuntesDetalleTemas)
                        {
                            detalle.Tema = apuntesTema;
                            await db.ApuntesDetalleTemas.AddAsync(detalle);
                        }
                    }

                    await db.ApuntesTemas.AddAsync(apuntesTema);

                    await db.SaveChangesAsync();
                    await dbContextTransaction.CommitAsync();
                    return (apuntesTema, "");

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    return (null, ex.InnerException != null && ex.InnerException.InnerException != null ?
                        ex.InnerException.InnerException.Message :
                        ex.Message);
                }
            }
        }

        public async Task<(ApuntesTema, string)> Actualizar(ApuntesTema apuntesTema)
        {
            if (!ValidarApuntesTema(apuntesTema, out string error))
                return (null, error);

            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    ApuntesTema objeto = await db.ApuntesTemas.AsNoTracking().Where(x => x.Id == apuntesTema.Id).FirstOrDefaultAsync();

                    objeto.CategoriaId = apuntesTema.Categoria.Id;
                    objeto.Titulo = apuntesTema.Titulo;


                    db.Entry(objeto).State = EntityState.Modified;

                    List<ApuntesDetalleTema> listaApuntesDetalleTema = apuntesTema.ApuntesDetalleTemas.ToList();

                    if (listaApuntesDetalleTema.Count() > 0)
                    {
                        if (listaApuntesDetalleTema.Count == 1 && listaApuntesDetalleTema.Any(c => c.Id == 0))  // Si solo se recibio un detalle y es nuevo 
                        {
                            db.ApuntesDetalleTemas.RemoveRange(
                                db.ApuntesDetalleTemas.Where(c => c.Tema.Id == apuntesTema.Id).ToList()
                            );
                        }
                        if (listaApuntesDetalleTema.Any(c => c.Id > 0))  // Si al menos un detalle trae id  
                        {
                            List<int> listaIdDetalles = listaApuntesDetalleTema.Where(c => c.Id > 0).Select(c => c.Id).ToList();

                            db.ApuntesDetalleTemas.RemoveRange(
                                db.ApuntesDetalleTemas.Where(c => !listaIdDetalles.Contains(c.Id) &&
                                c.Tema.Id == apuntesTema.Id).ToList()
                            );
                        }
                        foreach (ApuntesDetalleTema detalle in listaApuntesDetalleTema.ToList())
                        {
                            if (detalle.Id > 0)  // Si trae id significa que esta almacenado. Se actualiza 
                            {
                                ApuntesDetalleTema detalleBuscado = db.ApuntesDetalleTemas.AsNoTracking().Where(c => c.Id == detalle.Id).FirstOrDefault();
                                detalleBuscado.RutaFoto = detalle.RutaFoto;
                                detalleBuscado.Contenido = detalle.Contenido;
                                detalleBuscado.Titulo = detalle.Titulo;
                                db.Entry(detalleBuscado).State = EntityState.Modified;
                            }
                            else  // Si no esta guardado el detalle recorrido, se agrega 
                            {
                                detalle.Tema = objeto;
                                await db.ApuntesDetalleTemas.AddAsync(detalle);
                            }
                        }
                    }
                    else  // Si no se recibieron detalles de la clase ApuntesDetalleTema 
                    {
                        db.ApuntesDetalleTemas.RemoveRange(
                            db.ApuntesDetalleTemas.Where(c => c.Tema.Id == apuntesTema.Id).ToList()
                        );
                    }

                    await db.SaveChangesAsync();
                    await dbContextTransaction.CommitAsync();
                    return (apuntesTema, "");
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();

                    return (null, ex.InnerException != null && ex.InnerException.InnerException != null ?
                        ex.InnerException.InnerException.Message :
                        ex.Message);
                }
            }
        }

        public async Task<(bool, string)> Eliminar(int id)
        {
            string mensajeError = "";

            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    ApuntesTema apuntesTema = await db.ApuntesTemas.AsNoTracking().Where(c => c.Id == id).FirstOrDefaultAsync();

                    db.ApuntesDetalleTemas.RemoveRange(
                        db.ApuntesDetalleTemas.Where(c => c.Tema.Id == id).ToList()
                    );

                    db.Entry(apuntesTema).State = EntityState.Deleted;
                    await db.SaveChangesAsync();
                    await dbContextTransaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    mensajeError = ex.Message;
                }
            }

            return (string.IsNullOrEmpty(mensajeError), mensajeError);
        }

        public bool ValidarApuntesTema(ApuntesTema apuntesTema, out string mensajeError)
        {
            mensajeError = "";

            if (apuntesTema.Categoria == null)
                mensajeError = "El campo apuntesCategoria no posee un valor";

            else if (string.IsNullOrEmpty(apuntesTema.Titulo))
                mensajeError = "El campo titulo no posee un valor";

            else
            {
                foreach (ApuntesDetalleTema c in apuntesTema.ApuntesDetalleTemas)
                {
                    if (c.Contenido == null)
                    {
                        mensajeError = "Un detalle de tipo ApuntesDetalleTema tiene vacío el campo contenido";
                        break;
                    }
                    if (string.IsNullOrEmpty(c.Titulo))
                    {
                        mensajeError = "Un detalle de tipo ApuntesDetalleTema tiene vacío el campo titulo";
                        break;
                    }
                }
            }

            return string.IsNullOrEmpty(mensajeError);
        }
    }
}
