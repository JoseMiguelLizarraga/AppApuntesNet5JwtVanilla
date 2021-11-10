using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Models;
using Mappings;
using Util;

namespace Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private List<ApuntesCategorium> Lista { get; set; }  // Esta lista se llena solo una vez consultando la base de datos, y queda en memoria. Despues solo se edita

        public CategoriaService(IServiceScopeFactory scopeFactory)  // Esto solo se llama una vez
        {
            this.scopeFactory = scopeFactory;
            LlenarLista();
        }

        public void LlenarLista()  // Esto solo se llama una vez
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppApuntesNet5Context>();  // Esto es para acceder al ApplicationDbContext desde singleton
                Lista = db.ApuntesCategoria.ToList();
            }
        }

        public DataTableDTO LlenarDataTableApuntesCategoria(ApuntesCategorium apuntesCategoria, int inicio, int registrosPorPagina)
        {
            if (apuntesCategoria == null) apuntesCategoria = new ApuntesCategorium();  // En caso de que sea nulo se inicializa 

            IQueryable<ApuntesCategorium> consulta = Lista.AsQueryable();

            if (apuntesCategoria.Titulo != null)
                consulta = consulta.Where(a => a.Titulo.ToLower().Contains(apuntesCategoria.Titulo.ToLower()));

            List<ApuntesCategorium> lista = consulta.OrderBy(x => x.Id).Skip(inicio).Take(registrosPorPagina).ToList();

            return new DataTableDTO()
            {
                RecordsFiltered = Lista.Count,
                RecordsTotal = Lista.Count,
                Data = lista.Select(Mapper.ToDTO).ToList()
            };
        }

        public Select2DTO LlenarSelect2(string busqueda, int registrosPorPagina, int numeroPagina)
        {
            IQueryable<ApuntesCategorium> consulta = Lista.AsQueryable();

            if (!string.IsNullOrEmpty(busqueda))
                consulta = consulta.Where(a => a.Titulo.ToLower().Contains(busqueda.ToLower()));

            return new Select2DTO()
            {
                Total = Lista.Count,
                Results = consulta.Skip((numeroPagina - 1) * registrosPorPagina).Take(registrosPorPagina).Select(a => new { id = a.Id, text = a.Titulo }).ToList()
            };
        }

        public ApuntesCategorium BuscarPorId(int id)
        {
            return Lista.FirstOrDefault(x => x.Id == id);
        }

        public async Task<(ApuntesCategorium, ExcepcionCapturada)> GuardarLogo(ApuntesCategorium objeto)
        {
            try
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<AppApuntesNet5Context>();  // Esto es para acceder al ApplicationDbContext desde singleton

                    ApuntesCategorium categoria = await db.ApuntesCategoria.FirstOrDefaultAsync(x => x.Id == objeto.Id);
                    categoria.Logo = objeto.Logo;   // Es de tipo byte[]
                    categoria.TipoLogo = objeto.TipoLogo;
                    db.SaveChanges();

                    // Actualiza el elemento de la lista unica
                    ApuntesCategorium elementoLista = Lista.Where(x => x.Id == objeto.Id).FirstOrDefault();
                    elementoLista.Logo = objeto.Logo;
                    elementoLista.TipoLogo = objeto.TipoLogo;

                    return (categoria, null);
                }
            }
            catch (Exception ex)
            {
                return (null, ExcepcionesHelper.ObtenerExcepcion(ex));
            }
        }

        public async Task<(ApuntesCategorium, ExcepcionCapturada)> Guardar(ApuntesCategorium objeto)
        {
            if (!ValidarApuntesCategoria(objeto, out string error))
                return (null, ExcepcionesHelper.GenerarExcepcion(error, 400));

            using (var scope = scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppApuntesNet5Context>();  // Esto es para acceder al ApplicationDbContext desde singleton

                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        ApuntesCategorium apuntesCategoria = new ApuntesCategorium();
                        apuntesCategoria.Titulo = objeto.Titulo;

                        await db.ApuntesCategoria.AddAsync(apuntesCategoria); // Insertar 

                        await db.SaveChangesAsync();
                        await dbContextTransaction.CommitAsync();

                        Lista.Add(apuntesCategoria);   // Se agrega a la lista unica

                        return (apuntesCategoria, null);
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();

                        ExcepcionCapturada excepcion = ExcepcionesHelper.ObtenerExcepcion(ex);

                        if (excepcion.MensajeError.Contains("duplicate key")) 
                        {
                            excepcion.MensajeError = "El título de la categoría debe ser único e irrepetible";
                            excepcion.Status = 400;
                        }   

                        return (null, excepcion);
                    }
                }
            }
        }


        public async Task<(ApuntesCategorium, ExcepcionCapturada)> Actualizar(ApuntesCategorium apuntesCategoria)
        {
            if (!ValidarApuntesCategoria(apuntesCategoria, out string error))
                return (null, ExcepcionesHelper.GenerarExcepcion(error, 400));

            using (var scope = scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppApuntesNet5Context>();  // Esto es para acceder al ApplicationDbContext desde singleton

                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        ApuntesCategorium objeto = await db.ApuntesCategoria.AsNoTracking().Where(x => x.Id == apuntesCategoria.Id).FirstOrDefaultAsync();
                        objeto.Titulo = apuntesCategoria.Titulo;

                        db.Entry(objeto).State = EntityState.Modified; // Actualizar ApuntesCategoria 

                        await db.SaveChangesAsync();
                        await dbContextTransaction.CommitAsync();

                        // Actualiza el elemento de la lista unica
                        ApuntesCategorium elementoLista = Lista.Where(x => x.Id == objeto.Id).FirstOrDefault();
                        elementoLista.Titulo = objeto.Titulo;

                        return (apuntesCategoria, null);
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();   // No se realizan los cambios 

                        ExcepcionCapturada excepcion = ExcepcionesHelper.ObtenerExcepcion(ex);

                        if (excepcion.MensajeError.Contains("duplicate key"))
                        {
                            excepcion.MensajeError = "El título de la categoría debe ser único e irrepetible";
                            excepcion.Status = 400;
                        }

                        return (null, excepcion);
                    }
                }
            }
        }

        public async Task<(bool, ExcepcionCapturada)> Eliminar(int id)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppApuntesNet5Context>();  // Esto es para acceder al ApplicationDbContext desde singleton

                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        ApuntesCategorium apuntesCategoria = await db.ApuntesCategoria.AsNoTracking().Where(c => c.Id == id).FirstOrDefaultAsync();

                        db.ApuntesTemas.RemoveRange(
                            db.ApuntesTemas.Where(c => c.Categoria.Id == id).ToList()
                        );

                        db.Entry(apuntesCategoria).State = EntityState.Deleted;
                        await db.SaveChangesAsync();
                        await dbContextTransaction.CommitAsync();

                        // Remueve de la lista unica
                        ApuntesCategorium elemento = Lista.Single(x => x.Id == apuntesCategoria.Id);
                        Lista.Remove(elemento);

                        return (true, null);
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();   // No se realizan los cambios 

                        ExcepcionCapturada excepcion = ExcepcionesHelper.ObtenerExcepcion(ex);
                        return (false, excepcion);
                    }
                }
            }
        }


        public bool ValidarApuntesCategoria(ApuntesCategorium apuntesCategoria, out string mensajeError)
        {
            mensajeError = "";  // Inicializa el out como un string vacio

            if (apuntesCategoria.Titulo == null)
                mensajeError = "El campo Titulo no posee un valor";

            return string.IsNullOrEmpty(mensajeError);  // El retorno booleano dependera de si se encontro un error o no
        }
    }
}
