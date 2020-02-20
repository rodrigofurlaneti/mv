using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using ApiInfox.Extensions;
using Entidade;

namespace ApiInfox.Helpers
{
    public class LoggedUser
    {
        public static Usuario GetLoggedUser()
        {
            var identity = Thread.CurrentPrincipal.Identity as ClaimsIdentity;
            if (string.IsNullOrWhiteSpace(identity?.UserId()))
                throw new Exception("Nenhum usuário logado no sistema");
            //
            return new Usuario
            {
                Id = Convert.ToInt32(identity.UserId()),
                Pessoa =
                    new Pessoa
                    {
                        Id = Convert.ToInt32(identity.FindFirst("PessoaId")?.Value),
                        Nome = identity.FindFirst(ClaimTypes.Name)?.Value
                    },
                Perfils = new List<UsuarioPerfil> { new UsuarioPerfil { Perfil = new Perfil { Id = Convert.ToInt32(identity.FindFirst("PerfilId")?.Value) } } }
            };
        }
    }
}