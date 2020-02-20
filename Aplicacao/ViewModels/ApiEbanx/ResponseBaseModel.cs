using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.apiebanx.Models
{
    public interface IStatusResponse
    {
        string Code { get; }
        string Message { get; }
    }

    public class ResponseBaseModel
    {
        public string status { get; set; }
        public string status_code { get; set; }
        public string status_message { get; set; }
    }
}
