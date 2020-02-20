using System;
using System.ComponentModel.DataAnnotations;

namespace EntidadePcSist.Base
{
    public class Audit : IEntityPcSist
    {
        public Audit()
        {
        }

        public Audit(string usuario, string entidade, string atributo, int codigoEntidade, string valorAntigo, string valorNovo)
        {
            Usuario = usuario;
            Entidade = entidade;
            Atributo = atributo;
            CodigoEntidade = codigoEntidade;
            ValorAntigo = valorAntigo;
            ValorNovo = valorNovo;
            Data = DateTime.Now;
        }

        [Required]
        public virtual DateTime Data { get; set; }

        [Required, StringLength(50)]
        public virtual string Usuario { get; set; }

        [Required, StringLength(50)]
        public virtual string Entidade { get; set; }

        [Required, StringLength(50)]
        public virtual string Atributo { get; set; }

        [Required]
        public virtual int CodigoEntidade { get; set; }

        [Required, StringLength(200)]
        public virtual string ValorAntigo { get; set; }

        [Required, StringLength(200)]
        public virtual string ValorNovo { get; set; }
    }
}