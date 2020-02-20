using Dominio.IRepositorioIntegracaoPCSist;
using EntidadePcSist;
using RepositorioIntegracaoPCSist.Base;

namespace RepositorioIntegracaoPCSist
{
    public class InformacaoProdutoRepositorio : NHibRepository<InformacaoProduto>, IInformacaoProdutoRepositorioIntegracao
    {
        public InformacaoProdutoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}
