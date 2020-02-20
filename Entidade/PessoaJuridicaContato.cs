using Entidade.Base;

namespace Entidade
{
    public class PessoaJuridicaContato : BaseEntity
    {
        public virtual Contato Contato { get; set; }
    }
}
