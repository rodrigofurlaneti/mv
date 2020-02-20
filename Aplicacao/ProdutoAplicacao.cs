using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IProdutoAplicacao : IBaseAplicacao<Produto>
    {
    }

    public class ProdutoAplicacao : BaseAplicacao<Produto, IProdutoServico>, IProdutoAplicacao
    {
    }
}