using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SEV.Data;
using SEV.Models;

namespace SEV.Controllers
{
    public class ItemVendasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemVendasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ItemVendas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ItensVenda.Include(i => i.Produto).Include(i => i.Venda);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ItemVendas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemVenda = await _context.ItensVenda
                .Include(i => i.Produto)
                .Include(i => i.Venda)
                .FirstOrDefaultAsync(m => m.ItemVendaId == id);
            if (itemVenda == null)
            {
                return NotFound();
            }

            return View(itemVenda);
        }

        // GET
        public IActionResult Create()
        {
            ViewBag.VendaId = new SelectList(_context.Vendas.ToList(), "VendaId", "VendaId");
            ViewBag.ProdutoId = new SelectList(_context.Produtos.ToList(), "ProdutoId", "Nome");
            return View();
        }

        // POST
       [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create([Bind("ItemVendaId,VendaId,ProdutoId,Quantidade,PrecoUnitario")] ItemVenda itemVenda)
{
    if (ModelState.IsValid)
    {
        var produto = await _context.Produtos.FindAsync(itemVenda.ProdutoId);

        if (produto == null)
        {
            ModelState.AddModelError("", "Produto não encontrado.");
            return View(itemVenda);
        }

        if (produto.QuantidadeEstoque < itemVenda.Quantidade)
        {
            ModelState.AddModelError("", "Estoque insuficiente para este produto.");
            return View(itemVenda);
        }

        // Subtrair a quantidade vendida do estoque
        produto.QuantidadeEstoque -= itemVenda.Quantidade;

        // Registrar o preço atual
        itemVenda.PrecoUnitario = produto.Preco;

        _context.Add(itemVenda);
        _context.Update(produto); // Atualiza o produto com estoque novo

        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Vendas", new { id = itemVenda.VendaId });
    }

    ViewData["ProdutoId"] = new SelectList(_context.Produtos, "ProdutoId", "Nome", itemVenda.ProdutoId);
    ViewData["VendaId"] = new SelectList(_context.Vendas, "VendaId", "VendaId", itemVenda.VendaId);

    return View(itemVenda);
}




        // GET: ItemVendas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemVenda = await _context.ItensVenda.FindAsync(id);
            if (itemVenda == null)
            {
                return NotFound();
            }
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "ProdutoId", "Nome", itemVenda.ProdutoId);
            ViewData["VendaId"] = new SelectList(_context.Vendas, "VendaId", "VendaId", itemVenda.VendaId);
            return View(itemVenda);
        }

        // POST: ItemVendas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemVendaId,VendaId,ProdutoId,Quantidade,PrecoUnitario")] ItemVenda itemVenda)
        {
            if (id != itemVenda.ItemVendaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemVenda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemVendaExists(itemVenda.ItemVendaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "ProdutoId", "Nome", itemVenda.ProdutoId);
            ViewData["VendaId"] = new SelectList(_context.Vendas, "VendaId", "VendaId", itemVenda.VendaId);
            return View(itemVenda);
        }

        // GET: ItemVendas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemVenda = await _context.ItensVenda
                .Include(i => i.Produto)
                .Include(i => i.Venda)
                .FirstOrDefaultAsync(m => m.ItemVendaId == id);

            if (itemVenda == null)
            {
                return NotFound();
            }

            return View(itemVenda);
        }

        // POST: ItemVendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var itemVenda = await _context.ItensVenda
                .Include(i => i.Produto)
                .FirstOrDefaultAsync(i => i.ItemVendaId == id);

            if (itemVenda != null)
            {
                // Devolver a quantidade ao estoque
                if (itemVenda.Produto != null)
                {
                    itemVenda.Produto.QuantidadeEstoque += itemVenda.Quantidade;
                    _context.Update(itemVenda.Produto);
                }

                _context.ItensVenda.Remove(itemVenda);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool ItemVendaExists(int id)
        {
            return _context.ItensVenda.Any(e => e.ItemVendaId == id);
        }
    }
}
