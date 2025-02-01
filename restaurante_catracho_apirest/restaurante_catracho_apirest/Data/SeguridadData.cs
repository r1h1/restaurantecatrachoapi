using restaurante_catracho_apirest.Models;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace restaurante_catracho_apirest.Data
{
    public class SeguridadData
    {
        private readonly string conexion;

        public SeguridadData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL")!;
        }

        public async Task<Usuarios?> ValidarUsuario(string correo, string clave)
        {
            Usuarios? usuario = null;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_GetUsuarioByEmailAndPass", con);
                cmd.Parameters.AddWithValue("@correo", correo);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await con.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            string claveAlmacenada = reader["clave"].ToString(); // Obtiene la clave encriptada desde la BD

                            // Verifica la clave ingresada con la clave almacenada en la base de datos
                            if (BCrypt.Net.BCrypt.Verify(clave, claveAlmacenada))
                            {
                                usuario = new Usuarios
                                {
                                    id_usuario = Convert.ToInt32(reader["id_usuario"]),
                                    nombre = reader["nombre"].ToString(),
                                    correo = reader["correo"].ToString(),
                                    clave = claveAlmacenada, // Guarda la clave encriptada
                                    rol = reader["rol"].ToString(),
                                    telefono = reader["telefono"] != DBNull.Value ? reader["telefono"].ToString() : null,
                                    direccion = reader["direccion"] != DBNull.Value ? reader["direccion"].ToString() : null
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al validar el usuario: " + ex.Message);
                }
            }

            return usuario;
        }
    }
}