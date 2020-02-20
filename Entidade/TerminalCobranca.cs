using Entidade.Base;

namespace Entidade
{
    public class TerminalCobranca : BaseEntity
    {
        public TerminalCobranca() :base()
        {
        }

        public virtual string EnderecoIP { get; set; }
        public virtual string EnderecoMAC { get; set; }
        public virtual string Modelo { get; set; }
        public virtual string NumeroSerial { get; set; }
        public virtual int TipoTerminal { get; set; }

        public virtual string Maquininha { get; set; }
        public virtual string NomeGerenciadora { get; set; }
        public virtual string SoftwareHouse { get; set; }
        public virtual decimal TaxaCredito { get; set; }
        public virtual decimal TaxaDebito { get; set; }
    }
}
