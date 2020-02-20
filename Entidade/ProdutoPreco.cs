using System;
using Entidade.Base;

namespace Entidade
{
    public class ProdutoPreco : BaseEntity
    {
        public ProdutoPreco()
        {
            InicioVigencia = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            FimVigencia = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
        }

        public virtual Produto Produto { get; set; }

        public virtual Loja Loja { get; set; }

        public virtual Fornecedor Fornecedor { get; set; }

        public virtual decimal Valor { get; set; }
        
        public virtual decimal ValorDesconto { get; set; }

        public virtual DateTime InicioVigencia { get; set; }

        public virtual DateTime FimVigencia { get; set; }

        public virtual bool Status { get; set; }

        public virtual string CodigoDesconto { get; set; }

        public virtual string LinkDesconto { get; set; }
    }
}