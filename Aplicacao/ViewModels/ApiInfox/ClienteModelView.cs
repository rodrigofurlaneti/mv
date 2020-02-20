namespace Aplicacao.ApiInfox.Models
{
    public class ClienteModelView
    {
        public string Id { get; set; }

        public string CodCliente { get; set; }

        public PessoaModelView Pessoa { get; set; }

        public CartaoModelView Cartao { get; set; }
    }
}