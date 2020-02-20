using Entidade.Base;

namespace Entidade
{
    public class PushNotification : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual string Mensagem { get; set; }
    }
}