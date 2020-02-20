using Dominio.IRepositorioIntegracaoPCSist;
using EntidadePcSist;
using RepositorioIntegracaoPCSist.Base;

namespace RepositorioIntegracaoPCSist
{
    public class ProdutoPrecoRepositorio : NHibRepository<ProdutoPreco>, IProdutoPrecoRepositorioIntegracao
    {
        public ProdutoPrecoRepositorio(NHibContext context)
            : base(context)
        {
        }

        public ProdutoPreco GetByProduto(int produtoId)
        {
            return FirstBy(x => x.Produto.Id.Equals(produtoId));
        }
    }
}