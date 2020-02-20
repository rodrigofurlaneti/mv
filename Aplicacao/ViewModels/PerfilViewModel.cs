using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class PerfilViewModel
    {
        public PerfilViewModel()
        {
            Permissoes = new List<PermissaoViewModel>();
            Usuarios = new List<UsuarioViewModel>();
        }

        public string Nome { get; set; }
        public IList<PerfilMenuViewModel> Menus { get; set; }
        public IList<PermissaoViewModel> Permissoes { get; set; }
        public IList<UsuarioViewModel> Usuarios { get; set; }

        private List<int> ListaIds { get; set; }
        public List<int> ListaMenu
        {
            get
            {

                var retorno = new List<int>();

                if(Menus != null)
                    retorno.AddRange(Menus.Select(perfilMenu => perfilMenu.Menu.Id));

                return ListaIds != null && ListaIds.Any() 
                        ? ListaIds 
                        : retorno;
            }
            set { ListaIds = value; }
        }

    }
}