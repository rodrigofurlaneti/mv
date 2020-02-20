using Entidade.Base;

namespace Entidade
{
    public class QrCode : BaseEntity
    {
        public virtual string Url { get; set; }
        public virtual string CodigoConfirmacao { get; set; }
    }
}