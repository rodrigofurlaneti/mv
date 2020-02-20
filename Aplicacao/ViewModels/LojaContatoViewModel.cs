
namespace Aplicacao.ViewModels
{
    public class LojaContatoViewModel
    {
        public LojaContatoViewModel()
        {
            Contato = new ContatoViewModel();
        }

        public ContatoViewModel Contato { get; set; }
    }
}