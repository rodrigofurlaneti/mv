using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IPushNotificationAplicacao : IBaseAplicacao<PushNotification>
    {
    }

    public class PushNotificationAplicacao : BaseAplicacao<PushNotification, IPushNotificationServico>, IPushNotificationAplicacao
    {
    }
}