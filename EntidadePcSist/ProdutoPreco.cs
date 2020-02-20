using System;
using System.Runtime.Serialization;
using EntidadePcSist.Base;

namespace EntidadePcSist
{
    public class ProdutoPreco : IEntityPcSist
    {
        public virtual decimal Valor { get; set; }
        [IgnoreDataMember]
        public virtual Produto Produto { get; set; }
        [IgnoreDataMember]
        public virtual Cliente Loja { get; set; }
        public virtual int Id
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}