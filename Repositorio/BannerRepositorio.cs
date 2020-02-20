using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class BannerRepositorio : NHibRepository<Banner>, IBannerRepositorio
    {
        public BannerRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}
