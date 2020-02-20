using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entidade.Base;

namespace Entidade
{
    public class Rede : BaseEntity
    {
        [Required, StringLength(2000)]
        public virtual string RazaoSocial { get; set; }
        public virtual bool Importado { get; set; }
        public virtual string TipoOperacao { get; set; }
        public virtual string CpfCnpj { get; set; }
        public virtual string InscricaoEstadual { get; set; }
        public virtual int CodigoPraca { get; set; }

        public virtual IList<Loja> Lojas { get; set; }
    }
}