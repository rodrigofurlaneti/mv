using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class AgendamentoRepositorio : NHibRepository<Agendamento>, IAgendamentoRepositorio
    {
        public AgendamentoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}