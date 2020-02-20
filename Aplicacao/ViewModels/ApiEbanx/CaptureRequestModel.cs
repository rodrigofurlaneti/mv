using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.apiebanx.Models
{
    public class CaptureRequestModel : RequestBaseModel
    {
        public string hash { get; set; }
        public string merchant_payment_code { get; set; }
        public string merchant_capture_code { get; set; }
        public decimal amount { get; set; }
    }
}
