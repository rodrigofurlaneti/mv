using Entidade.Base;

namespace Entidade
{
    public class DadosBancario : BaseEntity
    {
        public virtual Banco Banco { get; set; }
        public virtual string Agencia { get; set; }
        public virtual string DigitoAgencia { get; set; }
        public virtual string Conta { get; set; }
        public virtual string DigitoConta { get; set; }
        public virtual string DocumentoTitular { get; set; }
        public virtual string NomeTitular { get; set; }
    }
}