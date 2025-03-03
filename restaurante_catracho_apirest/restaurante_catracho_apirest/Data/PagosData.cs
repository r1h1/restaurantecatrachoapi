using restaurante_catracho_apirest.Models;
using System.Data;
using System.Data.SqlClient;

namespace restaurante_catracho_apirest.Data
{
    public class PagosData
    {
        private readonly string conexion;

        public PagosData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL")!;
        }

        public async Task<List<Pagos>> Lista()
        {
            List<Pagos> lista = new List<Pagos>();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetPagos", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new Pagos
                        {
                            IdPago = Convert.ToInt32(reader["id_pago"]),
                            IdPedido = Convert.ToInt32(reader["id_pedido"]),
                            NumeroPedido = reader["numero_pedido"].ToString()!,
                            Monto = Convert.ToDecimal(reader["monto"]),
                            MetodoPago = reader["metodo_pago"].ToString()!,
                            FechaPago = Convert.ToDateTime(reader["fecha_pago"]),
                            EstadoPago = reader["estado_pago"].ToString()!
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<Pagos> ObtenerId(int id_pago)
        {
            Pagos objeto = new Pagos();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetPagoById", con);
                cmd.Parameters.AddWithValue("@id_pago", id_pago);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        objeto = new Pagos
                        {
                            IdPago = Convert.ToInt32(reader["id_pago"]),
                            IdPedido = Convert.ToInt32(reader["id_pedido"]),
                            NumeroPedido = reader["numero_pedido"].ToString()!,
                            Monto = Convert.ToDecimal(reader["monto"]),
                            MetodoPago = reader["metodo_pago"].ToString()!,
                            FechaPago = Convert.ToDateTime(reader["fecha_pago"]),
                            EstadoPago = reader["estado_pago"].ToString()!
                        };
                    }
                }
            }
            return objeto;
        }

        public async Task<bool> Crear(Pagos objeto)
        {
            bool respuesta = true;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_InsertPago", con);
                cmd.Parameters.AddWithValue("@id_pedido", objeto.IdPedido);
                cmd.Parameters.AddWithValue("@numero_pedido", objeto.NumeroPedido);
                cmd.Parameters.AddWithValue("@monto", objeto.Monto);
                cmd.Parameters.AddWithValue("@metodo_pago", objeto.MetodoPago);
                cmd.Parameters.AddWithValue("@estado_pago", objeto.EstadoPago);
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

        public async Task<bool> Editar(Pagos objeto)
        {
            bool respuesta = true;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdatePago", con);
                cmd.Parameters.AddWithValue("@id_pago", objeto.IdPago);
                cmd.Parameters.AddWithValue("@id_pedido", objeto.IdPedido);
                cmd.Parameters.AddWithValue("@numero_pedido", objeto.NumeroPedido);
                cmd.Parameters.AddWithValue("@monto", objeto.Monto);
                cmd.Parameters.AddWithValue("@metodo_pago", objeto.MetodoPago);
                cmd.Parameters.AddWithValue("@estado_pago", objeto.EstadoPago);
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

        public async Task<bool> Eliminar(int id_pago)
        {
            bool respuesta = false;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_DeletePago", con);
                cmd.Parameters.AddWithValue("@id_pago", id_pago);
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