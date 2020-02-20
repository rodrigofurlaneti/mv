using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;
using System;
using System.Linq;

namespace Repositorio
{
    public class UsuarioRepositorio : NHibRepository<Usuario>, IUsuarioRepositorio
    {
        public UsuarioRepositorio(NHibContext context)
            : base(context)
        {
        }

        public Usuario RetornarPorUsuario(string usuario)
        {
            return FirstBy(x => x.Login.Replace(".","").Replace("/","").Replace("-","").Trim() == usuario.Replace(".", "").Replace("/", "").Replace("-", "").Trim());
        }

        public Usuario ValidarLogin(string usuario, string senha, string facebookId)
        {
            return FirstBy(x =>
                            x.Login.Replace(".", "").Replace("/", "").Replace("-", "").Trim() == usuario.Replace(".", "").Replace("/", "").Replace("-", "").Trim()
                            && (x.Senha.Equals(senha) || x.FacebookId.Equals(facebookId)));
        }
    }
}
