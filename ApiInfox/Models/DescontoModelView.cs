using Entidade;
using Entidade.Uteis;
using System;

namespace ApiInfox.Models
{
    public class DescontoModelView
    {
        public DescontoModelView()
        {
            
        }

        public DescontoModelView(Desconto desconto)
        {
            Id = desconto.Id;
            TipoDesconto = desconto.TipoDesconto;
            ValorDesconto = desconto.ValorDesconto;
            Validade = desconto.Validade;
            LimiteDeCompra = desconto.LimiteDeCompra;
        }

        public int Id { get; set; }
        public TipoDesconto TipoDesconto { get; set; }
        public decimal ValorDesconto { get; set; }
        public DateTime Validade { get; set; }
        public virtual int LimiteDeCompra { get; set; }
    }
}