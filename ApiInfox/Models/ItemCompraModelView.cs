using System;
using Entidade;
using Entidade.Uteis;

namespace ApiInfox.Models
{
    public class ItemCompraModelView
    {
        public ItemCompraModelView(ItemCompra itemCompra)
        {
            if (itemCompra == null)
                throw new Exception("O item compra não foi encontrado.");

            Produto = new ProdutoModelView(itemCompra.Produto);
            Preco = new ProdutoPrecoModelView(itemCompra.Produto, itemCompra.Preco);
            Quantidade = itemCompra.Quantidade;
            Total = itemCompra.Total;
            StatusProdutoPreco = itemCompra.StatusProdutoPreco;
        }

        public ProdutoModelView Produto { get; set; }
        public ProdutoPrecoModelView Preco { get; set; }
        public int Quantidade { get; set; }
        public decimal Total { get; set; }

        public StatusProdutoPreco StatusProdutoPreco { get; set; } = StatusProdutoPreco.Existente;
    }
}