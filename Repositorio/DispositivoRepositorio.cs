using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class DispositivoRepositorio : NHibRepository<Dispositivo>, IDispositivoRepositorio
    {
        public DispositivoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}