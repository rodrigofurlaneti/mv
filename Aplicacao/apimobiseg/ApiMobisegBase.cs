using System.IO;
using System.Net;
using System.Web.Mvc;
using Core.Exceptions;
using Core.Extensions;

namespace Aplicacao.apimobiseg
{
    public static class ApiMobisegBase
    {
        //fluxo = ConfigurationManager.AppSettings["API_MOBISEG_FLUXO"],
        //chave = ConfigurationManager.AppSettings["API_MOBISEG_CHAVE"],
        //plano = ConfigurationManager.AppSettings["API_MOBISEG_PLANO"]

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
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = verb.ToString().ToUpper();

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
                        throw new ApiMobisegException(httpResponse.StatusCode, $"Server error (HTTP {httpResponse.StatusCode}: {httpResponse.StatusDescription}).");
                    }

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
            catch (WebException web)
            {
                throw new ApiMobisegException(((HttpWebResponse)web.Response).StatusCode,
                    $"Server error (HTTP {((HttpWebResponse)web.Response).StatusCode}: {((HttpWebResponse)web.Response).StatusDescription}) - (Web Status {web.Status}: {web.Status.ToDescription()}) - Msg ({web.Message}).");
            }
        }
    }
}
