using System.ComponentModel.DataAnnotations;
using Entidade.Base;

namespace Entidade
{
    public class Cidade : BaseEntity
    {
        public virtual Estado Estado { get; set; }

        [Required]
        public virtual string Descricao { get; set; }
    }
}
