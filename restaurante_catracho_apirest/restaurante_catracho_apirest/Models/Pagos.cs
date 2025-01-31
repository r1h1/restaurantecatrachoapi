namespace restaurante_catracho_apirest.Models
{
    public class Pagos
    {
        public int IdPago { get; set; }
        public int IdPedido { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; }
        public DateTime FechaPago { get; set; }
        public string EstadoPago { get; set; }
    }
}