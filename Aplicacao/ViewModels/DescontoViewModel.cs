using System;
using System.Collections.Generic;
using Entidade;
using Entidade.Uteis;

namespace Aplicacao.ViewModels
{
    public class DescontoViewModel
    {
        public int Id { get; set; }
        public TipoDesconto TipoDesconto { get; set; }
        public string ValorDesconto { get; set; }
        public int IdadeDe { get; set; }
        public int IdadeAte { get; set; }
        public Sexo Sexo { get; set; }
        public List<int> Cidades { get; set; }
        public List<int> Estados { get; set; }
        public List<int> ProdutosPreco { get; set; }
        public List<PessoaRetornoFiltroViewModel> Filtro { get; set; }

        public DescontoViewModel()
        {
            
        }

        public DescontoViewModel(Desconto desconto)
        {
            Id = desconto?.Id ?? 0;
            TipoDesconto = desconto?.TipoDesconto?? TipoDesconto.Monetario;
            ValorDesconto = desconto?.ValorDesconto.ToString("0.00");            
        }

        public Desconto ToEntity()
        {
            return new Desconto()
            {
                Id = this.Id,
                TipoDesconto = this.TipoDesconto,
                ValorDesconto = Convert.ToDecimal(this.ValorDesconto.Replace(",", "."))
            };
        }
    }
}