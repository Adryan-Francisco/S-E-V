using System;
using System.Collections.Generic;

namespace SEV.Models
{
    public class DashboardViewModel
    {
        public int TotalProdutos { get; set; }
        public int TotalVendas { get; set; }
        public decimal FaturamentoTotal { get; set; }
        public int ClientesAtivos { get; set; }

        public List<string> Meses { get; set; } = new();
        public List<decimal> VendasPorMes { get; set; } = new();

        public List<string> ProdutosBaixoEstoqueNomes { get; set; } = new();
        public List<int> ProdutosBaixoEstoqueQtd { get; set; } = new();

        // Atividades Recentes
        public List<AtividadeRecente> AtividadesRecentes { get; set; } = new();
    }

    public class AtividadeRecente
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public DateTime DataHora { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Icone { get; set; } = string.Empty;
        public string CorStatus { get; set; } = string.Empty;
    }
}
