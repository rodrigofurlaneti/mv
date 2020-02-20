using System;
using System.Collections.Generic;
using System.Linq;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IAgendamentoServico : IBaseServico<Agendamento>
    {
        IList<Agendamento> BuscarAgendamentosDisponiveis(int idLoja, int inicio, int quantidade);
        IList<Agendamento> BuscarAgendamentosDisponiveis(int idLoja, int dia, int mes, int ano);
        IList<Agendamento> BuscarAgendamentos(int idLoja, int dia, int mes, int ano);
    }

    public class AgendamentoServico : BaseServico<Agendamento, IAgendamentoRepositorio>, IAgendamentoServico
    {
        public IList<Agendamento> BuscarAgendamentosDisponiveis(int idLoja, int inicio, int quantidade)
        {
            var agendamentos =  BuscarPor(x => x.Data > DateTime.Now && x.Loja.Id == idLoja).Skip(inicio).Take(quantidade);

            var agendamentoFiltrado = agendamentos.Where(x => x.Disponivel).OrderBy(x => x.Data).ToList();

            if (agendamentoFiltrado.Count > 0)
                return agendamentoFiltrado;
            else
                agendamentos = BuscarPor(x => x.Data > DateTime.Now && x.Loja.Id == idLoja).Skip(quantidade).Take(quantidade);

            return agendamentos.Where(x => x.Disponivel).OrderBy(x => x.Data).ToList();
        }

        public IList<Agendamento> BuscarAgendamentosDisponiveis(int idLoja, int dia, int mes, int ano)
        {
            return BuscarPor(x => x.Data.Day == dia &&
                                    x.Data.Month == mes &&
                                    x.Data.Year == ano &&
                                    x.Loja.Id == idLoja).Where(x => x.Disponivel).ToList();
        }

        public IList<Agendamento> BuscarAgendamentos(int idLoja, int dia, int mes, int ano)
        {
            return BuscarPor(x => x.Data.Day == dia &&
                                    x.Data.Month == mes &&
                                    x.Data.Year == ano &&
                                    x.Loja.Id == idLoja).ToList();
        }
    }
}