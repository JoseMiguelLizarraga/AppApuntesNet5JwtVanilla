using System; 
using System.Collections.Generic; 
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc; 
using AppApuntesNet5.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using AppApuntesNet5.Services;
using AppApuntesNet5.Dto;
using System.Threading.Tasks;
using System.Linq;

namespace AppApuntesNet5.Controllers 
{
	[Route("ApuntesCategoria")] 
	[ApiController]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class ApuntesCategoriaController : ControllerBase 
	{ 

		private readonly ICategoriaService _categoriaService;

		public ApuntesCategoriaController(ICategoriaService categoriaService) 
		{
			_categoriaService = categoriaService;
		}


		[HttpGet]
		[Route("llenarDataTable")]
		public IActionResult llenarDataTable([FromQuery] ApuntesCategoria apuntesCategoria, int start, int length)
		{
			DataTableDTO respuesta = _categoriaService.LlenarDataTableApuntesCategoria(apuntesCategoria, start, length);
			return Ok(respuesta);
		}

		[HttpGet]
		[Route("llenarSelect2")]
		public ActionResult<Select2DTO> LlenarSelect2(string busqueda, int registrosPorPagina, int numeroPagina)
		{
			Select2DTO retorno = _categoriaService.LlenarSelect2(busqueda, registrosPorPagina, numeroPagina);
			return retorno;
		}

		[HttpPost]
		public async Task<ActionResult<ApuntesCategoria>> Post(ApuntesCategoria apuntesCategoria)
		{
			(ApuntesCategoria, string) result = await _categoriaService.Guardar(apuntesCategoria);

			if (!string.IsNullOrEmpty(result.Item2)) return BadRequest(result.Item2);
			return Ok(result.Item1);
		}
	} 
}
