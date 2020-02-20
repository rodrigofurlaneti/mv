using System;
using System.ComponentModel.DataAnnotations;
using Entidade.Base;

namespace Entidade
{
    public class Vendedor : IEntity
    {
        public Vendedor()
        {
            DataNascimento = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
        }

        public virtual int Id { get; set; }
        [Required]
        public virtual string Nome { get; set; }
        public virtual string Sobrenome { get; set; }
        public virtual string Sexo { get; set; }
        public virtual DateTime DataNascimento { get; set; }
        public virtual string Cpf { get; set; }
    }
}
