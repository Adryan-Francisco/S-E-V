using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SEV.Models
{
    public class ItemVenda
    {
        [Key]
        public int ItemVendaId { get; set; }

        [Required]
        public int VendaId { get; set; }

        [ForeignKey("VendaId")]
        public virtual Venda Venda { get; set; }

        [Required]
        public int ProdutoId { get; set; }

        [ForeignKey("ProdutoId")]
        public virtual Produto Produto { get; set; }

        [Required]
        public int Quantidade { get; set; }

        [Required]
        public decimal PrecoUnitario { get; set; }

        public decimal Subtotal => Quantidade * PrecoUnitario;
    }
}
