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
            var produtos = await _context.Produtos.ToListAsync();
            var vendas = await _context.Vendas.Include(v => v.Itens).ToListAsync();

            var viewModel = new DashboardViewModel
            {
                TotalProdutos = produtos.Count,
                TotalVendas = vendas.Count,
                FaturamentoTotal = vendas.Sum(v => v.Total),

                Meses = vendas
                    .GroupBy(v => v.DataVenda.ToString("MM/yyyy"))
                    .OrderBy(g => g.Key)
                    .Select(g => g.Key)
                    .ToList(),

                VendasPorMes = vendas
                    .GroupBy(v => v.DataVenda.ToString("MM/yyyy"))
                    .OrderBy(g => g.Key)
                    .Select(g => g.Sum(v => v.Total))
                    .ToList(),

                ProdutosBaixoEstoqueNomes = produtos
                    .Where(p => p.ProdutoId > 0)
                    .Select(p => p.Nome)
                    .ToList(),

                ProdutosBaixoEstoqueQtd = produtos
                    .Where(p => p.ProdutoId > 0)
                    .Select(p => p.QuantidadeEstoque)
                    .ToList()
            };

            return View(viewModel);
        }
    }
    }

