using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.apiebanx.Models
{
    public class RefundOrCancelRequestModel : RequestBaseModel
    {
        public string hash { get; set; }
        public string merchant_payment_code { get; set; }
        public string description { get; set; }
        public string merchant_refund_code { get; set; }
    }
}
