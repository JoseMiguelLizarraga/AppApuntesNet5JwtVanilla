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
using AppApuntesNet5.Mappings;

namespace AppApuntesNet5.Controllers
{
    [Route("ApuntesCategoria")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ApuntesCategoriaController : ControllerBase
    {

        private readonly ICategoriaService _servicio;

        public ApuntesCategoriaController(ICategoriaService servicio)
        {
            _servicio = servicio;
        }


        [HttpGet]
        [Route("llenarDataTable")]
        public IActionResult llenarDataTable([FromQuery] ApuntesCategoria apuntesCategoria, int start, int length)
        {
            DataTableDTO respuesta = _servicio.LlenarDataTableApuntesCategoria(apuntesCategoria, start, length);
            return Ok(respuesta);
        }

        [HttpGet]
        [Route("llenarSelect2")]
        public ActionResult<Select2DTO> LlenarSelect2(string busqueda, int registrosPorPagina, int numeroPagina)
        {
            Select2DTO retorno = _servicio.LlenarSelect2(busqueda, registrosPorPagina, numeroPagina);
            return retorno;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ActionResult<ApuntesCategoriaDTO> BuscarPorId(int id)
        {
            ApuntesCategoria apuntesCategoria = _servicio.BuscarPorId(id);
            return apuntesCategoria.ToDTO();
        }

        [HttpPost]
        public async Task<ActionResult<ApuntesCategoriaDTO>> Post(ApuntesCategoria apuntesCategoria)
        {
            (ApuntesCategoria, string) result = await _servicio.Guardar(apuntesCategoria);

            if (!string.IsNullOrEmpty(result.Item2)) return BadRequest(result.Item2);
            return Ok(result.Item1.ToDTO());
        }

        [HttpPut]
        public async Task<ActionResult<ApuntesCategoriaDTO>> Put(ApuntesCategoria apuntesCategoria)
        {
            (ApuntesCategoria, string) result = await _servicio.Actualizar(apuntesCategoria);

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
