using AppApuntesNet5.Dto;
using AppApuntesNet5.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppApuntesNet5.Services
{
    public class DetalleTemaService : IDetalleTemaService
    {

        private ApplicationDbContext db;

        public DetalleTemaService(ApplicationDbContext context)
        {
            this.db = context;
        }

        public async Task<List<ApuntesDetalleTema>> Listar()
        {
            return await db.ApuntesDetalleTema.ToListAsync();
        }

        public async Task<ApuntesDetalleTema> BuscarPorId(int id)
        {
            var v = (from c in db.ApuntesDetalleTema
                .Include(i => i.ApuntesTema)
                     where c.Id == id
                     select c);

            return await v.Cast<ApuntesDetalleTema>().FirstOrDefaultAsync();
        }

        public async Task<DataTableDTO> LlenarDataTableApuntesDetalleTema(ApuntesDetalleTema apuntesDetalleTema, int inicio, int registrosPorPagina)
        {
            if (apuntesDetalleTema == null) apuntesDetalleTema = new ApuntesDetalleTema();  // En caso de que sea nulo se inicializa 

            IQueryable<ApuntesDetalleTema> v = (from a in db.ApuntesDetalleTema
                .Include(c => c.ApuntesTema)
                .Include(c => c.ApuntesTema.ApuntesCategoria)
                                                select a);

            if (apuntesDetalleTema.ApuntesTema != null && apuntesDetalleTema.ApuntesTema.ApuntesCategoria != null && apuntesDetalleTema.ApuntesTema.ApuntesCategoria.Id != 0)
                v = v.Where(a => a.ApuntesTema.ApuntesCategoria.Id == apuntesDetalleTema.ApuntesTema.ApuntesCategoria.Id);

            if (apuntesDetalleTema.ApuntesTema != null && apuntesDetalleTema.ApuntesTema.Id != 0)
                v = v.Where(a => a.ApuntesTemaId == apuntesDetalleTema.ApuntesTema.Id);

            if (!string.IsNullOrEmpty(apuntesDetalleTema.RutaFoto))
                v = v.Where(a => a.RutaFoto.Contains(apuntesDetalleTema.RutaFoto));

            if (!string.IsNullOrEmpty(apuntesDetalleTema.Titulo))
                v = v.Where(a => a.Titulo.Contains(apuntesDetalleTema.Titulo));


            int totalRegistros = v.Count();
            v = v.OrderBy(x => x.Id).Skip(inicio).Take(registrosPorPagina);

            List<ApuntesDetalleTema> lista = await v.ToListAsync();
            return new DataTableDTO() { RecordsFiltered = totalRegistros, RecordsTotal = totalRegistros, Data = lista };
        }

        public async Task<(ApuntesDetalleTema, string)> Guardar(ApuntesDetalleTema objeto)
        {
            if (!ValidarApuntesDetalleTema(objeto, out string error))
                return (null, error);

            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    ApuntesDetalleTema model = new ApuntesDetalleTema()
                    {
                        ApuntesTemaId = objeto.ApuntesTema.Id,
                        Titulo = objeto.Titulo,
                        Contenido = objeto.Contenido,
                        RutaFoto = objeto.RutaFoto
                    };

                    await db.ApuntesDetalleTema.AddAsync(model); // Insertar 

                    await db.SaveChangesAsync();
                    await dbContextTransaction.CommitAsync();
                    return (model, "");

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

        public async Task<(ApuntesDetalleTema, string)> Actualizar(ApuntesDetalleTema objeto)
        {
            if (!ValidarApuntesDetalleTema(objeto, out string error))
                return (null, error);

            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    ApuntesDetalleTema model = await db.ApuntesDetalleTema.AsNoTracking().Where(x => x.Id == objeto.Id).FirstOrDefaultAsync();

                    model.ApuntesTema = objeto.ApuntesTema;
                    model.RutaFoto = objeto.RutaFoto;
                    model.Contenido = objeto.Contenido;
                    model.Titulo = objeto.Titulo;

                    db.Entry(model).State = EntityState.Modified;

                    await db.SaveChangesAsync();
                    await dbContextTransaction.CommitAsync();
                    return (model, "");
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();   // No se realizan los cambios 

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
                    ApuntesDetalleTema apuntesDetalleTema = await db.ApuntesDetalleTema.AsNoTracking().Where(c => c.Id == id).FirstOrDefaultAsync();

                    db.Entry(apuntesDetalleTema).State = EntityState.Deleted;
                    await db.SaveChangesAsync();
                    await dbContextTransaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();   // No se realizan los cambios 
                    mensajeError = ex.Message;
                }
            }

            return (string.IsNullOrEmpty(mensajeError), mensajeError);
        }


        public bool ValidarApuntesDetalleTema(ApuntesDetalleTema apuntesDetalleTema, out string mensajeError)
        {
            mensajeError = "";  // Inicializa el out como un string vacio

            if (apuntesDetalleTema.ApuntesTema == null)
                mensajeError = "El campo apuntesTema no posee un valor";

            else if (apuntesDetalleTema.Contenido == null)
                mensajeError = "El campo contenido no posee un valor";

            else if (string.IsNullOrEmpty(apuntesDetalleTema.Titulo))
                mensajeError = "El campo titulo no posee un valor";

            return string.IsNullOrEmpty(mensajeError);  // El retorno booleano dependera de si se encontro un error o no
        }
    }
}
