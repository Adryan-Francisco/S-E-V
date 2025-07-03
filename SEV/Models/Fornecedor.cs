using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace SEV.Models
{
    public class Fornecedor
    {
        [Key]
        public int FornecedorId { get; set; }

        [Required]
        public string Nome { get; set; }

        public string CNPJ { get; set; }

        public string Telefone { get; set; }

        public string Endereco { get; set; }

        [ValidateNever] // <- Isso evita a validação do campo no formulário
        public virtual ICollection<Produto> Produtos { get; set; }
    }
}
