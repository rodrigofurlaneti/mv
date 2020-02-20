using System.ComponentModel.DataAnnotations;
using Entidade.Base;

namespace Entidade
{
    public class Endereco : BaseEntity
    {
        [Required, StringLength(10)]
        public virtual string Cep { get; set; }
        [Required, StringLength(200)]
        public virtual string Logradouro { get; set; }
        [Required, StringLength(10)]
        public virtual string Numero { get; set; }
        [StringLength(50)]
        public virtual string Complemento { get; set; }
        [Required, StringLength(100)]
        public virtual string Bairro { get; set; }
        [StringLength(100)]
        public virtual string Descricao { get; set; }
        public virtual string Latitude { get; set; }
        public virtual string Longitude { get; set; }
        [Required]
        public virtual int Tipo { get; set; }

        public virtual Cidade Cidade { get; set; }

        public virtual string Resumo => Logradouro + ", " + Numero + (!string.IsNullOrWhiteSpace(Complemento) ? " - " + Complemento : string.Empty) + (Cidade != null ? ", " + Cidade.Descricao : string.Empty);

        public virtual Pessoa Pessoa { get; set; }
    }
}