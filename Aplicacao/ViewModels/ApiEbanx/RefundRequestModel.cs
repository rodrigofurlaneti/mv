using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.apiebanx.Models
{
    public class RefundRequestModel : RequestBaseModel
    {
        public string operation { get; set; }
        public string hash { get; set; }
        public string merchant_payment_code { get; set; }
        public decimal amount { get; set; }
        public string description { get; set; }
        public string merchant_refund_code { get; set; }
        public int refund_id { get; set; }
    }
}
