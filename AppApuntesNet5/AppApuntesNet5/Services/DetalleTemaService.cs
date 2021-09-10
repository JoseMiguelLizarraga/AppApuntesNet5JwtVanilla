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

		public List<ApuntesDetalleTema> Listar()
		{
			return db.ApuntesDetalleTema.ToList();
		}

		public ApuntesDetalleTema BuscarPorId(int id)
		{
			var v = (from c in db.ApuntesDetalleTema
			.Include(i => i.apuntesTema)
				where c.id == id
				select c);

			return v.Cast<ApuntesDetalleTema>().FirstOrDefault();
		}

		public DataTableDTO LlenarDataTableApuntesDetalleTema(ApuntesDetalleTema apuntesDetalleTema, int inicio, int registrosPorPagina)
		{
			if (apuntesDetalleTema == null) apuntesDetalleTema = new ApuntesDetalleTema();  // En caso de que sea nulo se inicializa 

			IQueryable<ApuntesDetalleTema> v = (from a in db.ApuntesDetalleTema
				.Include(c => c.apuntesTema)
				.Include(c => c.apuntesTema.apuntesCategoria)
			select a);

			if (apuntesDetalleTema.apuntesTema != null && apuntesDetalleTema.apuntesTema.apuntesCategoria != null && apuntesDetalleTema.apuntesTema.apuntesCategoria.id != 0)
				v = v.Where(a => a.apuntesTema.apuntesCategoria.id == apuntesDetalleTema.apuntesTema.apuntesCategoria.id);
			
			if (apuntesDetalleTema.apuntesTema != null && apuntesDetalleTema.apuntesTema.id != 0)
				v = v.Where(a => a.apuntesTemaId == apuntesDetalleTema.apuntesTema.id);
		
			if (!string.IsNullOrEmpty(apuntesDetalleTema.rutaFoto))
				v = v.Where(a => a.rutaFoto.Contains(apuntesDetalleTema.rutaFoto));
			
			if (!string.IsNullOrEmpty(apuntesDetalleTema.titulo))
				v = v.Where(a => a.titulo.Contains(apuntesDetalleTema.titulo));
			

			int totalRegistros = v.Count();
			v = v.OrderBy(x => x.id).Skip(inicio).Take(registrosPorPagina);

			return new DataTableDTO() { RecordsFiltered = totalRegistros, RecordsTotal = totalRegistros, Data = v.ToList() };
		}

		public (ApuntesDetalleTema, string) Guardar(ApuntesDetalleTema objeto)
		{
			if (! ValidarApuntesDetalleTema(objeto, out string error))
				return (null, error);

			using (var dbContextTransaction = db.Database.BeginTransaction())
			{
				try
				{
					ApuntesDetalleTema model = new ApuntesDetalleTema() { 
						apuntesTemaId = objeto.apuntesTema.id,
						titulo = objeto.titulo,
						contenido = objeto.contenido,
						rutaFoto = objeto.rutaFoto
					};

					db.ApuntesDetalleTema.Add(model); // Insertar 

					db.SaveChanges();
					dbContextTransaction.Commit();
					return (model, "");

				}
				catch (Exception ex)
				{
					dbContextTransaction.Rollback();

					return (null, ex.InnerException != null && ex.InnerException.InnerException != null ?
						ex.InnerException.InnerException.Message :  // Lanza errores SQL 
						ex.Message);
				}
			}	
		}

		public (ApuntesDetalleTema, string) Actualizar(ApuntesDetalleTema objeto)
		{
			if (!ValidarApuntesDetalleTema(objeto, out string error))
				return (null, error);

			using (var dbContextTransaction = db.Database.BeginTransaction())
			{
				try
				{
					ApuntesDetalleTema model = db.ApuntesDetalleTema.AsNoTracking().Where(x => x.id == objeto.id).FirstOrDefault();

					model.apuntesTema = objeto.apuntesTema;
					model.rutaFoto = objeto.rutaFoto;
					model.contenido = objeto.contenido;
					model.titulo = objeto.titulo;

					db.Entry(model).State = EntityState.Modified; // Actualizar ApuntesDetalleTema 

					db.SaveChanges();
					dbContextTransaction.Commit();
					return (model, "");
				}
				catch (Exception ex)
				{
					dbContextTransaction.Rollback();   // No se realizan los cambios 

					return (null, ex.InnerException != null && ex.InnerException.InnerException != null ?
						ex.InnerException.InnerException.Message :  // Lanza errores SQL 
						ex.Message);
				}
			}
		}

		public bool Eliminar(int id, out string mensajeError)
		{
			mensajeError = "";  // Inicializa el out como un string vacio

			using (var dbContextTransaction = db.Database.BeginTransaction())
			{
				try
				{
					ApuntesDetalleTema apuntesDetalleTema = db.ApuntesDetalleTema.AsNoTracking().Where(c => c.id == id).FirstOrDefault();

					db.Entry(apuntesDetalleTema).State = EntityState.Deleted;
					db.SaveChanges();
					dbContextTransaction.Commit();
				}
				catch (Exception ex)
				{
					dbContextTransaction.Rollback();   // No se realizan los cambios 
					mensajeError = ex.Message;
				}
			}

			return string.IsNullOrEmpty(mensajeError);
		}


		public bool ValidarApuntesDetalleTema(ApuntesDetalleTema apuntesDetalleTema, out string mensajeError)
		{
			mensajeError = "";  // Inicializa el out como un string vacio

			if (apuntesDetalleTema.apuntesTema == null)
				mensajeError = "El campo apuntesTema no posee un valor";

			//else if (string.IsNullOrEmpty(apuntesDetalleTema.rutaFoto))
			//	mensajeError = "El campo rutaFoto no posee un valor";

			else if(apuntesDetalleTema.contenido == null)
				mensajeError = "El campo contenido no posee un valor";

			else if(string.IsNullOrEmpty(apuntesDetalleTema.titulo))
				mensajeError = "El campo titulo no posee un valor";

			return string.IsNullOrEmpty(mensajeError);  // El retorno booleano dependera de si se encontro un error o no
		}
	}
}
