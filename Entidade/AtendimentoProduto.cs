using Entidade.Base;
using System.Runtime.Serialization;

namespace Entidade
{
    public class AtendimentoProduto : BaseEntity
    {
        public virtual string Descricao { get; set; }
        public virtual decimal Valor { get; set; }
        public virtual Produto Produto { get; set; }
    }
}