using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace restaurante_catracho_apirest.Models
{
    public class Pedidos
    {
        public int IdPedido { get; set; }
        public int IdUsuario { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaEntregaEstimada { get; set; }
        public decimal MontoTotal { get; set; }
    }
}
