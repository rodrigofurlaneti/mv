using Entidade.Base;

namespace Entidade
{
    public class LojaTipoLoja : BaseEntity
    {
        public virtual TipoLoja TipoLoja { get; set; }
    }
}
