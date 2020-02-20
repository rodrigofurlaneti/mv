using Entidade.Base;

namespace Entidade
{
    public class FornecedorProduto : BaseEntity
    {
        public virtual Produto Produto { get; set; }
    }
}
