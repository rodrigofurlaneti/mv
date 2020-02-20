using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class SecaoProdutoRepositorio : NHibRepository<SecaoProduto>, ISecaoProdutoRepositorio
    {
        public SecaoProdutoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}