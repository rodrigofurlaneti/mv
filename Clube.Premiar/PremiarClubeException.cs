using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Clube.Premiar
{
    public class PremiarClubeException : Exception
    {
        public HttpStatusCode? StatusCode { get; }
        public int Code { get; set; }
        public string Error { get; }
        public string Error_Description { get; }
        public Guid ActivityId { get; }

        public PremiarClubeException(int code, string message)
            : base(message)
        {
            Code = code;
        }

        public PremiarClubeException(HttpStatusCode? statusCode, string error, string error_description)
            : base(error_description)
        {
            StatusCode = statusCode;
            Error = error;
            Error_Description = error_description;
        }

        public PremiarClubeException(HttpStatusCode? statusCode, string message, Guid activityId)
            : base(message)
        {
            StatusCode = statusCode;
            ActivityId = activityId;
        }
        
        public PremiarClubeException(HttpStatusCode? statusCode, IEnumerable<Tuple<int, string>> errors)
            : base("Um ou mais erros.", new AggregateException(errors.Select(e => new PremiarClubeException(e.Item1, e.Item2))))
        {
            StatusCode = statusCode;
        }
    }
}
