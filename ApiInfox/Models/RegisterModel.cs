using Entidade;

namespace ApiInfox.Models
{
    public class RegisterModel
    {
        public int Id { get; set; }
        public Pessoa Pessoa { get; set; }
        public string Senha { get; set; }
	    public string FacebookId { get; set; }
    }
}