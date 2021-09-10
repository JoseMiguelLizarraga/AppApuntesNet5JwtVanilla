using System; 
using System.Collections.Generic; 
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc; 
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
		private readonly IDetalleTemaService _detalleTemaService;

		public ApuntesDetalleTemaController(IDetalleTemaService detalleTemaService)
		{
			_detalleTemaService = detalleTemaService;
		}

		[HttpGet] 
		public IActionResult Get() 
		{ 
			return Ok(_detalleTemaService.Listar()); 
		} 

		[HttpPost] 
		[Route("llenarDataTable")] 
		public IActionResult LlenarDataTable([FromQuery] ApuntesDetalleTema apuntesDetalleTema, int start, int length) 
		{
			return Ok(_detalleTemaService.LlenarDataTableApuntesDetalleTema(apuntesDetalleTema, start, length));
		} 

		[HttpGet] 
		[Route("{id:int}")] 
		public IActionResult BuscarPorId(int id) 
		{
			return Ok(_detalleTemaService.BuscarPorId(id));
		} 

		[HttpPost] 
		public IActionResult Post(ApuntesDetalleTema apuntesDetalleTema) 
		{
			(ApuntesDetalleTema, string) result = _detalleTemaService.Guardar(apuntesDetalleTema);

			if (!string.IsNullOrEmpty(result.Item2)) return BadRequest(result.Item2);
			return Ok(result.Item1);
		} 

		[HttpPut] 
		public IActionResult Put(ApuntesDetalleTema apuntesDetalleTema) 
		{
			(ApuntesDetalleTema, string) result = _detalleTemaService.Actualizar(apuntesDetalleTema);

			if (!string.IsNullOrEmpty(result.Item2)) return BadRequest(result.Item2);
			return Ok(result.Item1);
		} 

		[HttpDelete("{id}")] 
		public IActionResult Delete(int id) 
		{
			if (! _detalleTemaService.Eliminar(id, out string error))
				return BadRequest(error);

			return Ok();
		}
	} 
}
