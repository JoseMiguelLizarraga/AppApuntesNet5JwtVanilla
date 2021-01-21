using System; 
using System.Collections.Generic; 
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc; 
using System.Net; 
using System.Linq; 
using System.Net.Http; 
using System.Net.Http.Headers; 
using AppApuntesNet5.Dao; 
using AppApuntesNet5.Models; 

//using OfficeOpenXml;	// Usa la libreria EPPlus.Core para generar excel. Version 1.5.4 
//using System.IO;		// Generar archivos con MemoryStream 
//using OpenHtmlToPdf;	// Generar PDF 

//using ExcelDataReader;  // Usa la libreria ExcelDataReader.DataSet para leer archivos excel 
//using System.Web;       // Lectura de archivo excel dentro de System.Web.HttpContext.Current.Request 
//using System.Data;      // Lectura de archivo excel 

namespace AppApuntesNet5.Controllers
{ 
	[Route("ApuntesDetalleTema")] 
	[ApiController] 
	public class ApuntesDetalleTemaController : ControllerBase 
	{ 

		private ApuntesDetalleTemaDao dao = null; 
		private readonly ApplicationDbContext context; 

		public ApuntesDetalleTemaController(ApplicationDbContext context) 
		{ 
			this.context = context;  // Esto es para poder usar el ApplicationDbContext 
			this.dao = new ApuntesDetalleTemaDao(this.context); 
		} 

		[HttpGet] 
		public IActionResult Get() 
		{ 
			try { 
				return Ok(dao.listar()); 
			} 
			catch (Exception) 
			{ 
				return NotFound(); 
			} 
		} 

		[ApiExplorerSettings(IgnoreApi = true)]  // Asi swagger ignora este metodo 
		public IDictionary<string, object> llenarInformacionDataTable(ApuntesDetalleTema apuntesDetalleTema) 
		{ 
			int inicio = int.Parse(HttpContext.Request.Query["start"].ToString()); 
			int registrosPorPagina = int.Parse(HttpContext.Request.Query["length"].ToString());  // Es la cantidad de registros por pagina 


			IDictionary<string, object> respuesta = dao.llenarDataTableApuntesDetalleTema(apuntesDetalleTema, inicio, registrosPorPagina); 
			return respuesta; 
		} 

		[HttpPost] 
		[Route("llenarDataTable")] 
		public IActionResult llenarDataTable([FromQuery] ApuntesDetalleTema apuntesDetalleTema) 
		{ 
			try { 
				IDictionary<string, object> mapa = llenarInformacionDataTable(apuntesDetalleTema); 
				return Ok(mapa); 
			} 
			catch (Exception ex) 
			{ 
				return BadRequest(ex.Message); 
			} 
		} 

		[HttpGet] // Metodo usado para llenar combobox en proyectos Angular  
		[Route("obtenerListasBuscador")] 
		public object obtenerListasBuscador() 
		{ 
			try { 
				IDictionary<string, object> respuesta = new Dictionary<string, object>(); 
				respuesta["listaApuntesTema"] = this.context.ApuntesTema.Select(c => new { c.id }); 
				return respuesta; 
			} 
			catch (Exception ex) 
			{ 
				return BadRequest(ex.Message); 
			} 
		} 

		[HttpGet] 
		[Route("llenarSelect2")] 
		public object llenarSelect2(String clase, String busqueda, int registrosPorPagina, int numeroPagina, int idApuntesCategoria) 
		{ 
			try { 
				return dao.llenarSelect2(clase, busqueda, registrosPorPagina, numeroPagina, idApuntesCategoria); 
			} 
			catch (Exception ex) 
			{ 
				return BadRequest(ex.Message); 
			} 
		} 

		[HttpGet] 
		[Route("{id:int}")] 
		public IActionResult buscarPorId(int id) 
		{ 
			try { 
				return Ok(dao.buscarPorId(id)); 
			} 
			catch (Exception ex) 
			{ 
				return NotFound(ex.Message); 
			} 
		} 

		[HttpPost] 
		public IActionResult Post(ApuntesDetalleTema apuntesDetalleTema) 
		{ 
			try { 
				return Ok(dao.guardar(apuntesDetalleTema)); 
			} 
			catch (Exception ex) 
			{ 
				return BadRequest(ex.Message); 
			} 
		} 

		[HttpPut] 
		public IActionResult Put(ApuntesDetalleTema apuntesDetalleTema) 
		{ 
			try { 
				return Ok(dao.actualizar(apuntesDetalleTema)); 
			} 
			catch (Exception ex) 
			{ 
				return BadRequest(ex.Message); 
			} 
		} 

