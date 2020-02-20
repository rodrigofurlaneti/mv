using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class InformacaoProdutoRepositorio : NHibRepository<InformacaoProduto>, IInformacaoProdutoRepositorio
    {
        public InformacaoProdutoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}
