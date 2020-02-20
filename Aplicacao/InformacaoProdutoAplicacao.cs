using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IInformacaoProdutoAplicacao : IBaseAplicacao<InformacaoProduto>
    {
    }

    public class InformacaoProdutoAplicacao : BaseAplicacao<InformacaoProduto, IInformacaoProdutoServico>, IInformacaoProdutoAplicacao
    {
    }
}