public class DashboardViewModel
{
    public int TotalProdutos { get; set; }
    public int TotalVendas { get; set; }
    public decimal FaturamentoTotal { get; set; }

    public List<string> Meses { get; set; } = new();
    public List<decimal> VendasPorMes { get; set; } = new();

    public List<string> ProdutosBaixoEstoqueNomes { get; set; } = new();
    public List<int> ProdutosBaixoEstoqueQtd { get; set; } = new();
}
