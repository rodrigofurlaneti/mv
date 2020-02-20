using Entidade.Base;

namespace Entidade
{
    public class Convenio : BaseEntity
    {
        public Convenio()
        {
        }

        public virtual string Descricao { get; set; }
        public virtual string Cnpj { get; set; }
        public virtual string RazaoSocial { get; set; }
        public virtual bool Status { get; set; }
        public virtual Endereco Endereco { get; set; }
    }
}
