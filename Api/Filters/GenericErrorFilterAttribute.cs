using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace Api.Filters
{
    public class GenericErrorFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is Core.Exceptions.RedirectException)
            {
                var exception = context.Exception as Core.Exceptions.RedirectException;
                context.Response = context.Request.CreateResponse((HttpStatusCode)exception.GetHttpCode(), "");
                context.Response.Headers.Location = new Uri(exception.Message);
            }
            else
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(context.Exception);
                context.Request.CreateResponse(HttpStatusCode.InternalServerError, json, "application/json");
            }
        }
    }
}