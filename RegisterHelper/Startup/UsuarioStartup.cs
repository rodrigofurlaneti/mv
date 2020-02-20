using System;
using System.Collections.Generic;
using System.Linq;
using Aplicacao;
using Entidade;
using Microsoft.Practices.ServiceLocation;

namespace InitializerHelper.Startup
{
    public static class UsuarioStartup
    {
        #region Private Members
        #endregion

        #region Public Members

        public static void Start()
        {
            AdicionaPerfilRoot();
        }

        private static void AdicionaPerfilRoot()
        {
            var usuarioAplicacao = ServiceLocator.Current.GetInstance<IUsuarioAplicacao>();
            var usuarioRoot = usuarioAplicacao.Buscar().FirstOrDefault();
            if (usuarioRoot != null)
                return;

            var perfilAplicacao = ServiceLocator.Current.GetInstance<IPerfilAplicacao>();
            var perfilRoot = perfilAplicacao.PrimeiroPor(x => x.Nome.Equals("Root"));

            var pessoaAplicacao = ServiceLocator.Current.GetInstance<IPessoaAplicacao>();
            var pessoaRoot = pessoaAplicacao.PrimeiroPor(x => x.Nome.Equals("Carlos Fernandes Lima"));

            var usuario = new Usuario
            {
                Login = "teste@4world.com.br",
                Senha = "JhttRHcxNmM=",
                Pessoa = pessoaRoot,
                UltimoAcesso = DateTime.MinValue
            };

            usuario.Perfil = (int)Entidade.Uteis.PerfilApp.Root;
            usuario.Perfils = new List<UsuarioPerfil> { new UsuarioPerfil { Perfil = perfilRoot, Usuario = usuario } };
            usuarioAplicacao.Salvar(usuario);
        }

        #endregion
    }
}