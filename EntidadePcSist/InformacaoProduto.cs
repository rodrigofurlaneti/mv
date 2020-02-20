using EntidadePcSist.Base;

namespace EntidadePcSist
{
    public class InformacaoProduto : IEntityPcSist
    {
        public virtual int Tipo { get; set; }
        public virtual string Descricao { get; set; }
        public virtual Produto Produto { get; set; }
    }
}