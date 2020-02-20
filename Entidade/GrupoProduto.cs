using Entidade.Base;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class GrupoProduto : IEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual string Nome { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual SecaoProduto Secao { get; set; }
    }
}
