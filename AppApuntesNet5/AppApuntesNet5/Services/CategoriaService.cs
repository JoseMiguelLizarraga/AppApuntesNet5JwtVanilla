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
		private List<ApuntesCategoria> Lista { get; set; }
		
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

			Func<IQueryable<ApuntesCategoria>> paginarDatatable = () =>
			{
				IQueryable<ApuntesCategoria> consulta = Lista.AsQueryable();

				if (apuntesCategoria.titulo != null)
					consulta = consulta.Where(a => a.titulo.ToLower().Contains(apuntesCategoria.titulo.ToLower()));

				consulta = consulta.OrderBy(x => x.id).Skip(inicio).Take(registrosPorPagina);  // El OrderBy es necesario para poder funcionar 
				return consulta;
			};

			return new DataTableDTO() { 
				RecordsFiltered = Lista.Count, 
				RecordsTotal = Lista.Count,
				Data = paginarDatatable().ToList()
			};
		}

		public Select2DTO LlenarSelect2(string busqueda, int registrosPorPagina, int numeroPagina)
		{
			Func<int, int, IQueryable<ApuntesCategoria>> paginarLista = (registrosPorPagina, numeroPagina) =>
			{
				IQueryable<ApuntesCategoria> consulta = Lista.AsQueryable();

				if (! string.IsNullOrEmpty(busqueda))
					consulta = consulta.Where(a => a.titulo.ToLower().Contains(busqueda.ToLower()));

				return consulta.Skip((numeroPagina - 1) * registrosPorPagina).Take(registrosPorPagina);
			};

			return new Select2DTO() { 
				Total = Lista.Count, 
				Results = paginarLista(registrosPorPagina, numeroPagina).Select(a => new { id = a.id, text = a.titulo }).ToList()
			};
		}

		/*
		public async Task<(ApuntesDetalleTema, string)> Guardar(ApuntesDetalleTema objeto)
		{
			if (! ValidarApuntesDetalleTema(objeto, out string error))
				return (null, error); 
		*/

		public async Task<(ApuntesCategoria, string)> Guardar(ApuntesCategoria objeto)
		{
			if (! ValidarApuntesCategoria(objeto, out string error))
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

		/*
		public ApuntesCategoria Actualizar(ApuntesCategoria apuntesCategoria)
		{
			ValidarApuntesCategoria(apuntesCategoria);  // Validación 

			using (var context = db)
			{
				using (var dbContextTransaction = context.Database.BeginTransaction())
				{
					try
					{
						ApuntesCategoria objeto = context.ApuntesCategoria.AsNoTracking().Where(x => x.Id == apuntesCategoria.Id).FirstOrDefault();

						objeto.Titulo = apuntesCategoria.Titulo;


						context.Entry(objeto).State = EntityState.Modified; // Actualizar ApuntesCategoria 


						context.SaveChanges();
						dbContextTransaction.Commit();
						return apuntesCategoria;
					}
					catch (Exception ex)
					{
						dbContextTransaction.Rollback();   // No se realizan los cambios 

						throw new Exception(ex.InnerException != null && ex.InnerException.InnerException != null ?
							ex.InnerException.InnerException.Message :  // Lanza errores SQL 
							ex.Message
						);
					}
				}
			}

		}

		public void Eliminar(int id)
		{
			using (var context = db)
			{
				using (var dbContextTransaction = context.Database.BeginTransaction())
				{
					try
					{
						ApuntesCategoria apuntesCategoria = context.ApuntesCategoria.AsNoTracking().Where(c => c.id == id).FirstOrDefault();

						context.ApuntesTema.RemoveRange(
							context.ApuntesTema.Where(c => c.ApuntesCategoria.Id == id).ToList()
						);

						context.Entry(apuntesCategoria).State = EntityState.Deleted;
						context.SaveChanges();
						dbContextTransaction.Commit();
					}
					catch (Exception ex)
					{
						dbContextTransaction.Rollback();   // No se realizan los cambios 
						throw new Exception(ex.Message);
					}
				}
			}

		}
		*/

		public bool ValidarApuntesCategoria(ApuntesCategoria apuntesCategoria, out string mensajeError)
		{
			mensajeError = "";  // Inicializa el out como un string vacio

			if (apuntesCategoria.titulo == null)
				mensajeError = "El campo Titulo no posee un valor";

			return string.IsNullOrEmpty(mensajeError);  // El retorno booleano dependera de si se encontro un error o no
		}
	}
}
