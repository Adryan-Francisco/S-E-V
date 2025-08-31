using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SEV.Data;
using SEV.Models;

namespace SEV.Controllers
{
    public class VendasCompletasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VendasCompletasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VendasCompletas
        public async Task<IActionResult> Index()
        {
            var vendas = await _context.Vendas
                .Include(v => v.Cliente)
                .Include(v => v.Itens)
                .ThenInclude(i => i.Produto)
                .OrderByDescending(v => v.DataVenda)
                .ToListAsync();

            return View(vendas);
        }

        // GET: VendasCompletas/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.ClienteId = new SelectList(await _context.Clientes.ToListAsync(), "ClienteId", "Nome");
            ViewBag.Produtos = await _context.Produtos
                .Where(p => p.QuantidadeEstoque > 0)
                .Select(p => new { p.ProdutoId, p.Nome, p.Preco, p.QuantidadeEstoque })
                .ToListAsync();

            var viewModel = new VendaCompletaViewModel
            {
                DataVenda = DateTime.Now,
                Itens = new List<ItemVendaViewModel>()
            };

            return View(viewModel);
        }

        // POST: VendasCompletas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] VendaCompletaViewModel viewModel)
        {
            // Log para debug
            Console.WriteLine($"Dados recebidos: DataVenda={viewModel.DataVenda}, ClienteId={viewModel.ClienteId}");
            Console.WriteLine($"Itens recebidos: {viewModel.Itens?.Count ?? 0}");
            
            if (!ModelState.IsValid)
            {
                Console.WriteLine($"ModelState inválido: {string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))}");
                return BadRequest(ModelState);
            }

            if (viewModel.Itens == null || !viewModel.Itens.Any())
            {
                ModelState.AddModelError("", "Adicione pelo menos um item à venda");
                return BadRequest(ModelState);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Criar a venda
                var venda = new Venda
                {
                    DataVenda = DateTime.SpecifyKind(viewModel.DataVenda, DateTimeKind.Utc),
                    ClienteId = viewModel.ClienteId,
                    Total = 0 // Será calculado automaticamente
                };

                _context.Vendas.Add(venda);
                await _context.SaveChangesAsync();

                // Criar os itens da venda
                foreach (var item in viewModel.Itens)
                {
                    var produto = await _context.Produtos.FindAsync(item.ProdutoId);
                    if (produto == null)
                    {
                        throw new InvalidOperationException($"Produto {item.ProdutoId} não encontrado");
                    }

                    if (produto.QuantidadeEstoque < item.Quantidade)
                    {
                        throw new InvalidOperationException($"Estoque insuficiente para o produto {produto.Nome}");
                    }

                    // Atualizar estoque
                    produto.QuantidadeEstoque -= item.Quantidade;

                    // Criar item da venda
                    var itemVenda = new ItemVenda
                    {
                        VendaId = venda.VendaId,
                        ProdutoId = item.ProdutoId,
                        Quantidade = item.Quantidade,
                        PrecoUnitario = item.PrecoUnitario
                    };

                    _context.ItensVenda.Add(itemVenda);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Json(new { success = true, vendaId = venda.VendaId, message = "Venda criada com sucesso!" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // GET: VendasCompletas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas
                .Include(v => v.Cliente)
                .Include(v => v.Itens)
                .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(v => v.VendaId == id);

            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // GET: VendasCompletas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas
                .Include(v => v.Cliente)
                .Include(v => v.Itens)
                .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(v => v.VendaId == id);

            if (venda == null)
            {
                return NotFound();
            }

            ViewBag.ClienteId = new SelectList(await _context.Clientes.ToListAsync(), "ClienteId", "Nome", venda.ClienteId);
            ViewBag.Produtos = await _context.Produtos.ToListAsync();

            var viewModel = new VendaCompletaViewModel
            {
                DataVenda = venda.DataVenda,
                ClienteId = venda.ClienteId,
                Itens = venda.Itens?.Select(i => new ItemVendaViewModel
                {
                    ProdutoId = i.ProdutoId,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario,
                    NomeProduto = i.Produto?.Nome,
                    EstoqueDisponivel = i.Produto?.QuantidadeEstoque
                }).ToList() ?? new List<ItemVendaViewModel>()
            };

            return View(viewModel);
        }

        // POST: VendasCompletas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromBody] VendaCompletaViewModel viewModel)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var venda = await _context.Vendas
                    .Include(v => v.Itens)
                    .FirstOrDefaultAsync(v => v.VendaId == id);

                if (venda == null)
                {
                    return NotFound();
                }

                // Atualizar dados da venda
                venda.DataVenda = DateTime.SpecifyKind(viewModel.DataVenda, DateTimeKind.Utc);
                venda.ClienteId = viewModel.ClienteId;
                venda.Total = 0; // Será calculado automaticamente

                // Remover itens existentes e devolver ao estoque
                foreach (var item in venda.Itens)
                {
                    var produto = await _context.Produtos.FindAsync(item.ProdutoId);
                    if (produto != null)
                    {
                        produto.QuantidadeEstoque += item.Quantidade;
                    }
                }
                _context.ItensVenda.RemoveRange(venda.Itens);

                // Adicionar novos itens
                foreach (var item in viewModel.Itens)
                {
                    var produto = await _context.Produtos.FindAsync(item.ProdutoId);
                    if (produto == null)
                    {
                        throw new InvalidOperationException($"Produto {item.ProdutoId} não encontrado");
                    }

                    if (produto.QuantidadeEstoque < item.Quantidade)
                    {
                        throw new InvalidOperationException($"Estoque insuficiente para o produto {produto.Nome}");
                    }

                    produto.QuantidadeEstoque -= item.Quantidade;

                    var itemVenda = new ItemVenda
                    {
                        VendaId = venda.VendaId,
                        ProdutoId = item.ProdutoId,
                        Quantidade = item.Quantidade,
                        PrecoUnitario = item.PrecoUnitario
                    };

                    _context.ItensVenda.Add(itemVenda);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Json(new { success = true, message = "Venda atualizada com sucesso!" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // GET: VendasCompletas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas
                .Include(v => v.Cliente)
                .Include(v => v.Itens)
                .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(v => v.VendaId == id);

            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // POST: VendasCompletas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venda = await _context.Vendas
                .Include(v => v.Itens)
                .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(v => v.VendaId == id);

            if (venda != null)
            {
                // Devolver itens ao estoque
                foreach (var item in venda.Itens)
                {
                    if (item.Produto != null)
                    {
                        item.Produto.QuantidadeEstoque += item.Quantidade;
                    }
                }

                _context.Vendas.Remove(venda);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // AJAX: Obter informações do produto
        [HttpGet]
        public async Task<IActionResult> GetProdutoInfo(int produtoId)
        {
            var produto = await _context.Produtos
                .Where(p => p.ProdutoId == produtoId)
                .Select(p => new { p.Nome, p.Preco, p.QuantidadeEstoque })
                .FirstOrDefaultAsync();

            if (produto == null)
            {
                return NotFound();
            }

            return Json(produto);
        }

        private bool VendaExists(int id)
        {
            return _context.Vendas.Any(e => e.VendaId == id);
        }
    }
}
