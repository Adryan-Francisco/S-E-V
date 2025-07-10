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
    public class VendasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VendasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vendas
        public async Task<IActionResult> Index()
        {
            var vendas = await _context.Vendas
                .Include(v => v.Cliente) // ESSENCIAL!
                .ToListAsync();

            return View(vendas);
        }
        // GET: Vendas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(m => m.VendaId == id);
            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // GET
        public IActionResult Create()
        {
            ViewBag.ClienteId = new SelectList(_context.Clientes.ToList(), "ClienteId", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VendaId,DataVenda,ClienteId,Total")] Venda venda)
        {
            Console.WriteLine($"Venda recebida: ClienteId={venda.ClienteId}, Total={venda.Total}");

            if (ModelState.IsValid)
            {
                // ⚠️ Força DataVenda para UTC
                venda.DataVenda = DateTime.SpecifyKind(venda.DataVenda, DateTimeKind.Utc);

                _context.Add(venda);
                await _context.SaveChangesAsync();
                Console.WriteLine("Venda salva com sucesso!");
                return RedirectToAction(nameof(Index));
            }

            Console.WriteLine("ModelState inválido");
            foreach (var erro in ModelState.Values.SelectMany(e => e.Errors))
            {
                Console.WriteLine(erro.ErrorMessage);
            }

            ViewBag.ClienteId = new SelectList(_context.Clientes, "ClienteId", "Nome", venda.ClienteId);
            return View(venda);
        }

        // GET: Vendas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas.FindAsync(id);
            if (venda == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "CPF", venda.ClienteId);
            return View(venda);
        }

        // POST: Vendas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VendaId,DataVenda,ClienteId,Total")] Venda venda)
        {
            if (id != venda.VendaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // 🔧 Corrige o tipo da data para UTC (exigido pelo PostgreSQL)
                    venda.DataVenda = DateTime.SpecifyKind(venda.DataVenda, DateTimeKind.Utc);

                    _context.Update(venda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendaExists(venda.VendaId))
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

            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "CPF", venda.ClienteId);
            return View(venda);
        }
        // GET: Vendas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(m => m.VendaId == id);
            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // POST: Vendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venda = await _context.Vendas.FindAsync(id);
            if (venda != null)
            {
                _context.Vendas.Remove(venda);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendaExists(int id)
        {
            return _context.Vendas.Any(e => e.VendaId == id);
        }
        public async Task<IActionResult> TesteVendaDireta()
        {
            var venda = new Venda
            {
                DataVenda = DateTime.Now,
                ClienteId = 1, // ID existente!
                Total = 99.99M
            };

            _context.Add(venda);
            await _context.SaveChangesAsync();
            return Content("Venda cadastrada com ID: " + venda.VendaId);
        }
    }
}
