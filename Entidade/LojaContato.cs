using Entidade.Base;

namespace Entidade
{
    public class LojaContato : BaseEntity
    {
        public virtual Contato Contato { get; set; }
    }
}
