using Entidade.Base;

namespace Entidade
{
    public class TipoCartao : BaseEntity
    {
        public virtual string Descricao { get; set; }

        public virtual bool AdiantamentoSalarial { get; set; }
    }
}
