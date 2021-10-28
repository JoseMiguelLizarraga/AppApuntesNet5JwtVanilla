using AppApuntesNet5.Dto;
using AppApuntesNet5.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppApuntesNet5.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private List<ApuntesCategoria> Lista { get; set; }  // Esta lista se llena solo una vez consultando la base de datos, y queda en memoria. Despues solo se edita

        public CategoriaService(IServiceScopeFactory scopeFactory)  // Esto solo se llama una vez
        {
            this.scopeFactory = scopeFactory;
            LlenarLista();
        }

        public void LlenarLista()  // Esto solo se llama una vez
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();  // Esto es para acceder al ApplicationDbContext desde singleton
                Lista = db.ApuntesCategoria.ToList();
            }
        }

        public DataTableDTO LlenarDataTableApuntesCategoria(ApuntesCategoria apuntesCategoria, int inicio, int registrosPorPagina)
        {
            if (apuntesCategoria == null) apuntesCategoria = new ApuntesCategoria();  // En caso de que sea nulo se inicializa 

            IQueryable<ApuntesCategoria> consulta = Lista.AsQueryable();

            if (apuntesCategoria.titulo != null)
                consulta = consulta.Where(a => a.titulo.ToLower().Contains(apuntesCategoria.titulo.ToLower()));

            return new DataTableDTO()
            {
                RecordsFiltered = Lista.Count,
                RecordsTotal = Lista.Count,
                Data = consulta.OrderBy(x => x.id).Skip(inicio).Take(registrosPorPagina).ToList()
            };
        }

        public Select2DTO LlenarSelect2(string busqueda, int registrosPorPagina, int numeroPagina)
        {
            IQueryable<ApuntesCategoria> consulta = Lista.AsQueryable();

            if (!string.IsNullOrEmpty(busqueda))
                consulta = consulta.Where(a => a.titulo.ToLower().Contains(busqueda.ToLower()));

            return new Select2DTO()
            {
                Total = Lista.Count,
                Results = consulta.Skip((numeroPagina - 1) * registrosPorPagina).Take(registrosPorPagina).Select(a => new { id = a.id, text = a.titulo }).ToList()
            };
        }

        public ApuntesCategoria BuscarPorId(int id)
        {
            return Lista.FirstOrDefault(x => x.id == id);
        }

        public async Task<(ApuntesCategoria, string)> Guardar(ApuntesCategoria objeto)
        {
            if (!ValidarApuntesCategoria(objeto, out string error))
                return (null, error);

            using (var scope = scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();  // Esto es para acceder al ApplicationDbContext desde singleton

                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        ApuntesCategoria apuntesCategoria = new ApuntesCategoria();
                        apuntesCategoria.titulo = objeto.titulo;

                        await db.ApuntesCategoria.AddAsync(apuntesCategoria); // Insertar 

                        await db.SaveChangesAsync();
                        await dbContextTransaction.CommitAsync();

                        Lista.Add(apuntesCategoria);   // Se agrega a la lista unica

                        return (apuntesCategoria, "");
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();

                        error = ex.InnerException != null && ex.InnerException.InnerException != null ?
                            ex.InnerException.InnerException.Message :  // Lanza errores SQL 
                            ex.Message;

                        return (null, error);
                    }
                }
            }
        }


        public async Task<(ApuntesCategoria, string)> Actualizar(ApuntesCategoria apuntesCategoria)
        {
            if (!ValidarApuntesCategoria(apuntesCategoria, out string error))
                return (null, error);

            using (var scope = scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();  // Esto es para acceder al ApplicationDbContext desde singleton

                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        ApuntesCategoria objeto = await db.ApuntesCategoria.AsNoTracking().Where(x => x.id == apuntesCategoria.id).FirstOrDefaultAsync();
                        objeto.titulo = apuntesCategoria.titulo;

                        db.Entry(objeto).State = EntityState.Modified; // Actualizar ApuntesCategoria 

                        await db.SaveChangesAsync();
                        await dbContextTransaction.CommitAsync();

                        // Actualiza el elemento de la lista unica
                        ApuntesCategoria elementoLista = Lista.Where(x => x.id == objeto.id).FirstOrDefault();
                        elementoLista.titulo = objeto.titulo;

                        return (apuntesCategoria, "");
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();   // No se realizan los cambios 

                        error = ex.InnerException != null && ex.InnerException.InnerException != null ?
                            ex.InnerException.InnerException.Message :  // Lanza errores SQL 
                            ex.Message;

                        return (null, error);
                    }
                }
            }
        }

        public async Task<(bool, string)> Eliminar(int id)
        {
            string mensajeError = "";

            using (var scope = scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();  // Esto es para acceder al ApplicationDbContext desde singleton

                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        ApuntesCategoria apuntesCategoria = await db.ApuntesCategoria.AsNoTracking().Where(c => c.id == id).FirstOrDefaultAsync();

                        db.ApuntesTema.RemoveRange(
                            db.ApuntesTema.Where(c => c.apuntesCategoria.id == id).ToList()
                        );

                        db.Entry(apuntesCategoria).State = EntityState.Deleted;
                        await db.SaveChangesAsync();
                        await dbContextTransaction.CommitAsync();

                        // Remueve de la lista unica
                        ApuntesCategoria elemento = Lista.Single(x => x.id == apuntesCategoria.id);
                        Lista.Remove(elemento);
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();   // No se realizan los cambios 
                        mensajeError = ex.Message;
                    }
                }
            }

            return (string.IsNullOrEmpty(mensajeError), mensajeError);
        }


        public bool ValidarApuntesCategoria(ApuntesCategoria apuntesCategoria, out string mensajeError)
        {
            mensajeError = "";  // Inicializa el out como un string vacio

            if (apuntesCategoria.titulo == null)
                mensajeError = "El campo Titulo no posee un valor";

            return string.IsNullOrEmpty(mensajeError);  // El retorno booleano dependera de si se encontro un error o no
        }
    }
}
