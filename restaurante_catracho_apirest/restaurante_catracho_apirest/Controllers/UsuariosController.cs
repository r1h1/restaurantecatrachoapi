﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using restaurante_catracho_apirest.Data;
using restaurante_catracho_apirest.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

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

        // GET: api/Usuarios - Obtener todos los usuarios
        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            var lista = await _data.Lista();
            return Ok(lista);
        }

        // GET: api/Usuarios/{id_usuario} - Obtener un usuario por ID
        [HttpGet("{id_usuario}")]
        public async Task<IActionResult> Obtener(int id_usuario)
        {
            var usuario = await _data.ObtenerId(id_usuario);
            if (usuario == null)
            {
                return NotFound(new { isSuccess = false, message = "Usuario no encontrado" });
            }
            return Ok(usuario);
        }

        // POST: api/Usuarios - Crear un usuario
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Usuarios usuario)
        {
            if (usuario == null)
            {
                return BadRequest(new { isSuccess = false, message = "Datos inválidos" });
            }

            try
            {
                bool respuesta = await _data.Crear(usuario);
                return respuesta
                    ? StatusCode(StatusCodes.Status201Created, new { isSuccess = true, message = "Usuario creado exitosamente" })
                    : StatusCode(StatusCodes.Status409Conflict, new { isSuccess = false, message = "No se pudo crear el usuario" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { isSuccess = false, message = ex.Message });
            }
        }

        // POST: api/Usuarios - Modificar clave de usuario
        [HttpPost("ActualizarClave")]
        public async Task<IActionResult> ActualizarClave([FromBody] ActualizarClaveRequest request)
        {
            if (request == null || request.IdUsuario <= 0 || string.IsNullOrEmpty(request.NuevaClave))
            {
                return BadRequest(new { mensaje = "Datos inválidos." });
            }

            bool resultado = await _data.ActualizarClave(request.IdUsuario, request.NuevaClave);

            if (resultado)
            {
                return Ok(new { mensaje = "Contraseña actualizada correctamente." });
            }
            else
            {
                return NotFound(new { mensaje = "No se pudo actualizar la contraseña. Verifique el ID del usuario." });
            }
        }


        // PUT: api/Usuarios - Editar un usuario
        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] Usuarios usuario)
        {
            if (usuario == null || usuario.id_usuario == 0)
            {
                return BadRequest(new { isSuccess = false, message = "Datos inválidos" });
            }

            try
            {
                bool respuesta = await _data.Editar(usuario);
                return respuesta
                    ? Ok(new { isSuccess = true, message = "Usuario actualizado correctamente" })
                    : NotFound(new { isSuccess = false, message = "Usuario no encontrado" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { isSuccess = false, message = ex.Message });
            }
        }
        

        // DELETE: api/Usuarios/{id_usuario} - Eliminar un usuario
        [HttpDelete("{id_usuario}")]
        public async Task<IActionResult> Eliminar(int id_usuario)
        {
            try
            {
                bool respuesta = await _data.Eliminar(id_usuario);
                return respuesta
                    ? Ok(new { isSuccess = true, message = "Usuario eliminado correctamente" })
                    : NotFound(new { isSuccess = false, message = "Usuario no encontrado" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { isSuccess = false, message = ex.Message });
            }
        }
    }
}
