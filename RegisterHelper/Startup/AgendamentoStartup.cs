using System;
using Microsoft.Practices.ServiceLocation;
using System.Collections.Generic;
using Aplicacao;
using Entidade;

namespace InitializerHelper.Startup
{
    public static class AgendamentoStartup
    {
        #region Private Members
        private static void AdicionaAgendamento()
        {
            //agendamentos
            //var agendamentoAplicacao = ServiceLocator.Current.GetInstance<IAgendamentoAplicacao>();
            //if (agendamentoAplicacao.PrimeiroPor(x => x.Disponivel.Equals(true)) == null)
            //{
            //    var agendamentos = new List<Agendamento>
            //    {
            //        new Agendamento { Data = DateTime.Now.AddMonths(1), Disponivel = true },
            //        new Agendamento { Data = DateTime.Now.AddMonths(1).AddHours(1), Disponivel = true },
            //        new Agendamento { Data = DateTime.Now.AddMonths(1).AddHours(2), Disponivel = true },
            //        new Agendamento { Data = DateTime.Now.AddMonths(1).AddHours(3), Disponivel = true },
            //        new Agendamento { Data = DateTime.Now.AddMonths(1).AddHours(4), Disponivel = true },
            //        new Agendamento { Data = DateTime.Now.AddMonths(1).AddHours(5), Disponivel = true },
            //        new Agendamento { Data = DateTime.Now.AddMonths(1).AddHours(6), Disponivel = true },
            //    };
            //    agendamentos.ForEach(a => { agendamentoAplicacao.Salvar(a); });
            //}
        }
        #endregion

        #region Public Members

        public static void Start()
        {
            AdicionaAgendamento();
        }

        #endregion
    }
}