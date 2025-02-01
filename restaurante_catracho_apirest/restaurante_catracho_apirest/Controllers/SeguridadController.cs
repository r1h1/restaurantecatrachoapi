using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using restaurante_catracho_apirest.Data;
using restaurante_catracho_apirest.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace restaurante_catracho_apirest.Controllers
{
    [EnableCors("NuevaPolitica")]
    [Route("api/[controller]")]
    [ApiController]
    public class SeguridadController : ControllerBase
    {
        private readonly string _secretKey;
        private readonly SeguridadData _seguridadData;

        public SeguridadController(IConfiguration config, SeguridadData seguridadData)
        {
            _secretKey = config.GetSection("settings").GetSection("secretkey").ToString();
            _seguridadData = seguridadData;
        }

        // POST: Autenticación de usuario
        [HttpPost]
        [Route("/v1/Security")]
        public async Task<IActionResult> Comparar([FromBody] Seguridad request)
        {
            if (!string.IsNullOrEmpty(request.correo) && !string.IsNullOrEmpty(request.contraseña))
            {
                var usuario = await _seguridadData.ValidarUsuario(request.correo, request.contraseña);

                if (usuario != null)
                {
                    string hashClave = BCrypt.Net.BCrypt.HashPassword(request.contraseña);

                    // Validar usuario y contraseña encriptada
                    if (string.Equals(usuario.correo, request.correo, StringComparison.Ordinal) &&
                        BCrypt.Net.BCrypt.Verify(request.contraseña, usuario.clave))
                    {
                        var keyBytes = Encoding.ASCII.GetBytes(_secretKey);
                        var claims = new ClaimsIdentity();
                        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, request.correo));
                        claims.AddClaim(new Claim(ClaimTypes.Role, usuario.rol));

                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = claims,
                            Expires = DateTime.UtcNow.AddHours(12),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature),
                        };

                        var tokenHandler = new JwtSecurityTokenHandler();
                        var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);
                        string tokenCreado = tokenHandler.WriteToken(tokenConfig);

                        // Usuario autenticado correctamente
                        return StatusCode(StatusCodes.Status201Created, new
                        {
                            code = 201,
                            message = "Ok",
                            id_usuario = Convert.ToBase64String(Encoding.UTF8.GetBytes(usuario.id_usuario.ToString())),
                            nombre = Convert.ToBase64String(Encoding.UTF8.GetBytes(usuario.nombre)),
                            correo = Convert.ToBase64String(Encoding.UTF8.GetBytes(usuario.correo)),
                            rol = Convert.ToBase64String(Encoding.UTF8.GetBytes(usuario.rol)),
                            telefono = Convert.ToBase64String(Encoding.UTF8.GetBytes(usuario.telefono)),
                            direccion = Convert.ToBase64String(Encoding.UTF8.GetBytes(usuario.direccion)),
                            auth = tokenCreado
                        });
                    }
                    else
                    {
                        // Credenciales incorrectas
                        return StatusCode(StatusCodes.Status400BadRequest, new { code = 400, message = "BadRequest", token = "" });
                    }
                }
                else
                {
                    // Usuario no encontrado
                    return StatusCode(StatusCodes.Status204NoContent, new { code = 204, message = "NoContent", token = "NoContent" });
                }
            }
            else
            {
                // Datos incompletos en la solicitud
                return StatusCode(StatusCodes.Status204NoContent, new { code = 204, message = "NoContent", token = "NoContent" });
            }
        }

        // GET: Validar usuario por correo y clave
        [HttpGet("{correo}/{clave}")]
        public async Task<IActionResult> ValidarUsuario(string correo, string clave)
        {
            var usuario = await _seguridadData.ValidarUsuario(correo, clave);
            if (usuario != null)
            {
                return StatusCode(StatusCodes.Status200OK, usuario);
            }
            return StatusCode(StatusCodes.Status204NoContent, new { code = 204, message = "NoContent" });
        }
    }
}
