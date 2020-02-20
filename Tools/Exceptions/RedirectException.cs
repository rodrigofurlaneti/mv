using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class RedirectException : System.Web.HttpException
    {
        public RedirectException()
        {
        }

        public RedirectException(string uri)
            : base((int)HttpStatusCode.Moved, uri)
        {
        }
        public RedirectException(int httpCode, string uri)
            : base(httpCode, uri)
        {
        }
    }
}
