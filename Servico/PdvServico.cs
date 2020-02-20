using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IPdvServico : IBaseServico<Pdv>
    {
    }

    public class PdvServico : BaseServico<Pdv, IPdvRepositorio>, IPdvServico
    {
    }
}