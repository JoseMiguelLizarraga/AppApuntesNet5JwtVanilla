using System; 
using System.Collections.Generic; 
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc; 
using AppApuntesNet5.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using AppApuntesNet5.Services;
using AppApuntesNet5.Dto;

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
		[Route("llenarSelect2")] 
		public IActionResult LlenarSelect2(string busqueda, int registrosPorPagina, int numeroPagina) 
		{ 
			return Ok(_categoriaService.LlenarSelect2(busqueda, registrosPorPagina, numeroPagina)); 
		} 
	} 
}
