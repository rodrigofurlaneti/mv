using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class TabloideRepositorio : NHibRepository<Tabloide>, ITabloideRepositorio
    {
        public TabloideRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}
