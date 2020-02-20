using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ItemCompraRepositorio : NHibRepository<ItemCompra>, IItemCompraRepositorio
    {
        public ItemCompraRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}
