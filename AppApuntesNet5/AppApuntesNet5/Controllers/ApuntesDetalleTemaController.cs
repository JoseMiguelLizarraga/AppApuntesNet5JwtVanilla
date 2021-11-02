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
using System.Linq;
using AppApuntesNet5.Mappings;

namespace AppApuntesNet5.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("ApuntesDetalleTema")]
    [ApiController]
    public class ApuntesDetalleTemaController : ControllerBase
    {
        private readonly IDetalleTemaService _servicio;

        public ApuntesDetalleTemaController(IDetalleTemaService servicio)
        {
            _servicio = servicio;
        }

        [HttpGet]
        public async Task<ActionResult<List<ApuntesDetalleTemaDTO>>> Get()
        {
            List<ApuntesDetalleTema> lista = await _servicio.Listar();
            return lista.Select(Mapper.ToDTO).ToList();
        }

        [HttpPost]
        [Route("llenarDataTable")]
        public async Task<ActionResult<DataTableDTO>> LlenarDataTable([FromQuery] ApuntesDetalleTema apuntesDetalleTema, int start, int length)
        {
            return await _servicio.LlenarDataTableApuntesDetalleTema(apuntesDetalleTema, start, length);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<ApuntesDetalleTemaDTO>> BuscarPorId(int id)
        {
            ApuntesDetalleTema apuntesDetalleTema = await _servicio.BuscarPorId(id);
            return apuntesDetalleTema.ToDTO();
        }

        [HttpPost]
        public async Task<ActionResult<ApuntesDetalleTemaDTO>> Post(ApuntesDetalleTema apuntesDetalleTema)
        {
            (ApuntesDetalleTema, string) result = await _servicio.Guardar(apuntesDetalleTema);

            if (!string.IsNullOrEmpty(result.Item2)) return BadRequest(result.Item2);
            return Ok(result.Item1.ToDTO());
        }

        [HttpPut]
        public async Task<ActionResult<ApuntesDetalleTemaDTO>> Put(ApuntesDetalleTema apuntesDetalleTema)
        {
            (ApuntesDetalleTema, string) result = await _servicio.Actualizar(apuntesDetalleTema);

            if (!string.IsNullOrEmpty(result.Item2)) return BadRequest(result.Item2);
            return Ok(result.Item1.ToDTO());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            (bool, string) result = await _servicio.Eliminar(id);

            if (!result.Item1) return BadRequest(result.Item2);
            return Ok();
        }
    }
}
