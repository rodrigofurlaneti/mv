using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IPlanoVendaServico : IBaseServico<PlanoVenda>
    {
    }

    public class PlanoVendaServico : BaseServico<PlanoVenda, IPlanoVendaRepositorio>, IPlanoVendaServico
    {
        
    }
}
