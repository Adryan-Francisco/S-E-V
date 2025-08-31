using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SEV.Data;
using Microsoft.Extensions.Logging;

namespace SEV.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Testar conectividade com o banco
                var canConnect = await _context.Database.CanConnectAsync();
                ViewBag.DatabaseStatus = canConnect ? "Conectado" : "Desconectado";
                
                if (canConnect)
                {
                    // Testar se as tabelas existem
                    var produtosCount = await _context.Produtos.CountAsync();
                    var vendasCount = await _context.Vendas.CountAsync();
                    var clientesCount = await _context.Clientes.CountAsync();
                    
                    ViewBag.ProdutosCount = produtosCount;
                    ViewBag.VendasCount = vendasCount;
                    ViewBag.ClientesCount = clientesCount;
                }
                
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao verificar status do banco");
                ViewBag.DatabaseStatus = "Erro";
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
