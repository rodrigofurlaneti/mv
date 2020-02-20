using Entidade.Base;

namespace Entidade
{
    public class InformacaoProduto : BaseEntity
    {
        public virtual int Tipo { get; set; }
        public virtual string Descricao { get; set; }
        public virtual Produto Produto { get; set; }
    }
}