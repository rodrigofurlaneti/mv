using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IDispositivoAplicacao : IBaseAplicacao<Dispositivo>
    {
    }

    public class DispositivoAplicacao : BaseAplicacao<Dispositivo, IDispositivoServico>, IDispositivoAplicacao
    {
    }
}