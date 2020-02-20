using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Entidade;
using Entidade.Uteis;

namespace Aplicacao.ViewModels
{
    public class CupomViewModel
    {
        public int Id { get; set; }

        public string CodigoCupom { get; set; }

        public string Descricao { get; set; }

        public TipoDesconto TipoDesconto { get; set; }

        [Display(Name = "Validade Final")]
        public string ValidadeFinal { get; set; }

        public string ValorCupom { get; set; }

        public DateTime DataInsercao { get; set; }


        public CupomViewModel()
        {

        }

        public CupomViewModel(Cupom entidade)
        {
            Id = entidade?.Id ?? 0;
            CodigoCupom = entidade.CodigoCupom;
            Descricao = entidade.Descricao;
            TipoDesconto = entidade.TipoDesconto;
            ValidadeFinal = entidade.ValidadeFinal.ToString("dd/MM/yyyy");
            ValorCupom = entidade.ValorCupom.ToString("0.00");
        }

        public Cupom ToEntity() => new Cupom()
        {
            Id = Id,
            DataInsercao = this.DataInsercao,
            CodigoCupom = this.CodigoCupom,
            Descricao = this.Descricao,
            ValidadeFinal = string.IsNullOrEmpty(this.ValidadeFinal) ? DateTime.Now : DateTime.Parse(this.ValidadeFinal),
            ValorCupom = Convert.ToDecimal(this.ValorCupom.Replace(",", ".")),
            TipoDesconto = this.TipoDesconto
        };
    }
}