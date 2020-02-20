using System.ComponentModel.DataAnnotations;
using Entidade.Base;

namespace Entidade
{
    public class Estado : BaseEntity
    {
        [Required]
        public virtual string Descricao { get; set; }

        public virtual string Sigla { get; set; }

        public virtual Pais Pais { get; set; }
    }
}
