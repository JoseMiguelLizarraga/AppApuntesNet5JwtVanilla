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
using Util;

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
            RespuestaService<DataTableDTO> respuesta = _servicio.LlenarDataTableApuntesCategoria(dto.ToDatabaseObject(), start, length);
            return Ok(respuesta.Objeto);
        }

        [HttpGet]
        [Route("llenarSelect2")]
        public ActionResult<Select2DTO> LlenarSelect2(string busqueda, int registrosPorPagina, int numeroPagina)
        {
            return _servicio.LlenarSelect2(busqueda, registrosPorPagina, numeroPagina).Objeto;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ActionResult<ApuntesCategoriaDTO> BuscarPorId(int id)
        {
            var respuesta = _servicio.BuscarPorId(id);

            if (respuesta.Objeto != null)
                return Ok(respuesta.Objeto.ToDTO());
            else
                return BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult<ApuntesCategoriaDTO>> Post(ApuntesCategoriaDTO dto)
        {
            RespuestaService<ApuntesCategorium> result = await _servicio.Guardar(dto.ToDatabaseObject());

            if (result.Objeto != null)
                return Ok(result.Objeto.ToDTO());

            else if (result.ExcepcionCapturada.Status == 400)
                return BadRequest(result.ExcepcionCapturada.MensajeError);

            else
                return StatusCode(500, $"Se encontr� un error: {result.ExcepcionCapturada.MensajeError}");
        }

        [HttpPut]
        public async Task<ActionResult<ApuntesCategoriaDTO>> Put(ApuntesCategoriaDTO dto)
        {
            RespuestaService<ApuntesCategorium> result = await _servicio.Actualizar(dto.ToDatabaseObject());

            if (result.Objeto != null)
                return Ok(result.Objeto.ToDTO());

            else if (result.ExcepcionCapturada.Status == 400)
                return BadRequest(result.ExcepcionCapturada.MensajeError);

            else
                return StatusCode(500, $"Se encontr� un error: {result.ExcepcionCapturada.MensajeError}");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            (bool, ExcepcionCapturada) result = await _servicio.Eliminar(id);

            if (result.Item1)
                return Ok();
            else
                return StatusCode(500, $"Se encontr� un error: {result.Item2.MensajeError}");
        }

        [HttpPost("agregarLogo")]
        public async Task<ActionResult<ApuntesCategoriaDTO>> AgregarLogo(ApuntesCategoriaDTO dto)
        {
            RespuestaService<ApuntesCategorium> result = await _servicio.GuardarLogo(dto.ToDatabaseObject());

            if (result.Objeto != null)
                return Ok(result.Objeto.ToDTO());

            else if (result.ExcepcionCapturada.Status == 400)
                return BadRequest(result.ExcepcionCapturada.MensajeError);

            else
                return StatusCode(500, $"Se encontr� un error: {result.ExcepcionCapturada.MensajeError}");
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
