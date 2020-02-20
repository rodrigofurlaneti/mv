using Entidade;

namespace Api.Models
{
    public class RegisterModel
    {
        public int Id { get; set; }
        public Pessoa Pessoa { get; set; }
        public string Senha { get; set; }
	    public string FacebookId { get; set; }
        public bool PrimeiroLogin { get; set; }
    }
}