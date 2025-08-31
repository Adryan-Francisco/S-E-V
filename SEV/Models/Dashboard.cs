using System;
using System.Collections.Generic;

namespace SEV.Models
{
    public class DashboardViewModel
    {
        // Estatísticas principais
        public int TotalProdutos { get; set; }
        public int TotalVendas { get; set; }
        public decimal FaturamentoTotal { get; set; }
        public int ClientesAtivos { get; set; }
        
        // Novas métricas
        public int ProdutosSemEstoque { get; set; }
        public int ProdutosBaixoEstoque { get; set; }
        public decimal FaturamentoMesAtual { get; set; }
        public decimal FaturamentoMesAnterior { get; set; }
        public decimal CrescimentoFaturamento { get; set; }
        public int VendasMesAtual { get; set; }
        public int VendasMesAnterior { get; set; }
        public decimal CrescimentoVendas { get; set; }
        public decimal TicketMedio { get; set; }
        public int CategoriasAtivas { get; set; }
        public int FornecedoresAtivos { get; set; }

        // Dados para gráficos
        public List<string> Meses { get; set; } = new();
        public List<decimal> VendasPorMes { get; set; } = new();
        public List<int> QuantidadeVendasPorMes { get; set; } = new();

        // Gráficos de produtos
        public List<string> ProdutosBaixoEstoqueNomes { get; set; } = new();
        public List<int> ProdutosBaixoEstoqueQtd { get; set; } = new();
        public List<string> TopProdutosNomes { get; set; } = new();
        public List<int> TopProdutosVendas { get; set; } = new();

        // Gráficos de categorias
        public List<string> CategoriasNomes { get; set; } = new();
        public List<int> CategoriasQuantidade { get; set; } = new();

        // Dados para tabelas
        public List<Produto> ProdutosRecentes { get; set; } = new();
        public List<Venda> VendasRecentes { get; set; } = new();
        public List<Cliente> ClientesRecentes { get; set; } = new();

        // Atividades Recentes
        public List<AtividadeRecente> AtividadesRecentes { get; set; } = new();
        
        // Alertas e notificações
        public List<Alerta> Alertas { get; set; } = new();
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
        public string Valor { get; set; } = string.Empty;
    }

    public class Alerta
    {
        public string Tipo { get; set; } = string.Empty;
        public string Mensagem { get; set; } = string.Empty;
        public string Icone { get; set; } = string.Empty;
        public string Cor { get; set; } = string.Empty;
        public string Acao { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
