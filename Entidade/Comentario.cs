using Entidade.Base;

namespace Entidade
{
    public class Comentario : BaseEntity
    {
        public virtual Usuario Usuario { get; set; }
        public virtual string Titulo { get; set; }
        public virtual string Descricao { get; set; }
        public virtual int Estrelas { get; set; }
    }
}
