using Entidade.Base;

namespace Entidade
{
    public class UsuarioLoja : BaseEntity
    {
        public virtual Loja Loja { get; set; }
    }
}
