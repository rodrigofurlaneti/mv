using System;
using System.ComponentModel.DataAnnotations;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class Desconto : BaseEntity
    {
        public Desconto()
        {
            Validade = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
        }

        [Required]
        public virtual TipoDesconto TipoDesconto { get; set; }
        [Required]
        public virtual decimal ValorDesconto { get; set; }
        [Required]
        public virtual DateTime Validade { get; set; }
        [Required]
        public virtual int LimiteDeCompra { get; set; }
    }
}
