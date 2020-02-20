using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IInformacaoProdutoServico : IBaseServico<InformacaoProduto>
    {
        
    }

    public class InformacaoProdutoServico : BaseServico<InformacaoProduto, IInformacaoProdutoRepositorio>, IInformacaoProdutoServico
    {
    }
}
