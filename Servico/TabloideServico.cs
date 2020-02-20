using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ITabloideServico : IBaseServico<Tabloide>
    {
        
    }

    public class TabloideServico : BaseServico<Tabloide, ITabloideRepositorio>, ITabloideServico
    {
    }
}
