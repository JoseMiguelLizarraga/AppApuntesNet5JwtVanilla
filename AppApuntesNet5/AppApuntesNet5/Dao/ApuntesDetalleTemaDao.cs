using System; 
using Microsoft.EntityFrameworkCore; 
using System.Linq; 
using System.Collections.Generic; 
using AppApuntesNet5.Models;


namespace AppApuntesNet5.Dao
{ 
	public class ApuntesDetalleTemaDao 
	{ 

		private ApplicationDbContext db; 

		public ApuntesDetalleTemaDao(ApplicationDbContext context) 
		{ 
			this.db = context;  
		} 

		public List<ApuntesDetalleTema> listar() 
		{ 
			return db.ApuntesDetalleTema.ToList();  
		} 

		public ApuntesDetalleTema buscarPorId(int id) 
		{ 
			var v = (from c in db.ApuntesDetalleTema
			.Include(i => i.apuntesTema)	 
			where c.id == id select c);

			return v.Cast<ApuntesDetalleTema>().FirstOrDefault();

			//var v = (from c in db.ApuntesDetalleTema where c.id == id select new  
			//{  
			//	rutaFoto = c.rutaFoto, 
			//	id = c.id, 
			//	contenido = c.contenido, 
			//	titulo = c.titulo, 
			//	apuntesTemaId = c.apuntesTemaId
			//}); 

			//ApuntesDetalleTema apuntesDetalleTema = v.ToList().Select(c => new ApuntesDetalleTema 
			//{  
			//	rutaFoto = c.rutaFoto, 
			//	id = c.id, 
			//	contenido = c.contenido, 
			//	titulo = c.titulo, 
			//	apuntesTema = new ApuntesTema { id = c.apuntesTemaId }
			//}).FirstOrDefault(); 

			//return apuntesDetalleTema;  
		} 

		public IDictionary<string, object> llenarDataTableApuntesDetalleTema(ApuntesDetalleTema apuntesDetalleTema, int inicio, int registrosPorPagina) 
		{ 
			if (apuntesDetalleTema == null) apuntesDetalleTema = new ApuntesDetalleTema();  // En caso de que sea nulo se inicializa 

			IQueryable<ApuntesDetalleTema> v = (from a in db.ApuntesDetalleTema
				.Include(c => c.apuntesTema)
				.Include(c => c.apuntesTema.apuntesCategoria)
                select a);

            //var v = (from c in db.ApuntesDetalleTema select new  
            //{  
            //	rutaFoto = c.rutaFoto, 
            //	id = c.id, 
            //	contenido = c.contenido, 
            //	titulo = c.titulo, 
            //	apuntesTemaId = c.apuntesTemaId
            //}); 

            if (apuntesDetalleTema.apuntesTema != null && apuntesDetalleTema.apuntesTema.apuntesCategoria != null && apuntesDetalleTema.apuntesTema.apuntesCategoria.id != 0) {
				v = v.Where(a => a.apuntesTema.apuntesCategoria.id == apuntesDetalleTema.apuntesTema.apuntesCategoria.id);
			}

			if (apuntesDetalleTema.apuntesTema != null && apuntesDetalleTema.apuntesTema.id != 0) { 
				v = v.Where(a => a.apuntesTemaId == apuntesDetalleTema.apuntesTema.id); 
			} 
			if (! string.IsNullOrEmpty(apuntesDetalleTema.rutaFoto)) { 
				v = v.Where(a => a.rutaFoto.Contains(apuntesDetalleTema.rutaFoto)); 
			} 
			//if (apuntesDetalleTema.contenido != null) { 
				//v = v.Where(a => a.contenido == apuntesDetalleTema.contenido); 
			//} 
			if (! string.IsNullOrEmpty(apuntesDetalleTema.titulo)) { 
				v = v.Where(a => a.titulo.Contains(apuntesDetalleTema.titulo)); 
			} 

			int totalRegistros = v.Count(); 
			v = v.OrderBy(x=> x.id).Skip(inicio).Take(registrosPorPagina);  // El OrderBy es necesario para poder funcionar 

			List<ApuntesDetalleTema> listaApuntesDetalleTema = v.ToList();

			//List<ApuntesDetalleTema> listaApuntesDetalleTema = v.ToList().Select(c => new ApuntesDetalleTema 
			//{ 
			//	rutaFoto = c.rutaFoto, 
			//	id = c.id, 
			//	contenido = c.contenido, 
			//	titulo = c.titulo, 
			//	apuntesTema = new ApuntesTema { id = c.apuntesTemaId }
			//}).ToList(); 

			IDictionary<string, object> respuesta = new Dictionary<string, object>(); 

			respuesta["recordsFiltered"] = totalRegistros; 
			respuesta["recordsTotal"] = totalRegistros; 
			respuesta["data"] = listaApuntesDetalleTema; 

			return respuesta; 
		}

		
		public IDictionary<string, object> llenarSelect2(String clase, String busqueda, int registrosPorPagina, int numeroPagina, int idApuntesCategoria) 
		{ 
			object dataSalida = null; 
			int cantidadRegistros = 0;

			if (clase == "ApuntesTema") 
			{ 
				IQueryable<ApuntesTema> consulta = (from a in db.ApuntesTema select a);

                if (idApuntesCategoria != 0) {  // Agregado
					consulta = consulta.Where(a => a.apuntesCategoriaId == idApuntesCategoria);
				}

                if (! string.IsNullOrEmpty(busqueda)) 
					consulta = consulta.Where(a => a.id == int.Parse(busqueda)); // En este ejemplo se busca por el campo id 

				cantidadRegistros = consulta.Count(); 
				consulta.Skip((numeroPagina - 1) * registrosPorPagina).Take(registrosPorPagina); 
				dataSalida = consulta.ToList().Select(a => new { id = a.id, text = a.titulo }).ToList(); 
			} 

			IDictionary<string, object> respuesta = new Dictionary<string, object>(); 
			respuesta["Total"] = cantidadRegistros; 
			respuesta["Results"] = dataSalida; 
			return respuesta; 
		} 

