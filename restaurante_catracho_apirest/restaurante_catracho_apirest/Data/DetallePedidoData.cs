using restaurante_catracho_apirest.Models;
using System.Data;
using System.Data.SqlClient;

namespace restaurante_catracho_apirest.Data
{
    public class DetallePedidosData
    {
        private readonly string conexion;

        public DetallePedidosData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL")!;
        }

        public async Task<List<DetallePedidos>> Lista()
        {
            List<DetallePedidos> lista = new List<DetallePedidos>();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetDetallesPedidos", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new DetallePedidos
                        {
                            IdDetalle = Convert.ToInt32(reader["id_detalle"]),
                            IdPedido = Convert.ToInt32(reader["id_pedido"]),
                            IdProducto = Convert.ToInt32(reader["id_producto"]),
                            NumeroPedido = reader["numero_pedido"].ToString()!,
                            Cantidad = Convert.ToInt32(reader["cantidad"]),
                            PrecioUnitario = Convert.ToDecimal(reader["precio_unitario"])
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<List<DetallePedidos>> ObtenerId(int id_pedido)
        {
            List<DetallePedidos> lista = new List<DetallePedidos>();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetDetallePedidoById", con);
                cmd.Parameters.AddWithValue("@id_pedido", id_pedido);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new DetallePedidos
                        {
                            IdDetalle = Convert.ToInt32(reader["id_detalle"]),
                            IdPedido = Convert.ToInt32(reader["id_pedido"]),
                            IdProducto = Convert.ToInt32(reader["id_producto"]),
                            NombreProducto = reader["nombre_producto"].ToString()!, // Se obtiene el nombre del producto
                            NumeroPedido = reader["numero_pedido"].ToString()!,
                            Cantidad = Convert.ToInt32(reader["cantidad"]),
                            PrecioUnitario = Convert.ToDecimal(reader["precio_unitario"])
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<bool> Crear(DetallePedidos objeto)
        {
            bool respuesta = true;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_InsertDetallePedido", con);
                cmd.Parameters.AddWithValue("@id_pedido", objeto.IdPedido);
                cmd.Parameters.AddWithValue("@id_producto", objeto.IdProducto);
                cmd.Parameters.AddWithValue("@numero_pedido", objeto.NumeroPedido);
                cmd.Parameters.AddWithValue("@cantidad", objeto.Cantidad);
                cmd.Parameters.AddWithValue("@precio_unitario", objeto.PrecioUnitario);
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

        public async Task<bool> Editar(DetallePedidos objeto)
        {
            bool respuesta = true;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateDetallePedido", con);
                cmd.Parameters.AddWithValue("@id_detalle", objeto.IdDetalle);
                cmd.Parameters.AddWithValue("@id_pedido", objeto.IdPedido);
                cmd.Parameters.AddWithValue("@id_producto", objeto.IdProducto);
                cmd.Parameters.AddWithValue("@numero_pedido", objeto.NumeroPedido);
                cmd.Parameters.AddWithValue("@cantidad", objeto.Cantidad);
                cmd.Parameters.AddWithValue("@precio_unitario", objeto.PrecioUnitario);
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

        public async Task<bool> Eliminar(int id_detalle)
        {
            bool respuesta = false;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteDetallePedido", con);
                cmd.Parameters.AddWithValue("@id_detalle", id_detalle);
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
