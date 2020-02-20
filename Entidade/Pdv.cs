using Entidade.Base;

namespace Entidade
{
    public class Pdv : BaseEntity
    {
        public virtual string Codigo { get; set; }
        public virtual string Descricao { get; set; }
        public virtual Loja Loja { get; set; }
    }
}