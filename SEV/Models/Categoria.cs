using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace SEV.Models
{
    public class Categoria
    {
        [Key]
        public int CategoriaId { get; set; }

        [Required]
        public string Nome { get; set; }

        [ValidateNever] // <- Isso evita a validação do campo no formulário
        public virtual ICollection<Produto> Produtos { get; set; }

    }
}
