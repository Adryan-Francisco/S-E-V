using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SEV.Data;
using SEV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEV.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var produtos = await _context.Produtos.Include(p => p.Categoria).ToListAsync();
                var vendas = await _context.Vendas.Include(v => v.Itens!).ThenInclude(i => i.Produto).ToListAsync();
                var clientes = await _context.Clientes.ToListAsync();
                var categorias = await _context.Categorias.ToListAsync();
                var fornecedores = await _context.Fornecedores.ToListAsync();

                // Calcular estatísticas principais
                var totalProdutos = produtos.Count;
                var totalVendas = vendas.Count;
                var faturamentoTotal = vendas.Sum(v => v.TotalCalculado);
                var clientesAtivos = clientes.Count;

                // Novas métricas
                var produtosSemEstoque = produtos.Count(p => p.QuantidadeEstoque == 0);
                var produtosBaixoEstoque = produtos.Count(p => p.QuantidadeEstoque > 0 && p.QuantidadeEstoque <= 10);
                var categoriasAtivas = categorias.Count;
                var fornecedoresAtivos = fornecedores.Count;

                // Cálculos de crescimento
                var mesAtual = DateTime.Now.Month;
                var anoAtual = DateTime.Now.Year;
                var mesAnterior = mesAtual == 1 ? 12 : mesAtual - 1;
                var anoAnterior = mesAtual == 1 ? anoAtual - 1 : anoAtual;

                var faturamentoMesAtual = vendas
                    .Where(v => v.DataVenda.Month == mesAtual && v.DataVenda.Year == anoAtual)
                    .Sum(v => v.TotalCalculado);

                var faturamentoMesAnterior = vendas
                    .Where(v => v.DataVenda.Month == mesAnterior && v.DataVenda.Year == anoAnterior)
                    .Sum(v => v.TotalCalculado);

                var vendasMesAtual = vendas
                    .Count(v => v.DataVenda.Month == mesAtual && v.DataVenda.Year == anoAtual);

                var vendasMesAnterior = vendas
                    .Count(v => v.DataVenda.Month == mesAnterior && v.DataVenda.Year == anoAnterior);

                var crescimentoFaturamento = faturamentoMesAnterior > 0 
                    ? ((faturamentoMesAtual - faturamentoMesAnterior) / faturamentoMesAnterior) * 100 
                    : 0;

                var crescimentoVendas = vendasMesAnterior > 0 
                    ? ((vendasMesAtual - vendasMesAnterior) / (decimal)vendasMesAnterior) * 100 
                    : 0;

                var ticketMedio = totalVendas > 0 ? faturamentoTotal / totalVendas : 0;

                // Dados para gráficos
                var meses = vendas
                    .GroupBy(v => v.DataVenda.ToString("MM/yyyy"))
                    .OrderBy(g => g.Key)
                    .Select(g => g.Key)
                    .ToList();

                var vendasPorMes = vendas
                    .GroupBy(v => v.DataVenda.ToString("MM/yyyy"))
                    .OrderBy(g => g.Key)
                    .Select(g => g.Sum(v => v.TotalCalculado))
                    .ToList();

                var quantidadeVendasPorMes = vendas
                    .GroupBy(v => v.DataVenda.ToString("MM/yyyy"))
                    .OrderBy(g => g.Key)
                    .Select(g => g.Count())
                    .ToList();

                // Gráficos de produtos
                var produtosBaixoEstoqueNomes = produtos
                    .Where(p => p.QuantidadeEstoque > 0 && p.QuantidadeEstoque <= 10)
                    .OrderBy(p => p.QuantidadeEstoque)
                    .Take(8)
                    .Select(p => p.Nome)
                    .ToList();

                var produtosBaixoEstoqueQtd = produtos
                    .Where(p => p.QuantidadeEstoque > 0 && p.QuantidadeEstoque <= 10)
                    .OrderBy(p => p.QuantidadeEstoque)
                    .Take(8)
                    .Select(p => p.QuantidadeEstoque)
                    .ToList();

                // Top produtos por vendas
                var topProdutos = vendas
                    .SelectMany(v => v.Itens ?? new List<ItemVenda>())
                    .GroupBy(i => i.ProdutoId)
                    .Select(g => new { 
                        ProdutoId = g.Key, 
                        Nome = g.First().Produto?.Nome ?? "Produto não encontrado",
                        TotalVendas = g.Sum(i => i.Quantidade)
                    })
                    .OrderByDescending(p => p.TotalVendas)
                    .Take(8)
                    .ToList();

                var topProdutosNomes = topProdutos.Select(p => p.Nome).ToList();
                var topProdutosVendas = topProdutos.Select(p => p.TotalVendas).ToList();

                // Gráficos de categorias
                var categoriasComQuantidade = produtos
                    .GroupBy(p => p.Categoria?.Nome ?? "Sem Categoria")
                    .Select(g => new { Nome = g.Key, Quantidade = g.Count() })
                    .OrderByDescending(c => c.Quantidade)
                    .Take(8)
                    .ToList();

                var categoriasNomes = categoriasComQuantidade.Select(c => c.Nome).ToList();
                var categoriasQuantidade = categoriasComQuantidade.Select(c => c.Quantidade).ToList();

                // Dados para tabelas
                var produtosRecentes = produtos.OrderByDescending(p => p.ProdutoId).Take(5).ToList();
                var vendasRecentes = vendas.OrderByDescending(v => v.DataVenda).Take(5).ToList();
                var clientesRecentes = clientes.OrderByDescending(c => c.ClienteId).Take(5).ToList();

                // Gerar atividades recentes
                var atividadesRecentes = GerarAtividadesRecentes(produtos, vendas, clientes, categorias);

                // Gerar alertas
                var alertas = GerarAlertas(produtos, vendas, clientes);

                var viewModel = new DashboardViewModel
                {
                    // Estatísticas principais
                    TotalProdutos = totalProdutos,
                    TotalVendas = totalVendas,
                    FaturamentoTotal = faturamentoTotal,
                    ClientesAtivos = clientesAtivos,

                    // Novas métricas
                    ProdutosSemEstoque = produtosSemEstoque,
                    ProdutosBaixoEstoque = produtosBaixoEstoque,
                    FaturamentoMesAtual = faturamentoMesAtual,
                    FaturamentoMesAnterior = faturamentoMesAnterior,
                    CrescimentoFaturamento = crescimentoFaturamento,
                    VendasMesAtual = vendasMesAtual,
                    VendasMesAnterior = vendasMesAnterior,
                    CrescimentoVendas = crescimentoVendas,
                    TicketMedio = ticketMedio,
                    CategoriasAtivas = categoriasAtivas,
                    FornecedoresAtivos = fornecedoresAtivos,

                    // Dados para gráficos
                    Meses = meses,
                    VendasPorMes = vendasPorMes,
                    QuantidadeVendasPorMes = quantidadeVendasPorMes,

                    // Gráficos de produtos
                    ProdutosBaixoEstoqueNomes = produtosBaixoEstoqueNomes,
                    ProdutosBaixoEstoqueQtd = produtosBaixoEstoqueQtd,
                    TopProdutosNomes = topProdutosNomes,
                    TopProdutosVendas = topProdutosVendas,

                    // Gráficos de categorias
                    CategoriasNomes = categoriasNomes,
                    CategoriasQuantidade = categoriasQuantidade,

                    // Dados para tabelas
                    ProdutosRecentes = produtosRecentes,
                    VendasRecentes = vendasRecentes,
                    ClientesRecentes = clientesRecentes,

                    // Atividades e alertas
                    AtividadesRecentes = atividadesRecentes,
                    Alertas = alertas
                };
                
                return View(viewModel);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro no Dashboard: {ex.Message}");
                // Em caso de erro, retorna um viewModel vazio
                return View(new DashboardViewModel());
            }
        }

        private List<AtividadeRecente> GerarAtividadesRecentes(
            List<Produto> produtos, 
            List<Venda> vendas, 
            List<Cliente> clientes, 
            List<Categoria> categorias)
        {
            var atividades = new List<AtividadeRecente>();
            var dataAtual = DateTime.Now;

            // Atividades baseadas em produtos
            var produtosRecentes = produtos
                .OrderByDescending(p => p.ProdutoId)
                .Take(3)
                .ToList();

            foreach (var produto in produtosRecentes)
            {
                atividades.Add(new AtividadeRecente
                {
                    Id = produto.ProdutoId,
                    Tipo = "Produto",
                    Descricao = $"Produto '{produto.Nome}' cadastrado",
                    Usuario = "Sistema",
                    DataHora = dataAtual.AddDays(-new Random().Next(1, 7)),
                    Status = produto.QuantidadeEstoque > 0 ? "Ativo" : "Sem Estoque",
                    Icone = "bi-box",
                    CorStatus = produto.QuantidadeEstoque > 0 ? "success" : "warning",
                    Valor = $"Estoque: {produto.QuantidadeEstoque}"
                });
            }

            // Atividades baseadas em vendas
            var vendasRecentes = vendas
                .OrderByDescending(v => v.DataVenda)
                .Take(3)
                .ToList();

            foreach (var venda in vendasRecentes)
            {
                atividades.Add(new AtividadeRecente
                {
                    Id = venda.VendaId,
                    Tipo = "Venda",
                    Descricao = $"Venda realizada - R$ {venda.TotalCalculado:F2}",
                    Usuario = "Vendedor",
                    DataHora = venda.DataVenda,
                    Status = "Concluída",
                    Icone = "bi-cash-coin",
                    CorStatus = "success",
                    Valor = $"Itens: {venda.Itens?.Count ?? 0}"
                });
            }

            // Atividades baseadas em clientes
            var clientesRecentes = clientes
                .OrderByDescending(c => c.ClienteId)
                .Take(2)
                .ToList();

            foreach (var cliente in clientesRecentes)
            {
                atividades.Add(new AtividadeRecente
                {
                    Id = cliente.ClienteId,
                    Tipo = "Cliente",
                    Descricao = $"Cliente '{cliente.Nome}' cadastrado",
                    Usuario = "Sistema",
                    DataHora = dataAtual.AddDays(-new Random().Next(1, 5)),
                    Status = "Ativo",
                    Icone = "bi-person",
                    CorStatus = "info",
                    Valor = cliente.Telefone
                });
            }

            // Atividades baseadas em categorias
            var categoriasRecentes = categorias
                .OrderByDescending(c => c.CategoriaId)
                .Take(2)
                .ToList();

            foreach (var categoria in categoriasRecentes)
            {
                atividades.Add(new AtividadeRecente
                {
                    Id = categoria.CategoriaId,
                    Tipo = "Categoria",
                    Descricao = $"Categoria '{categoria.Nome}' criada",
                    Usuario = "Administrador",
                    DataHora = dataAtual.AddDays(-new Random().Next(1, 10)),
                    Status = "Ativa",
                    Icone = "bi-tags",
                    CorStatus = "primary",
                    Valor = $"Produtos: {produtos.Count(p => p.CategoriaId == categoria.CategoriaId)}"
                });
            }

            // Ordenar por data/hora mais recente e pegar as 8 mais recentes
            return atividades
                .OrderByDescending(a => a.DataHora)
                .Take(8)
                .ToList();
        }

        private List<Alerta> GerarAlertas(List<Produto> produtos, List<Venda> vendas, List<Cliente> clientes)
        {
            var alertas = new List<Alerta>();

            // Alerta para produtos sem estoque
            var produtosSemEstoque = produtos.Where(p => p.QuantidadeEstoque == 0).ToList();
            if (produtosSemEstoque.Any())
            {
                alertas.Add(new Alerta
                {
                    Tipo = "Estoque",
                    Mensagem = $"{produtosSemEstoque.Count} produto(s) sem estoque",
                    Icone = "bi-exclamation-triangle",
                    Cor = "warning",
                    Acao = "Ver Produtos",
                    Url = "/Produtos"
                });
            }

            // Alerta para produtos com baixo estoque
            var produtosBaixoEstoque = produtos.Where(p => p.QuantidadeEstoque > 0 && p.QuantidadeEstoque <= 5).ToList();
            if (produtosBaixoEstoque.Any())
            {
                alertas.Add(new Alerta
                {
                    Tipo = "Estoque Baixo",
                    Mensagem = $"{produtosBaixoEstoque.Count} produto(s) com estoque baixo",
                    Icone = "bi-exclamation-circle",
                    Cor = "info",
                    Acao = "Ver Produtos",
                    Url = "/Produtos"
                });
            }

            // Alerta para vendas do dia
            var vendasHoje = vendas.Where(v => v.DataVenda.Date == DateTime.Today).ToList();
            if (vendasHoje.Any())
            {
                alertas.Add(new Alerta
                {
                    Tipo = "Vendas Hoje",
                    Mensagem = $"{vendasHoje.Count} venda(s) realizadas hoje",
                    Icone = "bi-check-circle",
                    Cor = "success",
                    Acao = "Ver Vendas",
                    Url = "/VendasCompletas"
                });
            }

            // Alerta para novos clientes
            var clientesRecentes = clientes.Where(c => c.ClienteId > clientes.Max(cl => cl.ClienteId) - 3).ToList();
            if (clientesRecentes.Any())
            {
                alertas.Add(new Alerta
                {
                    Tipo = "Novos Clientes",
                    Mensagem = $"{clientesRecentes.Count} novo(s) cliente(s) cadastrado(s)",
                    Icone = "bi-person-plus",
                    Cor = "primary",
                    Acao = "Ver Clientes",
                    Url = "/Clientes"
                });
            }

            return alertas.Take(4).ToList();
        }
    }
}

