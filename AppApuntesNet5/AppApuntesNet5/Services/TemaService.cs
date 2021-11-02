using AppApuntesNet5.Dto;
using AppApuntesNet5.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppApuntesNet5.Services
{
    public class TemaService : ITemaService
    {

        private ApplicationDbContext db;

        public TemaService(ApplicationDbContext context)
        {
            this.db = context;
        }

        public async Task<List<ApuntesTema>> Listar()
        {
            return await db.ApuntesTema.ToListAsync();
        }

        public async Task<ApuntesTema> BuscarPorId(int id)
        {
            var v = (from c in db.ApuntesTema
            .Include(i => i.ApuntesCategoria)
            .Include(i => i.ListaApuntesDetalleTema)
                     where c.Id == id
                     select c);

            return await v.Cast<ApuntesTema>().FirstOrDefaultAsync();
        }

        public async Task<DataTableDTO> LlenarDataTableApuntesTema(ApuntesTema apuntesTema, int inicio, int registrosPorPagina)
        {
            if (apuntesTema == null) apuntesTema = new ApuntesTema();  // En caso de que sea nulo se inicializa 

            IQueryable<ApuntesTema> v = (from a in db.ApuntesTema
                .Include(c => c.ApuntesCategoria)
                                         select a);

            if (apuntesTema.ApuntesCategoria != null)
                v = v.Where(a => a.ApuntesCategoriaId == apuntesTema.ApuntesCategoria.Id);

            if (!string.IsNullOrEmpty(apuntesTema.Titulo))
                v = v.Where(a => a.Titulo.Contains(apuntesTema.Titulo));

            int totalRegistros = v.Count();
            v = v.OrderBy(x => x.Id).Skip(inicio).Take(registrosPorPagina);

            List<ApuntesTema> lista = await v.ToListAsync();
            return new DataTableDTO() { RecordsFiltered = totalRegistros, RecordsTotal = totalRegistros, Data = lista };
        }

        public async Task<Select2DTO> LlenarSelect2(string busqueda, int registrosPorPagina, int numeroPagina, int idApuntesCategoria)
        {
            object dataSalida = null;
            int cantidadRegistros = 0;

            IQueryable<ApuntesTema> consulta = (from a in db.ApuntesTema select a);

            if (idApuntesCategoria != 0)
                consulta = consulta.Where(a => a.ApuntesCategoriaId == idApuntesCategoria);

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

            apuntesTema.ApuntesCategoriaId = apuntesTema.ApuntesCategoria.Id;
            apuntesTema.ApuntesCategoria = null;

            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (apuntesTema.ListaApuntesDetalleTema != null && apuntesTema.ListaApuntesDetalleTema.Count > 0)
                    {
                        foreach (ApuntesDetalleTema detalle in apuntesTema.ListaApuntesDetalleTema)
                        {
                            detalle.ApuntesTema = apuntesTema;
                            await db.ApuntesDetalleTema.AddAsync(detalle);
                        }
                    }

                    await db.ApuntesTema.AddAsync(apuntesTema);

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
                    ApuntesTema objeto = await db.ApuntesTema.AsNoTracking().Where(x => x.Id == apuntesTema.Id).FirstOrDefaultAsync();

                    objeto.ApuntesCategoriaId = apuntesTema.ApuntesCategoria.Id;
                    objeto.Titulo = apuntesTema.Titulo;


                    db.Entry(objeto).State = EntityState.Modified; 

                    List<ApuntesDetalleTema> listaApuntesDetalleTema = apuntesTema.ListaApuntesDetalleTema.ToList();

                    if (listaApuntesDetalleTema.Count() > 0)
                    {
                        if (listaApuntesDetalleTema.Count == 1 && listaApuntesDetalleTema.Any(c => c.Id == 0))  // Si solo se recibio un detalle y es nuevo 
                        {
                            db.ApuntesDetalleTema.RemoveRange(
                                db.ApuntesDetalleTema.Where(c => c.ApuntesTema.Id == apuntesTema.Id).ToList()
                            );
                        }
                        if (listaApuntesDetalleTema.Any(c => c.Id > 0))  // Si al menos un detalle trae id  
                        {
                            List<int> listaIdDetalles = listaApuntesDetalleTema.Where(c => c.Id > 0).Select(c => c.Id).ToList();

                            db.ApuntesDetalleTema.RemoveRange(
                                db.ApuntesDetalleTema.Where(c => !listaIdDetalles.Contains(c.Id) &&
                                c.ApuntesTema.Id == apuntesTema.Id).ToList()
                            );
                        }
                        foreach (ApuntesDetalleTema detalle in listaApuntesDetalleTema.ToList())
                        {
                            if (detalle.Id > 0)  // Si trae id significa que esta almacenado. Se actualiza 
                            {
                                ApuntesDetalleTema detalleBuscado = db.ApuntesDetalleTema.AsNoTracking().Where(c => c.Id == detalle.Id).FirstOrDefault();
                                detalleBuscado.RutaFoto = detalle.RutaFoto;
                                detalleBuscado.Contenido = detalle.Contenido;
                                detalleBuscado.Titulo = detalle.Titulo;
                                db.Entry(detalleBuscado).State = EntityState.Modified;
                            }
                            else  // Si no esta guardado el detalle recorrido, se agrega 
                            {
                                detalle.ApuntesTema = objeto;
                                await db.ApuntesDetalleTema.AddAsync(detalle);
                            }
                        }
                    }
                    else  // Si no se recibieron detalles de la clase ApuntesDetalleTema 
                    {
                        db.ApuntesDetalleTema.RemoveRange(
                            db.ApuntesDetalleTema.Where(c => c.ApuntesTema.Id == apuntesTema.Id).ToList()
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
                    ApuntesTema apuntesTema = await db.ApuntesTema.AsNoTracking().Where(c => c.Id == id).FirstOrDefaultAsync();

                    db.ApuntesDetalleTema.RemoveRange(
                        db.ApuntesDetalleTema.Where(c => c.ApuntesTema.Id == id).ToList()
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

            if (apuntesTema.ApuntesCategoria == null)
                mensajeError = "El campo apuntesCategoria no posee un valor";

            else if (string.IsNullOrEmpty(apuntesTema.Titulo))
                mensajeError = "El campo titulo no posee un valor";

            else
            {
                foreach (ApuntesDetalleTema c in apuntesTema.ListaApuntesDetalleTema)
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
