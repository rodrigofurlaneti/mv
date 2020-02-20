using Dominio.IRepositorioIntegracaoPCSist;
using EntidadePcSist;
using RepositorioIntegracaoPCSist.Base;

namespace RepositorioIntegracaoPCSist
{
    public class ProdutoRepositorio : NHibRepository<Produto>, IProdutoRepositorioIntegracao
    {
        public ProdutoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}
