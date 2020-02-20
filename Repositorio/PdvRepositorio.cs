using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class PdvRepositorio : NHibRepository<Pdv>, IPdvRepositorio
    {
        public PdvRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}
