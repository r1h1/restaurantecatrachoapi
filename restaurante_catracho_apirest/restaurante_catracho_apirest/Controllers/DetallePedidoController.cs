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
    public class DetallesPedidoController : ControllerBase
    {
        private readonly DetallePedidosData _data;

        public DetallesPedidoController(DetallePedidosData data)
        {
            _data = data;
        }

        // GET: api/DetallesPedido - Obtener todos los detalles de pedidos
        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<DetallePedidos> lista = await _data.Lista();
            return Ok(lista);
        }

        // GET: api/DetallesPedido/{id_detalle} - Obtener un detalle de pedido por ID
        [HttpGet("{id_detalle}")]
        public async Task<IActionResult> Obtener(int id_detalle)
        {
            var detalle = await _data.ObtenerId(id_detalle);
            if (detalle == null)
            {
                return NotFound(new { isSuccess = false, message = "Detalle de pedido no encontrado" });
            }
            return Ok(detalle);
        }

        // POST: api/DetallesPedido - Crear un detalle de pedido
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] DetallePedidos detalle)
        {
            if (detalle == null)
            {
                return BadRequest(new { isSuccess = false, message = "Datos inválidos" });
            }

            try
            {
                bool respuesta = await _data.Crear(detalle);
                return respuesta
                    ? StatusCode(StatusCodes.Status201Created, new { isSuccess = true, message = "Detalle de pedido creado exitosamente" })
                    : StatusCode(StatusCodes.Status500InternalServerError, new { isSuccess = false, message = "Error al crear el detalle de pedido" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { isSuccess = false, message = ex.Message });
            }
        }

        // PUT: api/DetallesPedido - Editar un detalle de pedido
        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] DetallePedidos detalle)
        {
            if (detalle == null || detalle.IdDetalle == 0)
            {
                return BadRequest(new { isSuccess = false, message = "Datos inválidos" });
            }

            try
            {
                bool respuesta = await _data.Editar(detalle);
                return respuesta
                    ? Ok(new { isSuccess = true, message = "Detalle de pedido actualizado correctamente" })
                    : NotFound(new { isSuccess = false, message = "Detalle de pedido no encontrado" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { isSuccess = false, message = ex.Message });
            }
        }

        // DELETE: api/DetallesPedido/{id_detalle} - Eliminar un detalle de pedido
        [HttpDelete("{id_detalle}")]
        public async Task<IActionResult> Eliminar(int id_detalle)
        {
            try
            {
                bool respuesta = await _data.Eliminar(id_detalle);
                return respuesta
                    ? Ok(new { isSuccess = true, message = "Detalle de pedido eliminado correctamente" })
                    : NotFound(new { isSuccess = false, message = "Detalle de pedido no encontrado" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { isSuccess = false, message = ex.Message });
            }
        }
    }
}
