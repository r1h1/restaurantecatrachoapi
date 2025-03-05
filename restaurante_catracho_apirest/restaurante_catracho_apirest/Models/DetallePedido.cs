namespace restaurante_catracho_apirest.Models
{
    public class DetallePedidos
    {
        public int IdDetalle { get; set; }
        public int IdPedido { get; set; }
        public string NumeroPedido { get; set; }
        public int IdProducto { get; set; }
        public string? NombreProducto { get; set; } // Ahora permite valores nulos
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}