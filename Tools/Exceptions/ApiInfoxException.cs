using System;
using System.Net;

namespace Core.Exceptions
{
    public class ApiInfoxException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public ApiInfoxException(HttpStatusCode status, string message) : base(message)
        {
            StatusCode = status;
        }
    }
}