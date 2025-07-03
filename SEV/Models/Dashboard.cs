public class Dashboard
{
    public int TotalProdutos { get; set; }
    public int TotalVendas { get; set; }
    public decimal FaturamentoTotal { get; set; }
    
    public List<string> Meses { get; set; }
    public List<decimal> VendasPorMes { get; set; }

    public List<string> ProdutosBaixoEstoqueNomes { get; set; }
    public List<int> ProdutosBaixoEstoqueQtd { get; set; }
}
