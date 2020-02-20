using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class DepartamentoProdutoRepositorio : NHibRepository<DepartamentoProduto>, IDepartamentoProdutoRepositorio
    {
        public DepartamentoProdutoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}