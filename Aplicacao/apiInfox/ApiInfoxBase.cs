using System.Configuration;
using System.IO;
using System.Net;
using System.Web.Mvc;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Core.Extensions;

namespace Aplicacao.ApiInfox
{
    public static class ApiInfoxBase
    {
        private static readonly string API_INFOX_URL = ConfigurationManager.AppSettings["API_INFOX_URL_PATH"];
        
        /// <summary>
        /// Request Api - Only JSON
        /// </summary>
        /// <param name="verb"></param>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns>String JSON</returns>
        public static string RequestApi(HttpVerbs verb, string url, string json)
        {
            //Initial Structure
            //var token = BuscarToken();

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = verb.ToString().ToUpper();
            //httpWebRequest.Headers.Add("Authorization", $"Bearer {token.AccessToken}");

            //Param
            if (verb != HttpVerbs.Get)
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    if (!string.IsNullOrEmpty(json))
                        streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }

            try
            {
                using (var response = httpWebRequest.GetResponse())
                {
                    //Response
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    if (httpResponse.StatusCode != HttpStatusCode.OK
                        && httpResponse.StatusCode != HttpStatusCode.Created)
                    {
                        throw new ApiInfoxException(httpResponse.StatusCode, $"Server error (HTTP {httpResponse.StatusCode}: {httpResponse.StatusDescription}).");
                    }

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
            catch (WebException web)
            {
                throw new ApiInfoxException(((HttpWebResponse)web.Response).StatusCode,
                    $"Server error (HTTP {((HttpWebResponse)web.Response).StatusCode}: {((HttpWebResponse)web.Response).StatusDescription}) - (Web Status {web.Status}: {web.Status.ToDescription()}) - Msg ({web.Message}).");
            }
        }

        public static TokenViewModel BuscarToken()
        {
            var token = new TokenViewModel();

            var httpWebRequestToken = (HttpWebRequest)WebRequest.Create(API_INFOX_URL.Replace("api/", "token"));
            httpWebRequestToken.ContentType = "application/x-www-form-urlencoded";
            httpWebRequestToken.Method = "POST";

            var strRequestDetails = string.Format("grant_type=password&username=adm&password=1234");

            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(strRequestDetails);
            httpWebRequestToken.ContentLength = bytes.Length;
            using (Stream outputStream = httpWebRequestToken.GetRequestStream())
            {
                outputStream.Write(bytes, 0, bytes.Length);
            }

            try
            {
                using (var response = httpWebRequestToken.GetResponse())
                {
                    //Response
                    var httpResponse = (HttpWebResponse)httpWebRequestToken.GetResponse();
                    if (httpResponse.StatusCode != HttpStatusCode.OK
                        && httpResponse.StatusCode != HttpStatusCode.Created)
                    {
                        throw new ApiInfoxException(httpResponse.StatusCode, $"Server error (HTTP {httpResponse.StatusCode}: {httpResponse.StatusDescription}).");
                    }

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<TokenViewModel>(streamReader.ReadToEnd());
                    }
                }
            }
            catch (WebException web)
            {
                throw new ApiInfoxException(((HttpWebResponse)web.Response).StatusCode,
                    $"Server error (HTTP {((HttpWebResponse)web.Response).StatusCode}: {((HttpWebResponse)web.Response).StatusDescription}) - (Web Token Status {web.Status}: {web.Status.ToDescription()}) - Msg ({web.Message}).");
            }
        }
    }
}
