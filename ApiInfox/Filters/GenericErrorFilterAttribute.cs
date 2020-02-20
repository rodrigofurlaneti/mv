using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace ApiInfox.Filters
{
    public class GenericErrorFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(context.Exception.Message),
                ReasonPhrase = "Critical Exception"
            };

            //var message = new
            //{
            //    ExceptionType = "Custom",
            //    context.Exception.Message
            //};

            //context.Response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, message, "application/json");
        }
    }
}