using Aplicacao.Base;
using Aplicacao.ViewModels;
using Dominio;
using Entidade;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao
{
    public interface IAgendamentoAplicacao : IBaseAplicacao<Agendamento>
    {
        IEnumerable<AgendamentoViewModel> Listar();
    }

    public class AgendamentoAplicacao : BaseAplicacao<Agendamento, IAgendamentoServico>, IAgendamentoAplicacao
    {
        public IEnumerable<AgendamentoViewModel> Listar()
        {
            return Servico.Buscar().Select(x => new AgendamentoViewModel(x)).ToList();
        }
    }
}