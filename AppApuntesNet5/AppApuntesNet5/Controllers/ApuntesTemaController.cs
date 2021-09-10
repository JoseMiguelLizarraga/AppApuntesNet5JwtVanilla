using System; 
using Microsoft.AspNetCore.Mvc; 
using AppApuntesNet5.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using AppApuntesNet5.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using AppApuntesNet5.Dto;

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
		public async Task<ActionResult<List<ApuntesTema>>> Get()
		{
			return await _temaService.Listar();
		}

		[HttpPost] 
		[Route("llenarDataTable")] 
		public async Task<ActionResult<DataTableDTO>> LlenarDataTable([FromQuery] ApuntesTema apuntesTema, int start, int length) 
		{
			return await _temaService.LlenarDataTableApuntesTema(apuntesTema, start, length);
		}

		[HttpGet]
		[Route("llenarSelect2")]
		public async Task<ActionResult<Select2DTO>> LlenarSelect2(string busqueda, int registrosPorPagina, int numeroPagina, int idApuntesCategoria)
		{
			return await _temaService.LlenarSelect2(busqueda, registrosPorPagina, numeroPagina, idApuntesCategoria);
		}

		[HttpGet] 
		[Route("{id:int}")] 
		public async Task<ActionResult<ApuntesTema>> BuscarPorId(int id) 
		{ 
			return await _temaService.BuscarPorId(id); 
		} 

		[HttpPost] 
		public async Task<ActionResult<ApuntesTema>> Post(ApuntesTema apuntesTema) 
		{
            (ApuntesTema, string) result = await _temaService.Guardar(apuntesTema);

			if (! string.IsNullOrEmpty(result.Item2)) return BadRequest(result.Item2);
			return Ok(result.Item1);
		} 

		[HttpPut] 
		public async Task<ActionResult<ApuntesTema>> Put(ApuntesTema apuntesTema) 
		{
			(ApuntesTema, string) result = await _temaService.Actualizar(apuntesTema);

			if (! string.IsNullOrEmpty(result.Item2)) return BadRequest(result.Item2);
			return Ok(result.Item1);
		} 

		[HttpDelete("{id}")] 
		public async Task<ActionResult> Delete(int id) 
		{
			(bool, string) result = await _temaService.Eliminar(id);

			if (! result.Item1) return BadRequest(result.Item2);
			return Ok();
		} 


	} 


 
}
