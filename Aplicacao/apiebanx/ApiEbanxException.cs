using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.apiebanx
{
    public class ApiEbanxException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public ApiEbanxException(HttpStatusCode status, string message) : base(message)
        {
            StatusCode = status;
        }
    }
}
