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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace AppApuntesNet5.Controllers 
{
	// En Authorize se coloca el schema de la autenticacion (existe por cookies y por token)

	[Route("ApuntesTema")] 
	[ApiController]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class ApuntesTemaController : ControllerBase 
	{ 

		private ApuntesTemaDao dao = null; 
		private readonly ApplicationDbContext context; 

		public ApuntesTemaController(ApplicationDbContext context) 
		{ 
			this.context = context;  // Esto es para poder usar el ApplicationDbContext 
			this.dao = new ApuntesTemaDao(this.context); 
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
		public IDictionary<string, object> llenarInformacionDataTable(ApuntesTema apuntesTema) 
		{ 
			int inicio = int.Parse(HttpContext.Request.Query["start"].ToString()); 
			int registrosPorPagina = int.Parse(HttpContext.Request.Query["length"].ToString());  // Es la cantidad de registros por pagina 


			IDictionary<string, object> respuesta = dao.llenarDataTableApuntesTema(apuntesTema, inicio, registrosPorPagina); 
			return respuesta; 
		} 

		[HttpPost] 
		[Route("llenarDataTable")] 
		public IActionResult llenarDataTable([FromQuery] ApuntesTema apuntesTema) 
		{ 
			try { 
				IDictionary<string, object> mapa = llenarInformacionDataTable(apuntesTema); 
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
				respuesta["listaApuntesCategoria"] = this.context.ApuntesCategoria.Select(c => new { c.id }); 
				return respuesta; 
			} 
			catch (Exception ex) 
			{ 
				return BadRequest(ex.Message); 
			} 
		} 

		[HttpGet] 
		[Route("llenarSelect2")] 
		public object llenarSelect2(String clase, String busqueda, int registrosPorPagina, int numeroPagina) 
		{ 
			try { 
				return dao.llenarSelect2(clase, busqueda, registrosPorPagina, numeroPagina); 
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
		public IActionResult Post(ApuntesTema apuntesTema) 
		{ 
			try { 
				return Ok(dao.guardar(apuntesTema)); 
			} 
			catch (Exception ex) 
			{ 
				return BadRequest(ex.Message); 
			} 
		} 

		[HttpPut] 
		public IActionResult Put(ApuntesTema apuntesTema) 
		{ 
			try { 
				return Ok(dao.actualizar(apuntesTema)); 
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


	} 


 
}
