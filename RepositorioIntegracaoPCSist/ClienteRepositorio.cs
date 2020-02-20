using Dominio.IRepositorioIntegracaoPCSist;
using EntidadePcSist;
using RepositorioIntegracaoPCSist.Base;

namespace RepositorioIntegracaoPCSist
{
    public class ClienteRepositorio : NHibRepository<Cliente>, IClienteRepositorioIntegracao
    {
        public ClienteRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}
