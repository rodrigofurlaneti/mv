using System;
using Entidade;
using Entidade.Uteis;

namespace Api.Models
{
    public class DescontoPessoaModelView
    {
        public DescontoPessoaModelView()
        {
            
        }

        public DescontoPessoaModelView(Desconto desconto, Pessoa pessoa, ProdutoPreco produtoPreco)
        {
            if (desconto == null)
                throw new Exception("O desconto não foi encontrado.");

            if (pessoa == null)
                throw new Exception("A pessoa não foi encontrado.");

            if(produtoPreco == null)
                throw new Exception("Nenhum preço foi encontrado para o produto.");

            Desconto = new DescontoModelView(desconto);
            Pessoa = new PessoaModelView(pessoa);
            ProdutoPreco = new ProdutoPrecoModelView(produtoPreco.Produto, produtoPreco);
        }

        public PessoaModelView Pessoa { get; set; }
        public ProdutoPrecoModelView ProdutoPreco { get; set; }
        public DescontoModelView Desconto { get; set; }

        public virtual decimal ValorDesconto
            => Desconto?.TipoDesconto == TipoDesconto.Percentual
                ? (ProdutoPreco?.Valor ?? 0) * (decimal)Desconto?.ValorDesconto / 100
                : Desconto?.ValorDesconto ?? 0;

        public virtual decimal ValorProdutoComDesconto => (ProdutoPreco?.Valor ?? 0) - ValorDesconto;
    }
}