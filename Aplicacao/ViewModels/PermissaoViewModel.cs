using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class PermissaoViewModel
    {
        public PermissaoViewModel()
        {
            Perfis = new List<PerfilViewModel>();
        }
        
        public virtual string Nome { get; set; }
        public virtual string Regra { get; set; }
        public virtual IList<PerfilViewModel> Perfis { get; set; }
    }
}