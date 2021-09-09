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

		public List<ApuntesTema> listar()
		{
			return db.ApuntesTema.ToList();
		}

		public ApuntesTema buscarPorId(int id)
		{
			/*
			return db.ApuntesTema
				.Include(i => i.apuntesCategoria)
				.Include(i => i.listaApuntesDetalleTema)
				.Where(c => c.id == id)
				.FirstOrDefault();
			*/

			var v = (from c in db.ApuntesTema
			.Include(i => i.apuntesCategoria)
			.Include(i => i.listaApuntesDetalleTema)
					 where c.id == id
					 select c);

			return v.Cast<ApuntesTema>().FirstOrDefault();
		}

		public IDictionary<string, object> llenarDataTableApuntesTema(ApuntesTema apuntesTema, int inicio, int registrosPorPagina)
		{
			if (apuntesTema == null) apuntesTema = new ApuntesTema();  // En caso de que sea nulo se inicializa 

			IQueryable<ApuntesTema> v = (from a in db.ApuntesTema
				.Include(c => c.apuntesCategoria)
										 select a);

			if (apuntesTema.apuntesCategoria != null)
			{
				v = v.Where(a => a.apuntesCategoriaId == apuntesTema.apuntesCategoria.id);
			}
			if (!string.IsNullOrEmpty(apuntesTema.titulo))
			{
				v = v.Where(a => a.titulo.Contains(apuntesTema.titulo));
			}

			int totalRegistros = v.Count();
			v = v.OrderBy(x => x.id).Skip(inicio).Take(registrosPorPagina);  // El OrderBy es necesario para poder funcionar 

			List<ApuntesTema> listaApuntesTema = v.ToList();

			IDictionary<string, object> respuesta = new Dictionary<string, object>();

			respuesta["recordsFiltered"] = totalRegistros;
			respuesta["recordsTotal"] = totalRegistros;
			respuesta["data"] = listaApuntesTema;

			return respuesta;
		}

		public IDictionary<string, object> llenarSelect2(String clase, String busqueda, int registrosPorPagina, int numeroPagina)
		{
			object dataSalida = null;
			int cantidadRegistros = 0;

			if (clase == "ApuntesCategoria")
			{
				IQueryable<ApuntesCategoria> consulta = (from a in db.ApuntesCategoria select a);

				if (!string.IsNullOrEmpty(busqueda))
					consulta = consulta.Where(a => a.titulo.Contains(busqueda));

				cantidadRegistros = consulta.Count();
				consulta.Skip((numeroPagina - 1) * registrosPorPagina).Take(registrosPorPagina);
				dataSalida = consulta.ToList().Select(a => new { id = a.id, text = a.titulo }).ToList();
			}

			IDictionary<string, object> respuesta = new Dictionary<string, object>();
			respuesta["Total"] = cantidadRegistros;
			respuesta["Results"] = dataSalida;
			return respuesta;
		}

		public ApuntesTema guardar(ApuntesTema apuntesTema)
		{
			validarApuntesTema(apuntesTema);  // Validación 

			// Esto previene el error de la creacion de un nuevo registro de tipo ApuntesCategoria
			apuntesTema.apuntesCategoriaId = apuntesTema.apuntesCategoria.id;
			apuntesTema.apuntesCategoria = null;   // Se elimina ya que trae id, y dara error


			using (var context = db)
			{
				using (var dbContextTransaction = context.Database.BeginTransaction())
				{
					try
					{
						if (apuntesTema.listaApuntesDetalleTema != null && apuntesTema.listaApuntesDetalleTema.Count > 0)  // Referencias Cruzadas de la entidad ApuntesDetalleTema 
						{
							foreach (ApuntesDetalleTema detalle in apuntesTema.listaApuntesDetalleTema)
							{
								detalle.apuntesTema = apuntesTema;
								context.ApuntesDetalleTema.Add(detalle);
							}
						}

						context.ApuntesTema.Add(apuntesTema); // Insertar 

						context.SaveChanges();
						dbContextTransaction.Commit();
						return apuntesTema;

					}
					catch (Exception ex)
					{
						dbContextTransaction.Rollback();

						throw new Exception(ex.InnerException != null && ex.InnerException.InnerException != null ?
							ex.InnerException.InnerException.Message :  // Lanza errores SQL 
							ex.Message
						);
					}
				}
			}
		}

		public ApuntesTema actualizar(ApuntesTema apuntesTema)
		{
			validarApuntesTema(apuntesTema);  // Validación 

			using (var context = db)
			{
				using (var dbContextTransaction = context.Database.BeginTransaction())
				{
					try
					{
						ApuntesTema objeto = context.ApuntesTema.AsNoTracking().Where(x => x.id == apuntesTema.id).FirstOrDefault();

						//objeto.apuntesCategoria = apuntesTema.apuntesCategoria;
						objeto.apuntesCategoriaId = apuntesTema.apuntesCategoria.id;
						objeto.titulo = apuntesTema.titulo;


						context.Entry(objeto).State = EntityState.Modified; // Actualizar ApuntesTema 


						// Referencias Cruzadas de la entidad ApuntesDetalleTema 

						List<ApuntesDetalleTema> listaApuntesDetalleTema = apuntesTema.listaApuntesDetalleTema.ToList();

						if (listaApuntesDetalleTema.Count() > 0)
						{
							if (listaApuntesDetalleTema.Count == 1 && listaApuntesDetalleTema.Any(c => c.id == 0))  // Si solo se recibio un detalle y es nuevo 
							{
								// Borra los anteriores registros 
								context.ApuntesDetalleTema.RemoveRange(
									context.ApuntesDetalleTema.Where(c => c.apuntesTema.id == apuntesTema.id).ToList()
								);
							}
							if (listaApuntesDetalleTema.Any(c => c.id > 0))  // Si al menos un detalle trae id  
							{
								// Se borran los detalles almacenados en la base de datos cuyos ids no esten en los ids de los detalles recibidos 
								List<int> listaIdDetalles = listaApuntesDetalleTema.Where(c => c.id > 0).Select(c => c.id).ToList();

								context.ApuntesDetalleTema.RemoveRange(
									context.ApuntesDetalleTema.Where(c => !listaIdDetalles.Contains(c.id) &&
									c.apuntesTema.id == apuntesTema.id).ToList()
								);
							}
							foreach (ApuntesDetalleTema detalle in listaApuntesDetalleTema.ToList())
							{
								if (detalle.id > 0)  // Si trae id significa que esta almacenado. Se actualiza 
								{
									ApuntesDetalleTema detalleBuscado = context.ApuntesDetalleTema.AsNoTracking().Where(c => c.id == detalle.id).FirstOrDefault();
									detalleBuscado.rutaFoto = detalle.rutaFoto;
									detalleBuscado.contenido = detalle.contenido;
									detalleBuscado.titulo = detalle.titulo;
									context.Entry(detalleBuscado).State = EntityState.Modified;
								}
								else  // Si no esta guardado el detalle recorrido, se agrega 
								{
									detalle.apuntesTema = objeto;
									context.ApuntesDetalleTema.Add(detalle);
								}
							}
						}
						else  // Si no se recibieron detalles de la clase ApuntesDetalleTema 
						{
							// Borra los anteriores registros 
							context.ApuntesDetalleTema.RemoveRange(
								context.ApuntesDetalleTema.Where(c => c.apuntesTema.id == apuntesTema.id).ToList()
							);
						}

						context.SaveChanges();
						dbContextTransaction.Commit();
						return apuntesTema;
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

		public void eliminar(int id)
		{
			using (var context = db)
			{
				using (var dbContextTransaction = context.Database.BeginTransaction())
				{
					try
					{
						ApuntesTema apuntesTema = context.ApuntesTema.AsNoTracking().Where(c => c.id == id).FirstOrDefault();

						context.ApuntesDetalleTema.RemoveRange(
							context.ApuntesDetalleTema.Where(c => c.apuntesTema.id == id).ToList()
						);

						context.Entry(apuntesTema).State = EntityState.Deleted;
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

		public void procesarDatosExcel(List<ApuntesTema> elementosInsertados, List<ApuntesTema> elementosActualizados)
		{
			using (var context = db)
			{
				using (var dbContextTransaction = context.Database.BeginTransaction())
				{
					try
					{
						foreach (ApuntesTema apuntesTema in elementosInsertados) { context.ApuntesTema.Add(apuntesTema); }
						foreach (ApuntesTema apuntesTema in elementosActualizados) { context.Entry(apuntesTema).State = EntityState.Modified; }

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

		public void validarApuntesTema(ApuntesTema apuntesTema)
		{
			if (apuntesTema.apuntesCategoria == null)
				throw new ArgumentException("El campo apuntesCategoria no posee un valor");

			if (string.IsNullOrEmpty(apuntesTema.titulo))
				throw new ArgumentException("El campo titulo no posee un valor");

			// Se validan las referencias cruzadas de la entidad ApuntesDetalleTema 

			apuntesTema.listaApuntesDetalleTema.ToList().ForEach(c =>
			{
				//if (string.IsNullOrEmpty(c.rutaFoto)) 
				//	throw new ArgumentException("Un detalle de tipo ApuntesDetalleTema tiene vacío el campo rutaFoto"); 

				if (c.contenido == null)
					throw new ArgumentException("Un detalle de tipo ApuntesDetalleTema tiene vacío el campo contenido");

				if (string.IsNullOrEmpty(c.titulo))
					throw new ArgumentException("Un detalle de tipo ApuntesDetalleTema tiene vacío el campo titulo");

			});


		}

	}
}
