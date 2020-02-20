using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class AvaliacaoPedidoRepositorio : NHibRepository<AvaliacaoPedido>, IAvaliacaoPedidoRepositorio
    {
        public AvaliacaoPedidoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}