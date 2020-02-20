using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class CategoriaProdutoRepositorio : NHibRepository<CategoriaProduto>, ICategoriaProdutoRepositorio
	{
        public CategoriaProdutoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}
