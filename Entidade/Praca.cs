using Entidade.Base;

namespace Entidade
{
    public class Praca : IEntity
    {
        public virtual int Id { get; set; }
        public virtual string Descricao {get; set;}
    }
}