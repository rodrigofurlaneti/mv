using System;
using System.Net;

namespace Core.Exceptions
{
    public class ApiMobisegException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public ApiMobisegException(HttpStatusCode status, string message) : base(message)
        {
            StatusCode = status;
        }
    }
}