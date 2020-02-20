using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.apiebanx.Models
{
    public class RefundModel
    {
        public int id { get; set; }
        public string merchant_refund_code { get; set; }
        public string status { get; set; }
        public DateTime? request_date { get; set; }
        public DateTime? pending_date { get; set; }
        public DateTime? confirm_date { get; set; }
        public decimal amount_ext { get; set; }
        public string description { get; set; }
    }
}
