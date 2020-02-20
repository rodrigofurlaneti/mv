using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.apiebanx.Models
{
    public class PaymentItemModel
    {
        public string sku { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal unit_price { get; set; }
        public int quantity { get; set; }
        public string type { get; set; }
    }
}
