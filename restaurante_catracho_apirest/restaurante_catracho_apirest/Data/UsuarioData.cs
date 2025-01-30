using restaurante_catracho_apirest.Models;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

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
                            id_usuario = Convert.ToInt32(reader["id_usuario"])!,
                            nombre = reader["nombre"].ToString()!,
                            correo = reader["correo"].ToString()!,
                            contraseña = reader["contraseña"].ToString()!,
                            rol = reader["rol"].ToString()!,
                            telefono = reader["telefono"].ToString()!,
                            direccion = reader["direccion"].ToString()!
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
                cmd.Parameters.AddWithValue("@id_empleado", id_usuario);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        objeto = new Usuarios
                        {
                            id_usuario = Convert.ToInt32(reader["id_usuario"]),
                            nombre = reader["nombre"].ToString(),
                            correo = reader["correo"].ToString(),
                            contraseña = reader["contraseña"].ToString(),
                            rol = reader["rol"].ToString(),
                            telefono = reader["telefono"].ToString(),
                            direccion = reader["direccion"].ToString()
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
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_InsertUsuario", con);
                cmd.Parameters.AddWithValue("@nombre", objeto.nombre);
                cmd.Parameters.AddWithValue("@correo", objeto.correo);
                cmd.Parameters.AddWithValue("@contraseña", objeto.contraseña);
                cmd.Parameters.AddWithValue("@rol", objeto.rol);
                cmd.Parameters.AddWithValue("@telefono", objeto.telefono);
                cmd.Parameters.AddWithValue("@direccion", objeto.direccion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await con.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;
                }
                catch (Exception)
                {
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
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_UpdateUsuario", con);
                cmd.Parameters.AddWithValue("@id_usuario", objeto.id_usuario);
                cmd.Parameters.AddWithValue("@nombre", objeto.nombre);
                cmd.Parameters.AddWithValue("@correo", objeto.correo);
                cmd.Parameters.AddWithValue("@contraseña", objeto.contraseña);
                cmd.Parameters.AddWithValue("@rol", objeto.rol);
                cmd.Parameters.AddWithValue("@telefono", objeto.telefono);
                cmd.Parameters.AddWithValue("@direccion", objeto.direccion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await con.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;
                }
                catch (Exception)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public async Task<bool> Eliminar(int id_usuario)
        {
            bool respuesta = true;

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_DeleteUsuario", con);
                cmd.Parameters.AddWithValue("@id_usuario", id_usuario);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await con.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;
                }
                catch (Exception)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }
    }
}
