using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppApuntesNet5.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AppApuntesNet5.Services;
using System.Threading.Tasks;
using AppApuntesNet5.Dto;

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
        public async Task<ActionResult<List<ApuntesDetalleTema>>> Get()
        {
            return await _detalleTemaService.Listar();
        }

        [HttpPost]
        [Route("llenarDataTable")]
        public async Task<ActionResult<DataTableDTO>> LlenarDataTable([FromQuery] ApuntesDetalleTema apuntesDetalleTema, int start, int length)
        {
            return await _detalleTemaService.LlenarDataTableApuntesDetalleTema(apuntesDetalleTema, start, length);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<ApuntesDetalleTema>> BuscarPorId(int id)
        {
            return await _detalleTemaService.BuscarPorId(id);
        }

        [HttpPost]
        public async Task<ActionResult<ApuntesDetalleTema>> Post(ApuntesDetalleTema apuntesDetalleTema)
        {
            (ApuntesDetalleTema, string) result = await _detalleTemaService.Guardar(apuntesDetalleTema);

            if (!string.IsNullOrEmpty(result.Item2)) return BadRequest(result.Item2);
            return Ok(result.Item1);
        }

        [HttpPut]
        public async Task<ActionResult<ApuntesDetalleTema>> Put(ApuntesDetalleTema apuntesDetalleTema)
        {
            (ApuntesDetalleTema, string) result = await _detalleTemaService.Actualizar(apuntesDetalleTema);

            if (!string.IsNullOrEmpty(result.Item2)) return BadRequest(result.Item2);
            return Ok(result.Item1);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            (bool, string) result = await _detalleTemaService.Eliminar(id);

            if (!result.Item1) return BadRequest(result.Item2);
            return Ok();
        }
    }
}
