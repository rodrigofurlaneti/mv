using System;
using System.ComponentModel.DataAnnotations;
using Entidade;

namespace Aplicacao.ViewModels
{
    public class TutorialViewModel
    {
        public int Id { get; set; }
        public string Url { get; set; }

        public DateTime DataInsercao { get; set; }

        public TutorialViewModel()
        {

        }

        public TutorialViewModel(Tutorial entidade)
        {
            Id = entidade?.Id ?? 0;
            Url = entidade?.URL;
            DataInsercao = entidade?.DataInsercao ?? DateTime.Now;
        }

        public Tutorial ToEntity() => new Tutorial()
        {
            Id = this.Id,
            URL= this.Url,
            DataInsercao = this.DataInsercao
        };
    }
}