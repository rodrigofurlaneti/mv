using Entidade.Base;

namespace Entidade
{
    public class LojaProduto : BaseEntity
    {
        public virtual Produto Produto { get; set; }
    }
}
