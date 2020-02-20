using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ICidadeServico : IBaseServico<Cidade>
    {
    }

    public class CidadeServico : BaseServico<Cidade, ICidadeRepositorio>, ICidadeServico
    {
    }
}