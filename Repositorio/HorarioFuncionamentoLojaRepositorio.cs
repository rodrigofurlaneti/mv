using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class HorarioFuncionamentoLojaRepositorio : NHibRepository<HorarioFuncionamentoLoja>, IHorarioFuncionamentoLojaRepositorio
    {
        public HorarioFuncionamentoLojaRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}
