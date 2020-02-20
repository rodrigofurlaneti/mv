using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class DescontoGlobalRepositorio : NHibRepository<DescontoGlobal>, IDescontoGlobalRepositorio
    {
        public DescontoGlobalRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}