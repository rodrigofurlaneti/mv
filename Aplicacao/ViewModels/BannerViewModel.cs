using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Entidade;
using Entidade.Uteis;

namespace Aplicacao.ViewModels
{
    public class BannerViewModel
    {
        public int Id { get; set; }

        public string URL { get; set; }

        public TipoBanner TipoBanner { get; set; }

        [Display(Name = "Data Inicio")]
        public string DataInicio { get; set; }

        [Display(Name = "Data Fim")]
        public string DataFim { get; set; }


        public BannerViewModel()
        {

        }

        public BannerViewModel(Banner entidade)
        {
            Id = entidade?.Id ?? 0;
            URL = entidade.URL;
            DataInicio = entidade.DataInicio.ToString("dd/MM/yyyy");
            DataFim = entidade.DataFim.ToString("dd/MM/yyyy");
            TipoBanner = entidade.TipoBanner;
        }

        public Banner ToEntity() => new Banner()
        {
            Id = Id,
            URL = this.URL,
            DataInicio = string.IsNullOrEmpty(this.DataInicio) ? DateTime.Now : DateTime.Parse(this.DataInicio),
            DataFim = string.IsNullOrEmpty(this.DataFim) ? DateTime.Now : DateTime.Parse(this.DataFim),
            TipoBanner = this.TipoBanner
        };
    }
}