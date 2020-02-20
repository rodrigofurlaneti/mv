using Entidade.Base;

namespace Entidade
{
    public class UsuarioConvenio : BaseEntity
    {
        public virtual Convenio Convenio { get; set; }
    }
}
