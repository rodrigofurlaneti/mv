using Dominio.IRepositorio.Base;
using Entidade;

namespace Dominio.IRepositorio
{
    public interface IUsuarioRepositorio : IRepository<Usuario>
    {
        Usuario ValidarLogin(string usuario, string senha, string facebookId);
        Usuario RetornarPorUsuario(string usuario);
    }
}
