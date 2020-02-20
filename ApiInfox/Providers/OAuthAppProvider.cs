using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dominio;
using Entidade;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.ServiceLocation;

namespace ApiInfox.Providers
{
    public class OAuthAppProvider : OAuthAuthorizationServerProvider
    {
        #region Private Members
        private readonly string _publicClientId;

        private static bool IsPersistent(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var form = context.Request.ReadFormAsync().Result;
            return form["IsPersistent"] != null &&
                   (form["IsPersistent"].Equals("on", StringComparison.InvariantCultureIgnoreCase) ||
                    form["IsPersistent"].Equals("true", StringComparison.InvariantCultureIgnoreCase));
        }
        #endregion

        public OAuthAppProvider(string publicClientId)
        {
            if (publicClientId == null)
                throw new ArgumentNullException(nameof(publicClientId));

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                var username = context.UserName;
                var password = context.Password;
                var usuarioServ = ServiceLocator.Current.GetInstance<IUsuarioServico>();

                //TODO: Execute Once and then remove
                //var cartServ = ServiceLocator.Current.GetInstance<ICartaoServico>();
                //cartServ.EncryptAllCreditCard();

                usuarioServ.EncryptAllPasswords();
                var usuario = usuarioServ.ValidarLogin(username, password);

                if (usuario == null)
                {
                    context.SetError("invalid_grant", "Usuário ou senha incorretos.");
                    return;
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Pessoa.Nome),
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                };

                // Adiciona aos claims as permissões do usuário
                claims.AddRange(usuario.Perfils.SelectMany(x => x.Perfil.Permissoes).Select(x => new Claim(ClaimTypes.Role, x.Regra)));

                var oAuthIdentity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                var cookiesIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationType);
                var properties = CreateProperties(usuario, null);
                properties.IsPersistent = IsPersistent(context);
                var ticket = new AuthenticationTicket(oAuthIdentity, properties);
                context.Validated(ticket);
                context.Request.Context.Authentication.SignIn(cookiesIdentity);
            }
            catch (Exception ex)
            {
                context.SetError("internal_error", ex.Message);
                throw;
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.ClientId == null)
            {
                context.Validated();
            }
            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                var expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// This method returns the properties that will be returned in the response token request
        /// </summary>
        public static AuthenticationProperties CreateProperties(Usuario usuario, string clientId)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "UsuarioId", usuario.Id.ToString() },
                { "UsuarioNome", usuario.Pessoa.Nome },
                { "PerfilId", usuario.Perfil.ToString() },
                { "PessoaId", usuario.Pessoa.Id.ToString() },
                { "PrimeiroLogin", usuario.PrimeiroLogin.ToString() }
            };
            return new AuthenticationProperties(data);
        }
    }
}