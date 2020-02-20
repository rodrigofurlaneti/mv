using System.ComponentModel.DataAnnotations;
using Entidade.Base;

namespace Entidade
{
    public class Dispositivo : BaseEntity
    {
        public virtual Usuario Usuario { get; set; }

        [Required]
        public virtual string Identificador { get; set; }
    }
}
