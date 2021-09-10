using System; 
using Microsoft.AspNetCore.Mvc; 
using AppApuntesNet5.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using AppApuntesNet5.Services;

namespace AppApuntesNet5.Controllers 
{
	[Route("ApuntesTema")]   // En Authorize se coloca el schema de la autenticacion (existe por cookies y por token)
	[ApiController]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class ApuntesTemaController : ControllerBase 
	{ 

		private readonly ITemaService _temaService;

		public ApuntesTemaController(ITemaService temaService) 
		{
			_temaService = temaService;
		} 

		[HttpGet] 
		public IActionResult Get() 
		{ 
			return Ok(_temaService.Listar()); 
		} 

		[HttpPost] 
		[Route("llenarDataTable")] 
		public IActionResult LlenarDataTable([FromQuery] ApuntesTema apuntesTema, int start, int length) 
		{ 
			return Ok(_temaService.LlenarDataTableApuntesTema(apuntesTema, start, length));
		}

		[HttpGet]
		[Route("llenarSelect2")]
		public IActionResult LlenarSelect2(string busqueda, int registrosPorPagina, int numeroPagina, int idApuntesCategoria)
		{
			return Ok(_temaService.LlenarSelect2(busqueda, registrosPorPagina, numeroPagina, idApuntesCategoria));
		}

		[HttpGet] 
		[Route("{id:int}")] 
		public IActionResult BuscarPorId(int id) 
		{ 
			return Ok(_temaService.BuscarPorId(id)); 
		} 

		[HttpPost] 
		public IActionResult Post(ApuntesTema apuntesTema) 
		{
            (ApuntesTema, string) result = _temaService.Guardar(apuntesTema);

			if (! string.IsNullOrEmpty(result.Item2)) return BadRequest(result.Item2);
			return Ok(result.Item1);
		} 

		[HttpPut] 
		public IActionResult Put(ApuntesTema apuntesTema) 
		{
			(ApuntesTema, string) result = _temaService.Actualizar(apuntesTema);

			if (! string.IsNullOrEmpty(result.Item2)) return BadRequest(result.Item2);
			return Ok(result.Item1);
		} 

		[HttpDelete("{id}")] 
		public IActionResult Delete(int id) 
		{
			if (! _temaService.Eliminar(id, out string error))
				return BadRequest(error);

			return Ok();
		} 


	} 


 
}
