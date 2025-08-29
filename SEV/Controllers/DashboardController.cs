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
                var produtos = await _context.Produtos.ToListAsync();
                var vendas = await _context.Vendas.Include(v => v.Itens).ToListAsync();
                var clientes = await _context.Clientes.ToListAsync();
                var categorias = await _context.Categorias.ToListAsync();

                // Log para debug
                System.Diagnostics.Debug.WriteLine($"Dashboard - Produtos: {produtos.Count}, Vendas: {vendas.Count}, Clientes: {clientes.Count}");

                // Calcular estatísticas
                var totalProdutos = produtos.Count;
                var totalVendas = vendas.Count;
                var faturamentoTotal = vendas.Sum(v => v.Total);
                var clientesAtivos = clientes.Count;

                // Dados para gráficos
                var meses = vendas
                    .GroupBy(v => v.DataVenda.ToString("MM/yyyy"))
                    .OrderBy(g => g.Key)
                    .Select(g => g.Key)
                    .ToList();

                var vendasPorMes = vendas
                    .GroupBy(v => v.DataVenda.ToString("MM/yyyy"))
                    .OrderBy(g => g.Key)
                    .Select(g => g.Sum(v => v.Total))
                    .ToList();

                var produtosBaixoEstoqueNomes = produtos
                    .Where(p => p.QuantidadeEstoque <= 10)
                    .Select(p => p.Nome)
                    .ToList();

                var produtosBaixoEstoqueQtd = produtos
                    .Where(p => p.QuantidadeEstoque <= 10)
                    .Select(p => p.QuantidadeEstoque)
                    .ToList();

                // Gerar atividades recentes baseadas em dados reais
                var atividadesRecentes = GerarAtividadesRecentes(produtos, vendas, clientes, categorias);

                var viewModel = new DashboardViewModel
                {
                    TotalProdutos = totalProdutos,
                    TotalVendas = totalVendas,
                    FaturamentoTotal = faturamentoTotal,
                    ClientesAtivos = clientesAtivos,

                    Meses = meses,
                    VendasPorMes = vendasPorMes,

                    ProdutosBaixoEstoqueNomes = produtosBaixoEstoqueNomes,
                    ProdutosBaixoEstoqueQtd = produtosBaixoEstoqueQtd,

                    AtividadesRecentes = atividadesRecentes
                };
                
                return View(viewModel);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro no Dashboard: {ex.Message}");
                // Em caso de erro, retorna um viewModel vazio
                var viewModel = new DashboardViewModel
                {
                    TotalProdutos = 0,
                    TotalVendas = 0,
                    FaturamentoTotal = 0,
                    ClientesAtivos = 0,
                    Meses = new List<string>(),
                    VendasPorMes = new List<decimal>(),
                    ProdutosBaixoEstoqueNomes = new List<string>(),
                    ProdutosBaixoEstoqueQtd = new List<int>(),
                    AtividadesRecentes = new List<AtividadeRecente>()
                };
                return View(viewModel);
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
                    CorStatus = produto.QuantidadeEstoque > 0 ? "success" : "warning"
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
                    Descricao = $"Venda realizada - R$ {venda.Total:F2}",
                    Usuario = "Vendedor",
                    DataHora = venda.DataVenda,
                    Status = "Concluída",
                    Icone = "bi-cash-coin",
                    CorStatus = "success"
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
                    CorStatus = "info"
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
                    CorStatus = "primary"
                });
            }

            // Ordenar por data/hora mais recente e pegar as 8 mais recentes
            return atividades
                .OrderByDescending(a => a.DataHora)
                .Take(8)
                .ToList();
        }
    }
}