		[HttpDelete("{id}")] 
		public IActionResult Delete(int id) 
		{ 
			try { 
				dao.eliminar(id); 
				return Ok(); 
			} 
			catch (Exception ex) 
			{ 
				return BadRequest(ex.Message); 
			} 
		} 

		/*
		[HttpGet] 
		[Route("generarExcel")] 
		public IActionResult generarExcel([FromQuery] ApuntesDetalleTema apuntesDetalleTema) 
		{ 
			try { 
				IDictionary<string, object> mapa = llenarInformacionDataTable(apuntesDetalleTema); 
				List<ApuntesDetalleTema> listaApuntesDetalleTema = (List<ApuntesDetalleTema>) mapa["data"]; 

				ExcelPackage package = new ExcelPackage(); 
				ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Hoja 1"); 
				int indice = 1; 

				// Header del archivo 

				sheet.Cells[$"A{indice}"].Value = "apuntesTema"; 
				sheet.Cells[$"B{indice}"].Value = "rutaFoto"; 
				sheet.Cells[$"C{indice}"].Value = "id"; 
				sheet.Cells[$"D{indice}"].Value = "contenido"; 
				sheet.Cells[$"E{indice}"].Value = "titulo"; 
				indice ++; 

				foreach (ApuntesDetalleTema c in listaApuntesDetalleTema) 
				{ 
					sheet.Cells[$"A{indice}"].Value = c.apuntesTema.id; 
					sheet.Cells[$"B{indice}"].Value = c.rutaFoto; 
					sheet.Cells[$"C{indice}"].Value = c.id; 
					sheet.Cells[$"D{indice}"].Value = c.contenido; 
					sheet.Cells[$"E{indice}"].Value = c.titulo; 
					indice ++; 
				} 

				package.Save(); 
				MemoryStream stream = new MemoryStream(package.GetAsByteArray()); 

				return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ApuntesDetalleTema.xlsx"); 
			} 
			catch (Exception ex) 
			{ 
				return BadRequest(ex.Message); 
			} 
		} 

		[HttpPost] 
		[Route("importarExcel")] 
		public IActionResult importarExcel() 
		{ 
			if (! HttpContext.Request.HasFormContentType) 
				return BadRequest("No se recibió un archivo.Error: UnsupportedMediaType"); 

			try 
			{ 
				if (HttpContext.Request.Form.Files.Count > 0) 
				{ 
					IFormFile archivos = HttpContext.Request.Form.Files[0]; 

					if (archivos.ContentType == "application/vnd.ms-excel" || archivos.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") 
					{ 
						System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);   // Sin esto, lanza un error al usar el stream 

						Stream stream = archivos.OpenReadStream(); 
						List<ApuntesDetalleTema> listaApuntesDetalleTema = new List<ApuntesDetalleTema>(); 

						using (var reader = ExcelReaderFactory.CreateReader(stream)) 
						{ 
							DataSet result = reader.AsDataSet(); 

							//if (reader.AsDataSet().Tables.Count == 1) 
								//throw new ArgumentException("El archivo recibido solo posee una hoja"); 

							DataTable tablaPagina_1 = reader.AsDataSet().Tables[0];  // La posicion cero es el numero de la pagina u hoja 
							int contadorFila = 2;  // Esto es porque empieza a contar desde la fila 2 

							if (tablaPagina_1.Rows.Count == 0) 
								throw new ArgumentException("La hoja 1 del archivo excel no posee registros"); 

							for (int i = 1; i < tablaPagina_1.Rows.Count; i++)  // Comienza a leer desde la posicion 1, es decir, la fila 2 
							{ 
								DataRow fila = tablaPagina_1.Rows[i]; 

								string apuntesTemaId = fila[0].ToString(); 
								string rutaFoto = fila[1].ToString(); 
								string id = fila[2].ToString(); 
								string contenido = fila[3].ToString(); 
								string titulo = fila[4].ToString(); 

								if (string.IsNullOrEmpty(apuntesTemaId)) 
									throw new ArgumentException($"El atributo de tipo ApuntesTema en la fila {contadorFila} esta vacio"); 

								if (string.IsNullOrEmpty(rutaFoto)) 
									throw new ArgumentException($"El campo rutaFoto de la fila {contadorFila} esta vacio"); 

								if (string.IsNullOrEmpty(id)) 
									throw new ArgumentException($"El campo id de la fila {contadorFila} esta vacio"); 

								if (string.IsNullOrEmpty(contenido)) 
									throw new ArgumentException($"El campo contenido de la fila {contadorFila} esta vacio"); 

								if (string.IsNullOrEmpty(titulo)) 
									throw new ArgumentException($"El campo titulo de la fila {contadorFila} esta vacio"); 

								ApuntesTema apuntesTema = context.ApuntesTema.Where(c => c.id.ToString() == apuntesTemaId).FirstOrDefault(); 

								if (apuntesTema == null) { 
									throw new ArgumentException("El id de la entidad ApuntesTema en la fila " + contadorFila + " no se encuentra registrado"); 
								} 

								ApuntesDetalleTema apuntesDetalleTema = new ApuntesDetalleTema(); 

								apuntesDetalleTema.apuntesTema = apuntesTema; 
								apuntesDetalleTema.rutaFoto = rutaFoto; 
								apuntesDetalleTema.id = int.Parse(id); 
								apuntesDetalleTema.contenido = contenido; 
								apuntesDetalleTema.titulo = titulo; 
								listaApuntesDetalleTema.Add(apuntesDetalleTema); 
								contadorFila ++; 
							} 
						} 

						// Guarda o edita dependiendo de si existe 
						List<ApuntesDetalleTema> elementosInsertados = new List<ApuntesDetalleTema>(); 
						List<ApuntesDetalleTema> elementosActualizados = new List<ApuntesDetalleTema>(); 

						foreach(ApuntesDetalleTema apuntesDetalleTema in listaApuntesDetalleTema) 
						{ 
							if (apuntesDetalleTema.id != null && apuntesDetalleTema.id != 0)  // Si el elemento tiene id, entonces busca el registro con ese id  
							{ 
								ApuntesDetalleTema registro = dao.buscarPorId(apuntesDetalleTema.id); 
								registro.apuntesTema = apuntesDetalleTema.apuntesTema; 
								registro.rutaFoto = apuntesDetalleTema.rutaFoto; 
								registro.contenido = apuntesDetalleTema.contenido; 
								registro.titulo = apuntesDetalleTema.titulo; 
								 
								elementosActualizados.Add(registro); 
							} 
							else { 
								elementosInsertados.Add(apuntesDetalleTema); 
							} 
						} 

						dao.procesarDatosExcel(elementosInsertados, elementosActualizados); 
					} 
					else 
					{ 
						throw new ArgumentException("El archivo debe ser en formato xls"); 
					} 
				} 
				else 
				{ 
					throw new ArgumentException("No se recibió ningún archivo"); 
				} 
			} 
			catch (Exception ex) 
			{ 
				return BadRequest(); 
			} 
			return Ok(); 
		} 

		[HttpGet] 
		[Route("generarPDF")] 
		public IActionResult generarPDF([FromQuery] ApuntesDetalleTema apuntesDetalleTema) 
		{ 
			try { 
				IDictionary<string, object> mapa = llenarInformacionDataTable(apuntesDetalleTema); 
				List<ApuntesDetalleTema> listaApuntesDetalleTema = (List<ApuntesDetalleTema>) mapa["data"]; 

				string html = "<html><body><table><thead><tr>" + 
					"<th> apuntesTema </th>" + 
					"<th> rutaFoto </th>" + 
					"<th> id </th>" + 
					"<th> contenido </th>" + 
					"<th> titulo </th>" + 
				"<tr></thead><tbody>"; 
				listaApuntesDetalleTema.ForEach(c => 
				{ 
					html += "<tr>" + 
						"<td> " + c.apuntesTema.id + "</td>" + 
						"<td> " + c.rutaFoto + "</td>" + 
						"<td> " + c.id + "</td>" + 
						"<td> " + c.contenido + "</td>" + 
						"<td> " + c.titulo + "</td>" + 
					"</tr>"; 
				}); 
				html += "<tbody></table></body></html>"; 

				byte[] buffer = new byte[0]; 

				buffer = Pdf.From(html) 
					.WithGlobalSetting("orientation", "Landscape") 
					.WithObjectSetting("web.defaultEncoding", "utf-8") 
					.Content(); 

				MemoryStream stream = new MemoryStream(buffer); 
				return File(stream, "application/pdf", "ApuntesDetalleTema.pdf"); 
			} 
			catch (Exception ex) 
			{ 
				return BadRequest(ex.Message); 
			} 
		} 
		*/
	} 


 
}
