using System;
using Entidade;

namespace Aplicacao.ViewModels
{
    public class EstadoViewModel
    {
        public int Id { get; private set; }
        public DateTime DataInsercao { get; }
        public string Descricao { get; set; }
        public string Sigla { get; set; }
        public PaisViewModel Pais { get; set; }

        public EstadoViewModel()
        {
        }

        public EstadoViewModel(Estado estado)
        {
            Id = estado?.Id ?? 0;
            DataInsercao = estado?.DataInsercao ?? DateTime.Now;
            Descricao = estado?.Descricao;
            Sigla = estado?.Sigla;
            Pais = new PaisViewModel(estado?.Pais);
        }
    }
}
