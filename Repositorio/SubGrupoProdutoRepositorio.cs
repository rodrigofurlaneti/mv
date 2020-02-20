using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class SubGrupoProdutoRepositorio : NHibRepository<SubGrupoProduto>, ISubGrupoProdutoRepositorio
    {
        public SubGrupoProdutoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}