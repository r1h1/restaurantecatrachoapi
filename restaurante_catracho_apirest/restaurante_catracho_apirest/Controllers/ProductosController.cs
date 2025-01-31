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
    public class ProductosController : ControllerBase
    {
        private readonly ProductoData _data;

        public ProductosController(ProductoData data)
        {
            _data = data;
        }

        // GET: api/Productos - Obtener todos los productos
        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<Productos> lista = await _data.Lista();
            return Ok(lista);
        }

        // GET: api/Productos/{id_producto} - Obtener un producto por ID
        [HttpGet("{id_producto}")]
        public async Task<IActionResult> Obtener(int id_producto)
        {
            var producto = await _data.ObtenerId(id_producto);
            if (producto == null)
            {
                return NotFound(new { isSuccess = false, message = "Producto no encontrado" });
            }
            return Ok(producto);
        }

        // POST: api/Productos - Crear un producto
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Productos producto)
        {
            if (producto == null)
            {
                return BadRequest(new { isSuccess = false, message = "Datos inválidos" });
            }

            try
            {
                bool respuesta = await _data.Crear(producto);
                return respuesta
                    ? StatusCode(StatusCodes.Status201Created, new { isSuccess = true, message = "Producto creado exitosamente" })
                    : StatusCode(StatusCodes.Status500InternalServerError, new { isSuccess = false, message = "Error al crear el producto" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { isSuccess = false, message = ex.Message });
            }
        }

        // PUT: api/Productos - Editar un producto
        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] Productos producto)
        {
            if (producto == null || producto.IdProducto == 0)
            {
                return BadRequest(new { isSuccess = false, message = "Datos inválidos" });
            }

            try
            {
                bool respuesta = await _data.Editar(producto);
                return respuesta
                    ? Ok(new { isSuccess = true, message = "Producto actualizado correctamente" })
                    : NotFound(new { isSuccess = false, message = "Producto no encontrado" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { isSuccess = false, message = ex.Message });
            }
        }

        // DELETE: api/Productos/{id_producto} - Eliminar un producto
        [HttpDelete("{id_producto}")]
        public async Task<IActionResult> Eliminar(int id_producto)
        {
            try
            {
                bool respuesta = await _data.Eliminar(id_producto);
                return respuesta
                    ? Ok(new { isSuccess = true, message = "Producto eliminado correctamente" })
                    : NotFound(new { isSuccess = false, message = "Producto no encontrado" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { isSuccess = false, message = ex.Message });
            }
        }
    }
}
