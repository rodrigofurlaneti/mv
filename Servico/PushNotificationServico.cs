using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IPushNotificationServico : IBaseServico<PushNotification>
    {
    }

    public class PushNotificationServico : BaseServico<PushNotification, IPushNotificationRepositorio>, IPushNotificationServico
    {
    }
}