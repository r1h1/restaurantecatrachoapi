using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restaurante_catracho_apirest.Models
{
    public class Pedidos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPedido { get; set; }

        [Required]
        public int IdUsuario { get; set; }

        [Required]
        [StringLength(255)]
        public string NumeroPedido { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaEntregaEstimada { get; set; }

        [Required]
        public decimal MontoTotal { get; set; }
    }
}