using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class PedidoVoucherRepositorio : NHibRepository<PedidoVoucher>, IPedidoVoucherRepositorio
    {
        public PedidoVoucherRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}