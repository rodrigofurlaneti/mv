using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class QrCodeRepositorio : NHibRepository<QrCode>, IQrCodeRepositorio
    {
        public QrCodeRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}