		public ApuntesDetalleTema guardar(ApuntesDetalleTema apuntesDetalleTema) 
		{ 
			validarApuntesDetalleTema(apuntesDetalleTema);  // Validación 

			using (var context = db) 
			{ 
				using (var dbContextTransaction = context.Database.BeginTransaction()) 
				{ 
					try 
					{ 
						context.ApuntesDetalleTema.Add(apuntesDetalleTema); // Insertar 

						context.SaveChanges(); 
						dbContextTransaction.Commit(); 
						return apuntesDetalleTema; 

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

		public ApuntesDetalleTema actualizar(ApuntesDetalleTema apuntesDetalleTema) 
		{ 
			validarApuntesDetalleTema(apuntesDetalleTema);  // Validación 

			using (var context = db) 
			{ 
				using (var dbContextTransaction = context.Database.BeginTransaction()) 
				{ 
					try 
					{ 
						ApuntesDetalleTema objeto = context.ApuntesDetalleTema.AsNoTracking().Where(x => x.id == apuntesDetalleTema.id).FirstOrDefault(); 

						objeto.apuntesTema = apuntesDetalleTema.apuntesTema; 
						objeto.rutaFoto = apuntesDetalleTema.rutaFoto; 
						objeto.contenido = apuntesDetalleTema.contenido; 
						objeto.titulo = apuntesDetalleTema.titulo; 


						context.Entry(objeto).State = EntityState.Modified; // Actualizar ApuntesDetalleTema 


						context.SaveChanges(); 
						dbContextTransaction.Commit(); 
						return apuntesDetalleTema; 
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
						ApuntesDetalleTema apuntesDetalleTema = context.ApuntesDetalleTema.AsNoTracking().Where(c=> c.id == id).FirstOrDefault(); 

						context.Entry(apuntesDetalleTema).State = EntityState.Deleted; 
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

		//public void procesarDatosExcel(List<ApuntesDetalleTema> elementosInsertados, List<ApuntesDetalleTema> elementosActualizados) 
		//{ 
		//	using (var context = db) 
		//	{ 
		//		using (var dbContextTransaction = context.Database.BeginTransaction()) 
		//		{ 
		//			try 
		//			{ 
		//				foreach(ApuntesDetalleTema apuntesDetalleTema in elementosInsertados) {  context.ApuntesDetalleTema.Add(apuntesDetalleTema);  } 
		//				foreach(ApuntesDetalleTema apuntesDetalleTema in elementosActualizados) {  context.Entry(apuntesDetalleTema).State = EntityState.Modified;  } 

		//				context.SaveChanges(); 
		//				dbContextTransaction.Commit(); 
		//			} 
		//			catch (Exception ex) 
		//			{ 
		//				dbContextTransaction.Rollback();   // No se realizan los cambios 
		//				throw new Exception(ex.Message); 
		//			} 
		//		} 
		//	} 

		//} 

		public void validarApuntesDetalleTema(ApuntesDetalleTema apuntesDetalleTema) 
		{ 
			if (apuntesDetalleTema.apuntesTema == null) 
				throw new ArgumentException("El campo apuntesTema no posee un valor"); 

			if (string.IsNullOrEmpty(apuntesDetalleTema.rutaFoto)) 
				throw new ArgumentException("El campo rutaFoto no posee un valor"); 

			if (apuntesDetalleTema.contenido == null) 
				throw new ArgumentException("El campo contenido no posee un valor"); 

			if (string.IsNullOrEmpty(apuntesDetalleTema.titulo)) 
				throw new ArgumentException("El campo titulo no posee un valor"); 


		} 

	} 


 
}
