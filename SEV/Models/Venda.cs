using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SEV.Models
{
    public class Venda
    {
        public int VendaId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DataVenda { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public Cliente? Cliente { get; set; }


        public List<ItemVenda>? Itens { get; set; }

        public decimal Total { get; set; }
    }
}
