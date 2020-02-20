using System.Collections.Generic;
using System.Linq;
using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IUsuarioAplicacao : IBaseAplicacao<Usuario>
    {
        Usuario ValidarLogin(string usuario, string senha);
        Usuario RetornarPorUsuario(string usuario);
        void RecuperarSenha(string usuario, string template);
        void TrocarSenha(string usuario, string senha, string senhaConfirmacao);
        void PrimeiroLoginRealizado(string usuario);
        List<Perfil> BuscarPerfils();
    }

    public class UsuarioAplicacao : BaseAplicacao<Usuario, IUsuarioServico>, IUsuarioAplicacao
    {
        private readonly IPerfilAplicacao _perfilAplicacao;
        private readonly IUsuarioServico _usuarioServico;

        public UsuarioAplicacao(IPerfilAplicacao perfilAplicacao, IUsuarioServico usuarioServico)
        {
            _perfilAplicacao = perfilAplicacao;
            _usuarioServico = usuarioServico;
        }

        public Usuario ValidarLogin(string usuario, string senha)
        {
            return _usuarioServico.ValidarLogin(usuario, senha);
        }

        public void RecuperarSenha(string usuario, string template)
        {
            _usuarioServico.RecuperarSenha(usuario, template);
        }

        public void TrocarSenha(string usuario, string senha, string senhaConfirmacao)
        {
            _usuarioServico.TrocarSenha(usuario, senha, senhaConfirmacao);
        }

        public void PrimeiroLoginRealizado(string usuario)
        {
            _usuarioServico.PrimeiroLoginRealizado(usuario);
        }

        public List<Perfil> BuscarPerfils()
        {
            return _perfilAplicacao.Buscar().ToList();
        }

        public new void Salvar(Usuario entity)
        {
            Servico.Salvar(entity);
        }

        public Usuario RetornarPorUsuario(string usuario)
        {
            return _usuarioServico.RetornarPorUsuario(usuario);
        }
    }
}