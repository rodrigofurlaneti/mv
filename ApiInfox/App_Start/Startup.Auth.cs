using System;
using ApiInfox.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace ApiInfox
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthServerOptions { get; private set; }
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        //static Startup()
        //{
        //    OAuthServerOptions = new OAuthAuthorizationServerOptions
        //    {
        //        TokenEndpointPath = new PathString("/token"),
        //        Provider = new OAuthAppProvider(),
        //        AccessTokenExpireTimeSpan = TimeSpan.FromDays(2),
        //        AllowInsecureHttp = true
        //    };
        //}

        //public void ConfigureAuth(IAppBuilder app)
        //{
        //    app.UseOAuthBearerTokens(OAuthOptions);
        //}

        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable cors for authentication context.  
            // IMPORTANT: Should be the first setting
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Configure the application for OAuth based flow
            PublicClientId = "self";
            OAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new OAuthAppProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(30),
                AllowInsecureHttp = true,
                AccessTokenProvider = new PersistAccessTokenProvider()
                //                RefreshTokenProvider = new SimpleRefreshTokenProvider() // Implementar refresh tokens
            };

            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
    }

    public class PersistAccessTokenProvider : AuthenticationTokenProvider
    {
        public override void Create(AuthenticationTokenCreateContext context)
        {
            if (context.Ticket.Properties.IsPersistent)
                context.Ticket.Properties.ExpiresUtc = DateTime.UtcNow.AddYears(1);
            base.Create(context);
        }
    }
}