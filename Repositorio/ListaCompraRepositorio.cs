using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ListaCompraRepositorio : NHibRepository<ListaCompra>, IListaCompraRepositorio
    {
        public ListaCompraRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}
