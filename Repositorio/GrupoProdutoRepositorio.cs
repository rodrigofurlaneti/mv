using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class GrupoProdutoRepositorio : NHibRepository<GrupoProduto>, IGrupoProdutoRepositorio
    {
        public GrupoProdutoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}