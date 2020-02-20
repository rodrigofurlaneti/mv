using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace ApiInfox.Models
{
    public class AgendamentoModelView
    {
        public AgendamentoModelView(Agendamento agendamento)
        {
            if (agendamento == null)
                throw new Exception("O agendamento não foi encontrado.");

            Data = agendamento.Data;
            Disponivel = agendamento.Disponivel;
            Id = agendamento.Id;
            Loja = new LojaModelView(agendamento.Loja);
        }

        public int Id { get; set; }

        public DateTime Data { get; set; }

        public string Hora { get { return Data.ToShortTimeString(); } }

        public bool Disponivel { get; set; }
        
        public LojaModelView Loja { get; set; }
    }
}