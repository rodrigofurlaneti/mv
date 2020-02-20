using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class ItemCompra : BaseEntity
    {
	    public virtual ListaCompra ListaCompra { get; set; }
        public virtual Produto Produto { get; set; }
        public virtual ProdutoPreco Preco { get; set; }
        public virtual int Quantidade { get; set; }
        public virtual decimal Total { get; set; }

	    public virtual StatusProdutoPreco StatusProdutoPreco { get; set; } = StatusProdutoPreco.Existente;

        public virtual Agendamento Agendamento { get; set; }
        
        public virtual Veiculo Veiculo { get; set; }

        public virtual PlanoVenda PlanoVenda { get; set; }

        public virtual Pessoa Beneficiario { get; set; }
    }
}