using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.apiebanx.Models
{
    public class TransactionStatusModel
    {
        public string acquirer { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public string authcode { get; set; }
    }
}
