using Entidade.Base;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class DepartamentoProduto : IEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual string Nome { get; set; }

        public virtual CategoriaProduto CategoriaProduto { get; set; }

        public virtual string LogoUpload { get; set; }
    }
}
