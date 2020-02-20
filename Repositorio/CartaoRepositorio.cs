using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class CartaoRepositorio : NHibRepository<Cartao>, ICartaoRepositorio
    {
        public CartaoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}