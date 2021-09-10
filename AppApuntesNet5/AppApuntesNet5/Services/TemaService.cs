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

		public List<ApuntesTema> Listar()
		{
			return db.ApuntesTema.ToList();
		}

		public ApuntesTema BuscarPorId(int id)
		{
			var v = (from c in db.ApuntesTema
			.Include(i => i.apuntesCategoria)
			.Include(i => i.listaApuntesDetalleTema)
					 where c.id == id
					 select c);

			return v.Cast<ApuntesTema>().FirstOrDefault();
		}

		public DataTableDTO LlenarDataTableApuntesTema(ApuntesTema apuntesTema, int inicio, int registrosPorPagina)
		{
			if (apuntesTema == null) apuntesTema = new ApuntesTema();  // En caso de que sea nulo se inicializa 

			IQueryable<ApuntesTema> v = (from a in db.ApuntesTema
				.Include(c => c.apuntesCategoria)
			select a);

			if (apuntesTema.apuntesCategoria != null)
				v = v.Where(a => a.apuntesCategoriaId == apuntesTema.apuntesCategoria.id);
		
			if (! string.IsNullOrEmpty(apuntesTema.titulo))
				v = v.Where(a => a.titulo.Contains(apuntesTema.titulo));

			int totalRegistros = v.Count();
			v = v.OrderBy(x => x.id).Skip(inicio).Take(registrosPorPagina);

			return new DataTableDTO() { RecordsFiltered = totalRegistros, RecordsTotal = totalRegistros, Data = v.ToList() };
		}

		public Select2DTO LlenarSelect2(string busqueda, int registrosPorPagina, int numeroPagina, int idApuntesCategoria)
		{
			object dataSalida = null;
			int cantidadRegistros = 0;

			IQueryable<ApuntesTema> consulta = (from a in db.ApuntesTema select a);

			if (idApuntesCategoria != 0)  // Extra
				consulta = consulta.Where(a => a.apuntesCategoriaId == idApuntesCategoria);

			if (!string.IsNullOrEmpty(busqueda))
				consulta = consulta.Where(a => a.titulo.Contains(busqueda));

			cantidadRegistros = consulta.Count();
			consulta.Skip((numeroPagina - 1) * registrosPorPagina).Take(registrosPorPagina);
			dataSalida = consulta.ToList().Select(a => new { id = a.id, text = a.titulo }).ToList();

			return new Select2DTO() { Total = cantidadRegistros, Results = dataSalida};
		}

		public (ApuntesTema, string) Guardar(ApuntesTema apuntesTema)
		{
            if (! ValidarApuntesTema(apuntesTema, out string error)) 
				return (null, error);

			// Esto previene el error de la creacion de un nuevo registro de tipo ApuntesCategoria
			apuntesTema.apuntesCategoriaId = apuntesTema.apuntesCategoria.id;
			apuntesTema.apuntesCategoria = null;   // Se elimina ya que trae id, y dara error

			using (var dbContextTransaction = db.Database.BeginTransaction())
			{
				try
				{
					if (apuntesTema.listaApuntesDetalleTema != null && apuntesTema.listaApuntesDetalleTema.Count > 0)  // Referencias Cruzadas de la entidad ApuntesDetalleTema 
					{
						foreach (ApuntesDetalleTema detalle in apuntesTema.listaApuntesDetalleTema)
						{
							detalle.apuntesTema = apuntesTema;
							db.ApuntesDetalleTema.Add(detalle);
						}
					}

					db.ApuntesTema.Add(apuntesTema); // Insertar 

					db.SaveChanges();
					dbContextTransaction.Commit();
					return (apuntesTema, "");

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

		public (ApuntesTema, string) Actualizar(ApuntesTema apuntesTema)
		{
			if (!ValidarApuntesTema(apuntesTema, out string error))
				return (null, error);

			using (var dbContextTransaction = db.Database.BeginTransaction())
			{
				try
				{
					ApuntesTema objeto = db.ApuntesTema.AsNoTracking().Where(x => x.id == apuntesTema.id).FirstOrDefault();

					//objeto.apuntesCategoria = apuntesTema.apuntesCategoria;
					objeto.apuntesCategoriaId = apuntesTema.apuntesCategoria.id;
					objeto.titulo = apuntesTema.titulo;


					db.Entry(objeto).State = EntityState.Modified; // Actualizar ApuntesTema 

					// Referencias Cruzadas de la entidad ApuntesDetalleTema 

					List<ApuntesDetalleTema> listaApuntesDetalleTema = apuntesTema.listaApuntesDetalleTema.ToList();

					if (listaApuntesDetalleTema.Count() > 0)
					{
						if (listaApuntesDetalleTema.Count == 1 && listaApuntesDetalleTema.Any(c => c.id == 0))  // Si solo se recibio un detalle y es nuevo 
						{
							// Borra los anteriores registros 
							db.ApuntesDetalleTema.RemoveRange(
								db.ApuntesDetalleTema.Where(c => c.apuntesTema.id == apuntesTema.id).ToList()
							);
						}
						if (listaApuntesDetalleTema.Any(c => c.id > 0))  // Si al menos un detalle trae id  
						{
							// Se borran los detalles almacenados en la base de datos cuyos ids no esten en los ids de los detalles recibidos 
							List<int> listaIdDetalles = listaApuntesDetalleTema.Where(c => c.id > 0).Select(c => c.id).ToList();

							db.ApuntesDetalleTema.RemoveRange(
								db.ApuntesDetalleTema.Where(c => !listaIdDetalles.Contains(c.id) &&
								c.apuntesTema.id == apuntesTema.id).ToList()
							);
						}
						foreach (ApuntesDetalleTema detalle in listaApuntesDetalleTema.ToList())
						{
							if (detalle.id > 0)  // Si trae id significa que esta almacenado. Se actualiza 
							{
								ApuntesDetalleTema detalleBuscado = db.ApuntesDetalleTema.AsNoTracking().Where(c => c.id == detalle.id).FirstOrDefault();
								detalleBuscado.rutaFoto = detalle.rutaFoto;
								detalleBuscado.contenido = detalle.contenido;
								detalleBuscado.titulo = detalle.titulo;
								db.Entry(detalleBuscado).State = EntityState.Modified;
							}
							else  // Si no esta guardado el detalle recorrido, se agrega 
							{
								detalle.apuntesTema = objeto;
								db.ApuntesDetalleTema.Add(detalle);
							}
						}
					}
					else  // Si no se recibieron detalles de la clase ApuntesDetalleTema 
					{
						// Borra los anteriores registros 
						db.ApuntesDetalleTema.RemoveRange(
							db.ApuntesDetalleTema.Where(c => c.apuntesTema.id == apuntesTema.id).ToList()
						);
					}

					db.SaveChanges();
					dbContextTransaction.Commit();
					return (apuntesTema, "");
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
					ApuntesTema apuntesTema = db.ApuntesTema.AsNoTracking().Where(c => c.id == id).FirstOrDefault();

					db.ApuntesDetalleTema.RemoveRange(
						db.ApuntesDetalleTema.Where(c => c.apuntesTema.id == id).ToList()
					);

					db.Entry(apuntesTema).State = EntityState.Deleted;
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

		public void ProcesarDatosExcel(List<ApuntesTema> elementosInsertados, List<ApuntesTema> elementosActualizados)
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

		public bool ValidarApuntesTema(ApuntesTema apuntesTema, out string mensajeError)
		{
			mensajeError = "";  // Inicializa el out como un string vacio

			if (apuntesTema.apuntesCategoria == null)
				mensajeError = "El campo apuntesCategoria no posee un valor";

			else if (string.IsNullOrEmpty(apuntesTema.titulo))
				mensajeError = "El campo titulo no posee un valor";

			else 
			{
                foreach (ApuntesDetalleTema c in apuntesTema.listaApuntesDetalleTema)
                {
					if (c.contenido == null) {
						mensajeError = "Un detalle de tipo ApuntesDetalleTema tiene vacío el campo contenido";
						break;
					}
					if (string.IsNullOrEmpty(c.titulo)) {
						mensajeError = "Un detalle de tipo ApuntesDetalleTema tiene vacío el campo titulo";
						break;
					}
				}
			}

			return string.IsNullOrEmpty(mensajeError);  // El retorno booleano dependera de si se encontro un error o no
		}
	}
}
