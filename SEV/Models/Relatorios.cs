namespace SEV.Models
{
    public class Relatorios
    {

        public int Mes { get; set; }
        public int Ano { get; set; }
        public List<Venda> Vendas { get; set; } = new();

    }
}
