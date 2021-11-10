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
using Newtonsoft.Json;

namespace AppApuntesNet5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpPost("agregarLogo")]
        public async Task<ActionResult<ApuntesCategoriaDTO>> AgregarLogo(ApuntesCategoriaDTO dto)
        {
            (ApuntesCategorium, string) result = await _servicio.GuardarLogo(dto.ToDatabaseObject());

            if (string.IsNullOrEmpty(result.Item2))
                return Ok(result.Item1.ToDTO());
            else
                return StatusCode(500, result.Item2);
        }
    }
}

/*
// Ejemplo de como guardar un logo

fetch("https://localhost:44322/api/apuntesCategoria/agregarLogo", {
	method: "POST",
	headers: {"Accept": "application/json", "Content-Type": "application/json"},
	body: JSON.stringify({
        "id": 4, 
        "logo": "iVBORw0KGgoAAAANSUhEUgAAABMAAAAQCAIAAAB7ptM1AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAB1SURBVDhPzY3LDYAwDEM7CEf234wZiqmjkF+L4MRTL3b81Na/UpvNI62naDE9tt2+Uo6V1a7vBuw5UFzGOWuEV87IHXCYaYpMBy9M9rL+k5llLWW9MLkjGmU68CHJ9q1MMJODBmIGWc4aKCqAqUVaT90+0/sJIs82uSJwNb0AAAAASUVORK5CYII=",
        "tipoLogo": "data:image/png;base64"
    })
})
.then(response => 
{
	if(response.ok) {
		response.json().then(data => console.log(data));
	} 
	else {
		response.text().then(textoError => alert(textoError));
	}
}); 
*/
