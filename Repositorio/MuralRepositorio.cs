using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class MuralRepositorio : NHibRepository<Mural>, IMuralRepositorio
    {
        public MuralRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}