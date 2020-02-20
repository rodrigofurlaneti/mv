using System;
using System.ComponentModel.DataAnnotations;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class Cupom : BaseEntity
    {
        public Cupom()
        {
            ValidadeFinal = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
        }

        [Required]
        public virtual string Descricao { get; set; }
        [Required]
        public virtual TipoDesconto TipoDesconto { get; set; }
        [Required]
        public virtual decimal ValorCupom { get; set; }
        [Required]
        public virtual DateTime ValidadeFinal { get; set; }
        [Required]
        public virtual string CodigoCupom { get; set; }
    }
}
