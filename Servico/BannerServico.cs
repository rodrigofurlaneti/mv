using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IBannerServico : IBaseServico<Banner>
    {
        
    }

    public class BannerServico : BaseServico<Banner, IBannerRepositorio>, IBannerServico
    {
    }
}
