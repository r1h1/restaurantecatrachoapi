using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using restaurante_catracho_apirest.Data;
using restaurante_catracho_apirest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restaurante_catracho_apirest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagosController : ControllerBase
    {
        private readonly PagosData _data;

        public PagosController(PagosData data)
        {
            _data = data;
        }

        // GET: api/Pagos - Obtener todos los pagos
        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<Pagos> lista = await _data.Lista();
            return Ok(lista);
        }

        // GET: api/Pagos/{id_pago} - Obtener un pago por ID
        [HttpGet("{id_pago}")]
        public async Task<IActionResult> Obtener(int id_pago)
        {
            var pago = await _data.ObtenerId(id_pago);
            if (pago == null)
            {
                return NotFound(new { isSuccess = false, message = "Pago no encontrado" });
            }
            return Ok(pago);
        }

        // POST: api/Pagos - Crear un pago
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Pagos pago)
        {
            if (pago == null)
            {
                return BadRequest(new { isSuccess = false, message = "Datos inválidos" });
            }

            try
            {
                bool respuesta = await _data.Crear(pago);
                return respuesta
                    ? StatusCode(StatusCodes.Status201Created, new { isSuccess = true, message = "Pago creado exitosamente" })
                    : StatusCode(StatusCodes.Status500InternalServerError, new { isSuccess = false, message = "Error al crear el pago" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { isSuccess = false, message = ex.Message });
            }
        }

        // PUT: api/Pagos - Editar un pago
        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] Pagos pago)
        {
            if (pago == null || pago.IdPago == 0)
            {
                return BadRequest(new { isSuccess = false, message = "Datos inválidos" });
            }

            try
            {
                bool respuesta = await _data.Editar(pago);
                return respuesta
                    ? Ok(new { isSuccess = true, message = "Pago actualizado correctamente" })
                    : NotFound(new { isSuccess = false, message = "Pago no encontrado" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { isSuccess = false, message = ex.Message });
            }
        }

        // DELETE: api/Pagos/{id_pago} - Eliminar un pago
        [HttpDelete("{id_pago}")]
        public async Task<IActionResult> Eliminar(int id_pago)
        {
            try
            {
                bool respuesta = await _data.Eliminar(id_pago);
                return respuesta
                    ? Ok(new { isSuccess = true, message = "Pago eliminado correctamente" })
                    : NotFound(new { isSuccess = false, message = "Pago no encontrado" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { isSuccess = false, message = ex.Message });
            }
        }
    }
}
