using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IDispositivoServico : IBaseServico<Dispositivo>
    {
    }

    public class DispositivoServico : BaseServico<Dispositivo, IDispositivoRepositorio>, IDispositivoServico
    {
    }
}