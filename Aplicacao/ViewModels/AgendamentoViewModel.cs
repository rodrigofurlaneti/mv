using System;
using System.ComponentModel.DataAnnotations;
using Entidade;

namespace Aplicacao.ViewModels
{
    public class AgendamentoViewModel
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public bool Disponivel { get; set; }

        [Display(Name="Loja")]
        public LojaViewModel Loja { get; set; }

        public AgendamentoViewModel()
        {
            Data = DateTime.Now;
        }

        public AgendamentoViewModel(Agendamento entidade)
        {
            Id = entidade?.Id ?? 0;
            Data = entidade.Data;
            Loja = AutoMapper.Mapper.Map<Loja, LojaViewModel>(entidade?.Loja);
        }

        public Agendamento ToEntity() => new Agendamento()
        {
            Id = Id,
            Data = Data,
            Loja = AutoMapper.Mapper.Map<LojaViewModel, Loja>(Loja)
        };
    }
}