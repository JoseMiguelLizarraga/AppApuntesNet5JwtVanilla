using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Models;
using Mappings;
using Services;

namespace AppApuntesNet5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ApuntesTemaController : ControllerBase
    {

        private readonly ITemaService _servicio;

        public ApuntesTemaController(ITemaService servicio)
        {
            _servicio = servicio;
        }


        [HttpGet]
        public async Task<ActionResult<List<ApuntesTemaDTO>>> Get()
        {
            List<ApuntesTema> lista = await _servicio.Listar();
            return lista.Select(Mapper.ToDTO).ToList();
        }

        [HttpPost]
        [Route("llenarDataTable")]
        public async Task<ActionResult<DataTableDTO>> LlenarDataTable([FromQuery] ApuntesTemaDTO dto, int start, int length)
        {
            return await _servicio.LlenarDataTableApuntesTema(dto.ToDatabaseObject(), start, length);
        }

        [HttpGet]
        [Route("llenarSelect2")]
        public async Task<ActionResult<Select2DTO>> LlenarSelect2(string busqueda, int registrosPorPagina, int numeroPagina, int idApuntesCategoria)
        {
            return await _servicio.LlenarSelect2(busqueda, registrosPorPagina, numeroPagina, idApuntesCategoria);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<ApuntesTemaDTO>> BuscarPorId(int id)
        {
            ApuntesTema apuntesTema = await _servicio.BuscarPorId(id);
            return apuntesTema.ToDTO();
        }

        [HttpPost]
        public async Task<ActionResult<ApuntesTemaDTO>> Post(ApuntesTemaDTO dto)
        {
            (ApuntesTema, string) result = await _servicio.Guardar(dto.ToDatabaseObject());

            if (!string.IsNullOrEmpty(result.Item2)) return BadRequest(result.Item2);
            return Ok(result.Item1.ToDTO());
        }

        [HttpPut]
        public async Task<ActionResult<ApuntesTemaDTO>> Put(ApuntesTemaDTO dto)
        {
            (ApuntesTema, string) result = await _servicio.Actualizar(dto.ToDatabaseObject());

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
