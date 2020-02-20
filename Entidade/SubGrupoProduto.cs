using Entidade.Base;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class SubGrupoProduto : IEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual string Nome { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual GrupoProduto Grupo { get; set; }
    }
}
