using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation; // Necessário!

namespace SEV.Models
{
    public class Produto
    {
        [Key]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        public string Descricao { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório.")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "A quantidade em estoque é obrigatória.")]
        public int QuantidadeEstoque { get; set; }

        [Required(ErrorMessage = "A categoria é obrigatória.")]
        public int? CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        [ValidateNever] // 🚨 Impede validação automática de objetos complexos
        public virtual Categoria Categoria { get; set; }

        [Required(ErrorMessage = "O fornecedor é obrigatório.")]
        public int? FornecedorId { get; set; }

        [ForeignKey("FornecedorId")]
        [ValidateNever] // 🚨 Mesmo motivo
        public virtual Fornecedor Fornecedor { get; set; }
    }
}
