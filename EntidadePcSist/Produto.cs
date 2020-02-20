using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EntidadePcSist.Base;

namespace EntidadePcSist
{
    public class Produto : IEntityPcSist
    {
        public Produto()
        {
            Informacoes = new List<InformacaoProduto>();
        }

        [Required, StringLength(150)]
        public virtual string Codigo { get; set; }
        [Required, StringLength(2000)]
        public virtual string Nome { get; set; }
        [Required, StringLength(2000)]
        public virtual string Descricao { get; set; }
        
        public virtual string CodigoBarras { get; set; }

        public virtual IList<InformacaoProduto> Informacoes { get; set; }
        
        public virtual int Id
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}