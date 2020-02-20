using Core.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Aplicacao.apiebanx
{
    public static class ApiEbanxBase
    {
        internal static readonly string API_EBANX_URL = ConfigurationManager.AppSettings["API_EBANX_URL"];

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
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(API_EBANX_URL.TrimEnd('/') + '/' + url.TrimStart('/'));
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
                        throw new ApiEbanxException(httpResponse.StatusCode, $"Server error (HTTP {httpResponse.StatusCode}: {httpResponse.StatusDescription}).");
                    }

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
            catch (WebException web)
            {
                throw new ApiEbanxException(((HttpWebResponse)web.Response).StatusCode,
                    $"Server error (HTTP {((HttpWebResponse)web.Response).StatusCode}: {((HttpWebResponse)web.Response).StatusDescription}) - (Web Status {web.Status}: {web.Status.ToDescription()}) - Msg ({web.Message}).");
            }
        }
    }
}
