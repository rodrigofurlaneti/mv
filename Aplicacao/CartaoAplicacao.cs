using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface ICartaoAplicacao : IBaseAplicacao<Cartao>
    {
    }

    public class CartaoAplicacao : BaseAplicacao<Cartao, ICartaoServico>, ICartaoAplicacao
    {
    }
}