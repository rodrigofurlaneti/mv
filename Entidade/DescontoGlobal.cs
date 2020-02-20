using System.ComponentModel.DataAnnotations;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class DescontoGlobal : BaseEntity
    {
        [Required]
        public virtual ProdutoPreco ProdutoPreco { get; set; }
        [Required]
        public virtual Desconto Desconto { get; set; }

        public virtual decimal ValorDesconto
            => Desconto?.TipoDesconto == TipoDesconto.Percentual
                ? (ProdutoPreco?.Valor ?? 0) * (decimal) Desconto?.ValorDesconto / 100
                : Desconto?.ValorDesconto ?? 0;

        public virtual decimal ValorProdutoComDesconto => (ProdutoPreco?.Valor ?? 0) - ValorDesconto;
    }
}
