using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IPdvAplicacao : IBaseAplicacao<Pdv>
    {
    }

    public class PdvAplicacao : BaseAplicacao<Pdv, IPdvServico>, IPdvAplicacao
    {
    }
}