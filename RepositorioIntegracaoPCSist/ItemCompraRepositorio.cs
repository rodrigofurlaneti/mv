using Dominio.IRepositorioIntegracaoPCSist;
using EntidadePcSist;
using RepositorioIntegracaoPCSist.Base;

namespace RepositorioIntegracaoPCSist
{
    public class ItemCompraRepositorio : NHibRepository<ItemCompra>, IItemCompraRepositorioIntegracao
    {
        public ItemCompraRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}
