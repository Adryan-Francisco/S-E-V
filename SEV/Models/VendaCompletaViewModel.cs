using System.ComponentModel.DataAnnotations;

namespace SEV.Models
{
    public class VendaCompletaViewModel
    {
        // Dados da Venda
        [Required(ErrorMessage = "A data da venda é obrigatória")]
        [DataType(DataType.Date)]
        public DateTime DataVenda { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "O cliente é obrigatório")]
        public int ClienteId { get; set; }

        // Lista de itens da venda
        public List<ItemVendaViewModel> Itens { get; set; } = new List<ItemVendaViewModel>();

        // Propriedades calculadas
        public decimal Total => Itens?.Sum(i => i.Subtotal) ?? 0;
        public int TotalItens => Itens?.Sum(i => i.Quantidade) ?? 0;
    }

    public class ItemVendaViewModel
    {
        [Required(ErrorMessage = "O produto é obrigatório")]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero")]
        public int Quantidade { get; set; }

        [Required(ErrorMessage = "O preço unitário é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero")]
        public decimal PrecoUnitario { get; set; }

        // Propriedades para exibição
        public string? NomeProduto { get; set; }
        public int? EstoqueDisponivel { get; set; }
        public decimal Subtotal => Quantidade * PrecoUnitario;
    }

    // Extensão para calcular o total da venda
    public static class VendaCompletaViewModelExtensions
    {
        public static decimal CalcularTotal(this VendaCompletaViewModel venda)
        {
            return venda.Itens?.Sum(i => i.Subtotal) ?? 0;
        }
    }
}
