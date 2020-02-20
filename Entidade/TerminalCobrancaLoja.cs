using Entidade.Base;

namespace Entidade
{
    public class TerminalCobrancaLoja : IEntity
    {
        public TerminalCobrancaLoja()
        {

        }

        public virtual int Id { get; set; }
        public virtual Loja Loja { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual TerminalCobranca Terminal { get; set; }
    }
}
