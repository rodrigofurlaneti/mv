using Entidade.Base;
using System;

namespace Entidade
{
    public class ProdutoAtivacaoLoja : BaseEntity
    {
        public ProdutoAtivacaoLoja()
        {
            DataInsercao = DateTime.Now;
        }

        public virtual bool ProdutoAlimentacao { get; set; }
        public virtual bool ProdutoRefeicao { get; set; }
        public virtual bool ProdutoAdiantamento { get; set; }
        public virtual bool ProdutoCombustivel { get; set; }
        public virtual bool ProdutoFarmacia { get; set; }
        public virtual decimal TaxaRefeicao { get; set; }
        public virtual decimal TaxaAlimentacao { get; set; }
        public virtual decimal TaxaCombustivel { get; set; }
        public virtual decimal TaxaAdiantamento { get; set; }
    }
}