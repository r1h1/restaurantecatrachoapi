using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using restaurante_catracho_apirest.Data;
using restaurante_catracho_apirest.Models;

namespace restaurante_catracho_apirest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioData _data;

        public UsuariosController(UsuarioData data)
        {
            _data = data;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
           List<Usuarios> Lista = await _data.Lista();
            return StatusCode(StatusCodes.Status200OK,Lista);
        }

        [HttpGet("/{id_usuario}")]
        public async Task<IActionResult> Obtener(int id_usuario)
        {
            Usuarios objeto = await _data.ObtenerId(id_usuario);
            return StatusCode(StatusCodes.Status200OK, objeto);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Usuarios objeto)
        {
            bool respuesta = await _data.Crear(objeto);
            return StatusCode(StatusCodes.Status200OK, new {isSuccess = respuesta});
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] Usuarios objeto)
        {
            bool respuesta = await _data.Editar(objeto);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }

        [HttpDelete("/{id_usuario}")]
        public async Task<IActionResult> Elimiar(int id_usuario)
        {
            bool respuesta = await _data.Eliminar(id_usuario);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }
    }
}
