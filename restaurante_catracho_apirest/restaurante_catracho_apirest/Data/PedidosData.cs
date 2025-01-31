using restaurante_catracho_apirest.Models;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restaurante_catracho_apirest.Data
{
    public class PedidosData
    {
        private readonly string conexion;

        public PedidosData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL")!;
        }

        // Listar todos los pedidos
        public async Task<List<Pedidos>> Lista()
        {
            List<Pedidos> lista = new List<Pedidos>();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetPedidos", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new Pedidos
                        {
                            IdPedido = Convert.ToInt32(reader["id_pedido"]),
                            IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                            Estado = reader["estado"].ToString(),
                            FechaCreacion = Convert.ToDateTime(reader["fecha_creacion"]),
                            FechaEntregaEstimada = reader["fecha_entrega_estimada"] != DBNull.Value
                                ? Convert.ToDateTime(reader["fecha_entrega_estimada"])
                                : (DateTime?)null,
                            MontoTotal = Convert.ToDecimal(reader["monto_total"])
                        });
                    }
                }
            }
            return lista;
        }

        // Obtener un pedido por ID
        public async Task<Pedidos> ObtenerId(int id_pedido)
        {
            Pedidos pedido = null;

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetPedidoById", con);
                cmd.Parameters.AddWithValue("@id_pedido", id_pedido);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        pedido = new Pedidos
                        {
                            IdPedido = Convert.ToInt32(reader["id_pedido"]),
                            IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                            Estado = reader["estado"].ToString(),
                            FechaCreacion = Convert.ToDateTime(reader["fecha_creacion"]),
                            FechaEntregaEstimada = reader["fecha_entrega_estimada"] != DBNull.Value
                                ? Convert.ToDateTime(reader["fecha_entrega_estimada"])
                                : (DateTime?)null,
                            MontoTotal = Convert.ToDecimal(reader["monto_total"])
                        };
                    }
                }
            }
            return pedido;
        }

        // Crear un pedido
        public async Task<bool> Crear(Pedidos pedido)
        {
            bool respuesta = false;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_InsertPedido", con);
                cmd.Parameters.AddWithValue("@id_usuario", pedido.IdUsuario);
                cmd.Parameters.AddWithValue("@estado", pedido.Estado.ToString());
                cmd.Parameters.AddWithValue("@fecha_entrega_estimada",
                    pedido.FechaEntregaEstimada.HasValue ? (object)pedido.FechaEntregaEstimada.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@monto_total", pedido.MontoTotal);
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
                }
            }
            return respuesta;
        }

        // Editar un pedido
        public async Task<bool> Editar(Pedidos pedido)
        {
            bool respuesta = false;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdatePedido", con);
                cmd.Parameters.AddWithValue("@id_pedido", pedido.IdPedido);
                cmd.Parameters.AddWithValue("@id_usuario", pedido.IdUsuario);
                cmd.Parameters.AddWithValue("@estado", pedido.Estado.ToString());
                cmd.Parameters.AddWithValue("@fecha_entrega_estimada",
                    pedido.FechaEntregaEstimada.HasValue ? (object)pedido.FechaEntregaEstimada.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@monto_total", pedido.MontoTotal);
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
                }
            }
            return respuesta;
        }

        // Eliminar un pedido
        public async Task<bool> Eliminar(int id_pedido)
        {
            bool respuesta = false;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_DeletePedido", con);
                cmd.Parameters.AddWithValue("@id_pedido", id_pedido);
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
                }
            }
            return respuesta;
        }
    }
}
