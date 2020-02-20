using Entidade.Base;

namespace Entidade
{
    public class UsuarioCombo : BaseEntity
    {
        public virtual PlanoVenda PlanoVenda { get; set; }
    }
}
