using restaurante_catracho_apirest.Models;
using System.Data;
using System.Data.SqlClient;

namespace restaurante_catracho_apirest.Data
{
    public class ProductoData
    {
        private readonly string conexion;

        public ProductoData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL")!;
        }

        public async Task<List<Productos>> Lista()
        {
            List<Productos> lista = new List<Productos>();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetProductos", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new Productos
                        {
                            IdProducto = Convert.ToInt32(reader["id_producto"]),
                            Nombre = reader["nombre"].ToString()!,
                            Descripcion = reader["descripcion"].ToString(),
                            Precio = Convert.ToDecimal(reader["precio"]),
                            Categoria = reader["categoria"].ToString(),
                            Disponible = Convert.ToBoolean(reader["disponible"])
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<Productos> ObtenerId(int id_producto)
        {
            Productos objeto = new Productos();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetProductoById", con);
                cmd.Parameters.AddWithValue("@id_producto", id_producto);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        objeto = new Productos
                        {
                            IdProducto = Convert.ToInt32(reader["id_producto"]),
                            Nombre = reader["nombre"].ToString(),
                            Descripcion = reader["descripcion"].ToString(),
                            Precio = Convert.ToDecimal(reader["precio"]),
                            Categoria = reader["categoria"].ToString(),
                            Disponible = Convert.ToBoolean(reader["disponible"])
                        };
                    }
                }
            }
            return objeto;
        }

        public async Task<bool> Crear(Productos objeto)
        {
            bool respuesta = true;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_InsertProducto", con);
                cmd.Parameters.AddWithValue("@nombre", objeto.Nombre);
                cmd.Parameters.AddWithValue("@descripcion", objeto.Descripcion);
                cmd.Parameters.AddWithValue("@precio", objeto.Precio);
                cmd.Parameters.AddWithValue("@categoria", objeto.Categoria);
                cmd.Parameters.AddWithValue("@disponible", objeto.Disponible);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await con.OpenAsync();
                    int filasAfectadas = await cmd.ExecuteNonQueryAsync();
                    respuesta = filasAfectadas > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en Crear: {ex.Message}");
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public async Task<bool> Editar(Productos objeto)
        {
            bool respuesta = true;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateProducto", con);
                cmd.Parameters.AddWithValue("@id_producto", objeto.IdProducto);
                cmd.Parameters.AddWithValue("@nombre", objeto.Nombre);
                cmd.Parameters.AddWithValue("@descripcion", objeto.Descripcion);
                cmd.Parameters.AddWithValue("@precio", objeto.Precio);
                cmd.Parameters.AddWithValue("@categoria", objeto.Categoria);
                cmd.Parameters.AddWithValue("@disponible", objeto.Disponible);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await con.OpenAsync();
                    int filasAfectadas = await cmd.ExecuteNonQueryAsync();
                    respuesta = filasAfectadas > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en Editar: {ex.Message}");
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public async Task<bool> Eliminar(int id_producto)
        {
            bool respuesta = false;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteProducto", con);
                cmd.Parameters.AddWithValue("@id_producto", id_producto);
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
