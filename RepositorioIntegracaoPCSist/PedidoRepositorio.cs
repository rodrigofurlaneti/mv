using Dominio.IRepositorioIntegracaoPCSist;
using EntidadePcSist;
using RepositorioIntegracaoPCSist.Base;

namespace RepositorioIntegracaoPCSist
{
    public class PedidoRepositorio : NHibRepository<Pedido>, IPedidoRepositorioIntegracao
    {
        public PedidoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}