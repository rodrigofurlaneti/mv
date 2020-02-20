using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class PlanoVendaRepositorio : NHibRepository<PlanoVenda>, IPlanoVendaRepositorio
    {
        public PlanoVendaRepositorio(NHibContext context)
            : base(context)
        {
        }
        
    }
}
