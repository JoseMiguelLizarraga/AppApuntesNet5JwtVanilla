using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Linq;
using DataAccess.Models;
using Mappings;
using Services;

namespace AppApuntesNet5.Controllers
{
    [ApiController]
    [Route("ApuntesCategoria")]
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
        public IActionResult llenarDataTable([FromQuery] ApuntesCategoriaDTO dto, int start, int length)
        {
            //ApuntesCategorium apuntesCategoria = dto.ToDatabaseObject();
            DataTableDTO respuesta = _servicio.LlenarDataTableApuntesCategoria(dto.ToDatabaseObject(), start, length);
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
            ApuntesCategorium apuntesCategoria = _servicio.BuscarPorId(id);
            return apuntesCategoria.ToDTO();
        }

        [HttpPost]
        public async Task<ActionResult<ApuntesCategoriaDTO>> Post(ApuntesCategoriaDTO dto)
        {
            (ApuntesCategorium, string) result = await _servicio.Guardar(dto.ToDatabaseObject());

            if (!string.IsNullOrEmpty(result.Item2)) return BadRequest(result.Item2);
            return Ok(result.Item1.ToDTO());
        }

        [HttpPut]
        public async Task<ActionResult<ApuntesCategoriaDTO>> Put(ApuntesCategoriaDTO dto)
        {
            (ApuntesCategorium, string) result = await _servicio.Actualizar(dto.ToDatabaseObject());

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
