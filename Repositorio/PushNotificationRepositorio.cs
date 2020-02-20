using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class PushNotificationRepositorio : NHibRepository<PushNotification>, IPushNotificationRepositorio
    {
        public PushNotificationRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}