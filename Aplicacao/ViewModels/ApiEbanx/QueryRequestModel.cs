using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.apiebanx.Models
{
    public class QueryRequestModel : RequestBaseModel
    {
        public string hash { get; set; }
        public string merchant_payment_code { get; set; }
    }
}
