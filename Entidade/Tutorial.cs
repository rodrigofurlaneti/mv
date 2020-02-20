using Entidade.Base;

namespace Entidade
{
    public class Tutorial : BaseEntity
    {
        public virtual string URL { get; set; }
        public virtual bool Logado { get; set; }
    }
}
