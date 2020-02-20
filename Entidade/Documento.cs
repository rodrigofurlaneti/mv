using System;
using System.ComponentModel.DataAnnotations;
using Entidade.Base;

namespace Entidade
{
    public class Documento : BaseEntity
    {
        [Required]
        public virtual int Tipo { get; set; }

        [Required]
        public virtual string Numero { get; set; }

        public virtual string OrgaoExpedidor { get; set; }

        public virtual DateTime? DataExpedicao { get; set; }

        public virtual Pessoa Pessoa { get; set; }
    }
}