using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IVendedorServico : IBaseServico<Vendedor>
    {
    }

    public class VendedorServico : BaseServico<Vendedor, IVendedorRepositorio>, IVendedorServico
    {
        
    }
}