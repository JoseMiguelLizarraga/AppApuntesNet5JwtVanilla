using System;
using Microsoft.AspNetCore.Mvc;
using AppApuntesNet5.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using AppApuntesNet5.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using AppApuntesNet5.Dto;
using System.Linq;
using AppApuntesNet5.Mappings;

namespace AppApuntesNet5.Controllers
{
    [Route("ApuntesTema")]
    [ApiController]
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
        public async Task<ActionResult<DataTableDTO>> LlenarDataTable([FromQuery] ApuntesTema apuntesTema, int start, int length)
        {
            return await _servicio.LlenarDataTableApuntesTema(apuntesTema, start, length);
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
        public async Task<ActionResult<ApuntesTemaDTO>> Post(ApuntesTema apuntesTema)
        {
            (ApuntesTema, string) result = await _servicio.Guardar(apuntesTema);

            if (!string.IsNullOrEmpty(result.Item2)) return BadRequest(result.Item2);
            return Ok(result.Item1.ToDTO());
        }

        [HttpPut]
        public async Task<ActionResult<ApuntesTemaDTO>> Put(ApuntesTema apuntesTema)
        {
            (ApuntesTema, string) result = await _servicio.Actualizar(apuntesTema);

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
