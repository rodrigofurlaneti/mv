using Entidade;

namespace Aplicacao.ApiInfox.Models
{
    public class CartaoModelView : ResponseModelView
    {
        public string NomeImpresso { get; set; }
        public TipoCartao TipoCartao { get; set; }
        public string Numero { get; set; }
        public string Cvv { get; set; }
        public string Senha { get; set; }

        public string SaldoDisponivel { get; set; }
        public string LimiteCredito { get; set; }
        public string DiaVencimento { get; set; }
    }
}