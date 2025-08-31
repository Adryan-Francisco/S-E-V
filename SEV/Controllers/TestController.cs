using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SEV.Data;
using Microsoft.Extensions.Logging;

namespace SEV.Controllers
{
    public class TestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TestController> _logger;

        public TestController(ApplicationDbContext context, ILogger<TestController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var result = new Dictionary<string, object>();
            
            try
            {
                // Teste 1: Verificar se o contexto está configurado
                result["ContextConfigured"] = _context != null;
                
                // Teste 2: Verificar se conseguimos conectar ao banco
                var canConnect = await _context.Database.CanConnectAsync();
                result["CanConnect"] = canConnect;
                
                if (canConnect)
                {
                    // Teste 3: Verificar se o banco existe
                    result["DatabaseExists"] = true;
                    
                    // Teste 4: Verificar se as tabelas existem
                    try
                    {
                        var produtosCount = await _context.Produtos.CountAsync();
                        result["ProdutosTable"] = true;
                        result["ProdutosCount"] = produtosCount;
                    }
                    catch (Exception ex)
                    {
                        result["ProdutosTable"] = false;
                        result["ProdutosError"] = ex.Message;
                    }
                    
                    try
                    {
                        var vendasCount = await _context.Vendas.CountAsync();
                        result["VendasTable"] = true;
                        result["VendasCount"] = vendasCount;
                    }
                    catch (Exception ex)
                    {
                        result["VendasTable"] = false;
                        result["VendasError"] = ex.Message;
                    }
                    
                    try
                    {
                        var clientesCount = await _context.Clientes.CountAsync();
                        result["ClientesTable"] = true;
                        result["ClientesCount"] = clientesCount;
                    }
                    catch (Exception ex)
                    {
                        result["ClientesTable"] = false;
                        result["ClientesError"] = ex.Message;
                    }
                    
                    try
                    {
                        var categoriasCount = await _context.Categorias.CountAsync();
                        result["CategoriasTable"] = true;
                        result["CategoriasCount"] = categoriasCount;
                    }
                    catch (Exception ex)
                    {
                        result["CategoriasTable"] = false;
                        result["CategoriasError"] = ex.Message;
                    }
                }
                else
                {
                    result["DatabaseExists"] = false;
                    result["ConnectionError"] = "Não foi possível conectar ao banco de dados";
                }
            }
            catch (Exception ex)
            {
                result["GeneralError"] = ex.Message;
                result["StackTrace"] = ex.StackTrace;
                _logger.LogError(ex, "Erro durante teste de conexão");
            }
            
            return View(result);
        }
    }
}

