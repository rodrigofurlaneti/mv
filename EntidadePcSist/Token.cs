using DriveThru.Entidade.Base;

namespace DriveThru.Entidade
{
    public class Token : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual string Descricao { get; set; }
    }
}