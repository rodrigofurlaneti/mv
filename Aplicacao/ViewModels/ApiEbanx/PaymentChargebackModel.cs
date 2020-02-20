using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.apiebanx.Models
{
    public class PaymentChargebackModel
    {
        public DateTime? create_date { get; set; }
        public DateTime? chargeback_date { get; set; }
        public string description { get; set; }
    }
}
