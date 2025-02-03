using restaurante_catracho_apirest.Models;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using BCrypt.Net;
using System.Text;

namespace restaurante_catracho_apirest.Data
{
    public class UsuarioData
    {
        private readonly string conexion;

        public UsuarioData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL")!;
        }

        public async Task<List<Usuarios>> Lista()
        {
            List<Usuarios> lista = new List<Usuarios>();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetUsuarios", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new Usuarios
                        {
                            id_usuario = Convert.ToInt32(reader["id_usuario"]),
                            nombre = Convert.ToBase64String(Encoding.UTF8.GetBytes(reader["nombre"].ToString())),
                            correo = Convert.ToBase64String(Encoding.UTF8.GetBytes(reader["correo"].ToString())),
                            clave = reader["clave"].ToString(), // Se mantiene sin cambios
                            rol = Convert.ToBase64String(Encoding.UTF8.GetBytes(reader["rol"].ToString())),
                            telefono = reader["telefono"] != DBNull.Value ? Convert.ToBase64String(Encoding.UTF8.GetBytes(reader["telefono"].ToString())) : null,
                            direccion = reader["direccion"] != DBNull.Value ? Convert.ToBase64String(Encoding.UTF8.GetBytes(reader["direccion"].ToString())) : null
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<Usuarios> ObtenerId(int id_usuario)
        {
            Usuarios objeto = new Usuarios();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetUsuarioById", con);
                cmd.Parameters.AddWithValue("@id_usuario", id_usuario);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        objeto = new Usuarios
                        {
                            id_usuario = Convert.ToInt32(reader["id_usuario"]),
                            nombre = Convert.ToBase64String(Encoding.UTF8.GetBytes(reader["nombre"].ToString())),
                            correo = Convert.ToBase64String(Encoding.UTF8.GetBytes(reader["correo"].ToString())),
                            clave = reader["clave"].ToString(), // Se mantiene sin cambios
                            rol = Convert.ToBase64String(Encoding.UTF8.GetBytes(reader["rol"].ToString())),
                            telefono = reader["telefono"] != DBNull.Value ? Convert.ToBase64String(Encoding.UTF8.GetBytes(reader["telefono"].ToString())) : null,
                            direccion = reader["direccion"] != DBNull.Value ? Convert.ToBase64String(Encoding.UTF8.GetBytes(reader["direccion"].ToString())) : null
                        };
                    }
                }
            }
            return objeto;
        }

        public async Task<bool> Crear(Usuarios objeto)
        {
            bool respuesta = true;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_InsertUsuario", con);
                cmd.Parameters.AddWithValue("@nombre", objeto.nombre);
                cmd.Parameters.AddWithValue("@correo", objeto.correo);
                cmd.Parameters.AddWithValue("@clave", BCrypt.Net.BCrypt.HashPassword(objeto.clave));
                cmd.Parameters.AddWithValue("@rol", objeto.rol);
                cmd.Parameters.AddWithValue("@telefono", objeto.telefono);
                cmd.Parameters.AddWithValue("@direccion", objeto.direccion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await con.OpenAsync();
                    int filasAfectadas = await cmd.ExecuteNonQueryAsync();
                    respuesta = filasAfectadas > 0;
                }
                catch (SqlException sqlx)
                {
                    Console.WriteLine($"Error en Editar (SQL): {sqlx.Number} - {sqlx.Message}");
                    respuesta = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en Editar (General): {ex.Message}");
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public async Task<bool> Editar(Usuarios objeto)
        {
            bool respuesta = true;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateUsuario", con);
                cmd.Parameters.AddWithValue("@id_usuario", Convert.ToInt32(objeto.id_usuario));
                cmd.Parameters.AddWithValue("@nombre", objeto.nombre);
                cmd.Parameters.AddWithValue("@correo", objeto.correo);
                cmd.Parameters.AddWithValue("@clave", objeto.clave);
                cmd.Parameters.AddWithValue("@rol", objeto.rol);
                cmd.Parameters.AddWithValue("@telefono", objeto.telefono);
                cmd.Parameters.AddWithValue("@direccion", objeto.direccion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await con.OpenAsync();
                    int filasAfectadas = await cmd.ExecuteNonQueryAsync();
                    respuesta = filasAfectadas > 0;
                }
                catch (SqlException sqlx)
                {
                    Console.WriteLine($"Error en Editar (SQL): {sqlx.Number} - {sqlx.Message}");
                    respuesta = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en Editar (General): {ex.Message}");
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public async Task<bool> ActualizarClave(int idUsuario, string nuevaClave)
        {
            bool respuesta = false;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdatePassword", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_usuario", idUsuario);

                //Encriptar la clave antes de almacenarla
                string nuevaClaveEncriptada = BCrypt.Net.BCrypt.HashPassword(nuevaClave);

                cmd.Parameters.AddWithValue("@nueva_clave", nuevaClaveEncriptada);

                try
                {
                    await con.OpenAsync();
                    int filasAfectadas = await cmd.ExecuteNonQueryAsync();
                    respuesta = filasAfectadas > 0;
                }
                catch (SqlException sqlx)
                {
                    Console.WriteLine($"Error en ActualizarClave (SQL): {sqlx.Number} - {sqlx.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en ActualizarClave (General): {ex.Message}");
                }
            }
            return respuesta;
        }

        public async Task<bool> Eliminar(int id_usuario)
        {
            bool respuesta = false;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteUsuario", con);
                cmd.Parameters.AddWithValue("@id_usuario", id_usuario);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await con.OpenAsync();
                    int filasAfectadas = await cmd.ExecuteNonQueryAsync();

                    respuesta = filasAfectadas > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en Eliminar: {ex.Message}");
                    respuesta = false;
                }
            }
            return respuesta;
        }
    }
}
