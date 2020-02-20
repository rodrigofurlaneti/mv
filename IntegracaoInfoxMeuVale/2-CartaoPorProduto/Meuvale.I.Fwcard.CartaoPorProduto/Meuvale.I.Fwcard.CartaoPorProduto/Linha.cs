namespace Meuvale.I.Fwcard.CartaoPorProduto
{
    public class Linha
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Produto { get; set; }
        public string Cartao { get; set; }
        public string Usuario { get; set; }
        public string Empresa { get; set; }
        public string Status { get; set; }
        public string Origem { get; set; }
        public string Data { get; set; }
        public Linha()
        {
        }
    }
}
