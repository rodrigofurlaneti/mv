using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IItemCompraServico : IBaseServico<ItemCompra>
    {
        
    }

    public class ItemCompraServico : BaseServico<ItemCompra, IItemCompraRepositorio>, IItemCompraServico
    {

    }
}