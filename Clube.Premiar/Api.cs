using Clube.Premiar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Clube.Premiar
{
    public class AuthenticatedApi : Api
    {
        private readonly PremiarClubeSettings premiarSettings;
        private readonly Participant loggedUser;

        public AuthenticatedApi(PremiarClubeSettings premiarSettings, Participant loggedUser = null)
            : base(premiarSettings)
        {
            this.premiarSettings = premiarSettings;
            this.loggedUser = loggedUser;
        }

        protected override OAuth.Manager GetOAuthManager()
        {
            var oauthManager = new OAuth.Manager(PremiarSettings.Authorization.ClientId.ToString(), PremiarSettings.Authorization.ClientSecret);
            oauthManager["subscription-key"] = PremiarSettings.Authorization.SubscriptionKey;
            oauthManager["campaign_id"] = PremiarSettings.Credentials.CampaignId.ToString();
            oauthManager["username"] = loggedUser.username;
            oauthManager["password"] = loggedUser.password;
            return oauthManager;
        }

        protected override OAuth.OAuthResponse GetOAuthToken()
        {
            var oauthManager = GetOAuthManager();
            var oauth = oauthManager.AcquireRequestToken(uri: PremiarSettings.Api.BaseUri + "access-token", method: "POST");
            AccessToken = oauth["access_token"];
            RawToken = oauth.AllText;
            return oauth;
        }
    }

    public class Api
    {
        protected PremiarClubeSettings PremiarSettings { get; }

        public Api(PremiarClubeSettings premiarSettings)
        {
            PremiarSettings = premiarSettings;
        }

        public string AccessToken { get; protected set; }

        public string RawToken { get; protected set; }

        protected void TryRequest(Action action)
        {
            try
            {
                action();
            }
            catch (WebException ex)
            {
                using (var rStream = new System.IO.StreamReader(ex.Response.GetResponseStream()))
                {
                    var json = rStream.ReadToEnd();
                    if (json.Contains("errors"))
                    {
                        var exObject = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(json, new { errors = new[] { new { code = 0, message = "" } } });
                        throw new PremiarClubeException((ex.Response as HttpWebResponse)?.StatusCode, exObject.errors.Select(a => Tuple.Create(a.code, a.message)).ToArray());
                    }
                    else if (json.Contains("activityId"))
                    {
                        var exObject = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(json, new { statusCode = 0, message = "", activityId = "" });
                        throw new PremiarClubeException((HttpStatusCode)exObject.statusCode, exObject.message, Guid.Parse(exObject.activityId));
                    }
                    else
                    {
                        var exObject = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(json, new { error = "", error_description = "" });
                        throw new PremiarClubeException((ex.Response as HttpWebResponse)?.StatusCode, exObject.error, exObject.error_description);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        protected void TryAuthenticatedRequest(Action<WebClient> action)
        {
            TryRequest(() => {
                var webClient = GetAuthenticatedWebClient();
                action(webClient);
            });
        }

        protected virtual OAuth.Manager GetOAuthManager()
        {
            var oauthManager = new OAuth.Manager(PremiarSettings.Authorization.ClientId.ToString(), PremiarSettings.Authorization.ClientSecret);
            oauthManager["subscription-key"] = PremiarSettings.Authorization.SubscriptionKey;
            oauthManager["campaign_id"] = PremiarSettings.Credentials.CampaignId.ToString();
            return oauthManager;
        }

        protected virtual OAuth.OAuthResponse GetOAuthToken()
        {
            var oauthManager = GetOAuthManager();
            var oauth = oauthManager.AcquireAccessToken(uri: PremiarSettings.Api.BaseUri + "access-token", method: "POST");
            AccessToken = oauth["access_token"];
            RawToken = oauth.AllText;
            return oauth;
        }

        protected WebClient GetDefaultWebClient()
        {
            var webClient = new WebClient();
            webClient.BaseAddress = PremiarSettings.Api.BaseUri;
            webClient.Headers["Ocp-Apim-Subscription-Key"] = PremiarSettings.Authorization.SubscriptionKey;
            return webClient;
        }

        protected WebClient GetAuthenticatedWebClient()
        {
            var oauth = GetOAuthToken();
            var webClient = GetDefaultWebClient();
            webClient.Headers[HttpRequestHeader.Authorization] = $"{oauth["token_type"]} {oauth["access_token"]}";
            return webClient;
        }

        public T Get<T>(string uri)
        {
            T response = default(T);

            TryAuthenticatedRequest((webClient) =>
            {
                var jsonResponse = webClient.DownloadString(uri);
                response = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonResponse);
            });

            return response;
        }

        public void Post(string uri)
        {
            TryAuthenticatedRequest((webClient) =>
            {
                var jsonResponse = webClient.UploadString(uri, "POST", null);
            });
        }

        public T Post<T>(string uri)
        {
            T response = default(T);

            TryAuthenticatedRequest((webClient) =>
            {
                var jsonResponse = webClient.UploadString(uri, "POST", null);
                response = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonResponse);
            });

            return response;
        }

        public T Post<T, I>(string uri, I input)
        {
            T response = default(T);

            TryAuthenticatedRequest((webClient) =>
            {
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

                var jsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(input);

                var jsonResponse = webClient.UploadString(uri, "POST", jsonRequest);
                response = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonResponse);
            });

            return response;
        }

        public T Put<T, I>(string uri, I input)
        {
            T response = default(T);

            TryAuthenticatedRequest((webClient) =>
            {
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

                var jsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(input);

                var jsonResponse = webClient.UploadString(uri, "PUT", jsonRequest);
                response = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonResponse);
            });

            return response;
        }

        public T Delete<T>(string uri)
        {
            T response = default(T);

            TryAuthenticatedRequest((webClient) =>
            {
                var jsonResponse = webClient.UploadString(uri, "DELETE", null);
                response = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonResponse);
            });

            return response;
        }
    }
}
