using Entidade.Base;

namespace Entidade
{
    public class LojaTipoCartao : BaseEntity
    {
        public virtual TipoCartao TipoCartao { get; set; }
    }
}
