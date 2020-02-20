using Entidade.Base;
using Entidade.Uteis;
using System;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class Banner : BaseEntity
    {
        public Banner()
        {
            DataInicio = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            DataFim = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
        }

        [Required(ErrorMessage = "Url obrigatória")]
        public virtual string URL { get; set; }

        [Required(ErrorMessage = "Tipo obrigatório")]
        public virtual TipoBanner TipoBanner { get; set; }

        [Required(ErrorMessage = "Data de inicio obrigatória")]
        public virtual DateTime DataInicio { get; set; }
        [Required(ErrorMessage = "Data de fim obrigatória")]
        public virtual DateTime DataFim { get; set; }
    }
}
