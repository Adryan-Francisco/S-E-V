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
            // Dados gerais
            int totalProdutos = await _context.Produtos.SumAsync(p => p.QuantidadeEstoque);
            int totalVendas = await _context.Vendas.CountAsync();
            decimal faturamentoTotal = await _context.Vendas.SumAsync(v => v.Total);

            // Vendas por mês (últimos 6 meses)
            var vendasMes = _context.Vendas
            .Where(v => v.DataVenda >= DateTime.UtcNow.AddMonths(-6))
            .AsEnumerable()
            .GroupBy(v => v.DataVenda.ToString("MMM"))
            .OrderBy(g => g.Key)
            .Select(g => new {
                Mes = g.Key,
                Total = g.Sum(v => v.Total)
            })
            .ToList();



            // Produtos com estoque baixo (≤ 5 unidades)
            var produtosBaixoEstoque = await _context.Produtos
                .Where(p => p.QuantidadeEstoque <= 5)
                .Select(p => new {
                    Nome = p.Nome,
                    Quantidade = p.QuantidadeEstoque
                }).ToListAsync();

            // Preencher o ViewModel
            var viewModel = new Dashboard
            {
                TotalProdutos = totalProdutos,
                TotalVendas = totalVendas,
                FaturamentoTotal = faturamentoTotal,
                Meses = vendasMes.Select(v => v.Mes).ToList(),
                VendasPorMes = vendasMes.Select(v => v.Total).ToList(),
                ProdutosBaixoEstoqueNomes = produtosBaixoEstoque.Select(p => p.Nome).ToList(),
                ProdutosBaixoEstoqueQtd = produtosBaixoEstoque.Select(p => p.Quantidade).ToList()
            };

            return View(viewModel);
        }
    }
}
