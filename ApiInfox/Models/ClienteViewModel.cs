namespace ApiInfox.Models
{
    public class ClienteModelView
    {
        public string Id { get; set; }

        public PessoaModelView Pessoa { get; set; }

        public CartaoModelView Cartao { get; set; }
    }
}