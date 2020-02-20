using System;
using Entidade;
using Entidade.Uteis;

namespace Api.Models
{
    public class DescontoGlobalModelView
    {
        public DescontoGlobalModelView()
        {
            
        }

        public DescontoGlobalModelView(Desconto desconto, ProdutoPreco produtoPreco)
        {
            if (desconto == null)
                throw new Exception("O desconto não foi encontrado.");

            if(produtoPreco == null)
                throw new Exception("Nenhum preço foi encontrado para o produto.");

            Desconto = new DescontoModelView(desconto);
            ProdutoPreco = new ProdutoPrecoModelView(produtoPreco.Produto, produtoPreco);
        }

        public ProdutoPrecoModelView ProdutoPreco { get; set; }
        public DescontoModelView Desconto { get; set; }

        public virtual decimal ValorDesconto
            => Desconto?.TipoDesconto == TipoDesconto.Percentual
                ? (ProdutoPreco?.Valor ?? 0) * (decimal)Desconto?.ValorDesconto / 100
                : Desconto?.ValorDesconto ?? 0;

        public virtual decimal ValorProdutoComDesconto => (ProdutoPreco?.Valor ?? 0) - ValorDesconto;
    }
}