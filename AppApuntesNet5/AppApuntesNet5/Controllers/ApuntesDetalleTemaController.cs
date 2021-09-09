using System; 
using System.Collections.Generic; 
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc; 
using System.Net; 
using System.Linq; 
using System.Net.Http; 
using System.Net.Http.Headers; 
using AppApuntesNet5.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AppApuntesNet5.Services;


namespace AppApuntesNet5.Controllers
{
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[Route("ApuntesDetalleTema")] 
	[ApiController] 
	public class ApuntesDetalleTemaController : ControllerBase 
	{
		private readonly IApuntesDetalleTemaService _apuntesDetalleTemaService;

		public ApuntesDetalleTemaController(IApuntesDetalleTemaService apuntesDetalleTemaService)
		{
			_apuntesDetalleTemaService = apuntesDetalleTemaService;
		}

		[HttpGet] 
		public IActionResult Get() 
		{ 
			try { 
				return Ok(_apuntesDetalleTemaService.listar()); 
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


			IDictionary<string, object> respuesta = _apuntesDetalleTemaService.llenarDataTableApuntesDetalleTema(apuntesDetalleTema, inicio, registrosPorPagina); 
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


		[HttpGet] 
		[Route("llenarSelect2")] 
		public object llenarSelect2(String clase, String busqueda, int registrosPorPagina, int numeroPagina, int idApuntesCategoria) 
		{ 
			try { 
				return _apuntesDetalleTemaService.llenarSelect2(clase, busqueda, registrosPorPagina, numeroPagina, idApuntesCategoria); 
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
				return Ok(_apuntesDetalleTemaService.buscarPorId(id)); 
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
				return Ok(_apuntesDetalleTemaService.guardar(apuntesDetalleTema)); 
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
				return Ok(_apuntesDetalleTemaService.actualizar(apuntesDetalleTema)); 
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
				_apuntesDetalleTemaService.eliminar(id); 
				return Ok(); 
			} 
			catch (Exception ex) 
			{ 
				return BadRequest(ex.Message); 
			} 
		}
	
	} 
 
}
