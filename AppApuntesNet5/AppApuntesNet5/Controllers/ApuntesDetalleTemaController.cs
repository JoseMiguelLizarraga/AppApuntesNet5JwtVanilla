using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using System.Linq;
using DataAccess.Models;
using Mappings;
using Services;
using Util;

namespace AppApuntesNet5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        public async Task<ActionResult<DataTableDTO>> LlenarDataTable([FromQuery] ApuntesDetalleTemaDTO dto, int start, int length)
        {
            return await _servicio.LlenarDataTableApuntesDetalleTema(dto.ToDatabaseObject(), start, length);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<ApuntesDetalleTemaDTO>> BuscarPorId(int id)
        {
            ApuntesDetalleTema apuntesDetalleTema = await _servicio.BuscarPorId(id);
            return apuntesDetalleTema.ToDTO();
        }

        [HttpPost]
        public async Task<ActionResult<ApuntesDetalleTemaDTO>> Post(ApuntesDetalleTemaDTO dto)
        {
            (ApuntesDetalleTema, ExcepcionCapturada) result = await _servicio.Guardar(dto.ToDatabaseObject());

            if (result.Item1 != null)
                return Ok(result.Item1.ToDTO());

            else if (result.Item2.Status == 400)
                return BadRequest(result.Item2.MensajeError);

            else
                return StatusCode(500, $"Se encontró un error: {result.Item2.MensajeError}");
        }

        [HttpPut]
        public async Task<ActionResult<ApuntesDetalleTemaDTO>> Put(ApuntesDetalleTemaDTO dto)
        {
            (ApuntesDetalleTema, ExcepcionCapturada) result = await _servicio.Actualizar(dto.ToDatabaseObject());

            if (result.Item1 != null)
                return Ok(result.Item1.ToDTO());

            else if (result.Item2.Status == 400)
                return BadRequest(result.Item2.MensajeError);

            else
                return StatusCode(500, $"Se encontró un error: {result.Item2.MensajeError}");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            (bool, string) result = await _servicio.Eliminar(id);

            if (result.Item1)
                return Ok();
            else
                return StatusCode(500, $"Se encontró un error: {result.Item2}");
        }
    }
}
