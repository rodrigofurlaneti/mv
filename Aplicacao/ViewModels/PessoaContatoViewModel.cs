
namespace Aplicacao.ViewModels
{
    public class PessoaContatoViewModel
    {
        public PessoaContatoViewModel()
        {
            Contato = new ContatoViewModel();
        }

        public ContatoViewModel Contato { get; set; }
    }
}