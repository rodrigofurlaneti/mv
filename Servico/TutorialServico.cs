using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ITutorialServico : IBaseServico<Tutorial>
    {
        
    }

    public class TutorialServico : BaseServico<Tutorial, ITutorialRepositorio>, ITutorialServico
    {
    }
}
