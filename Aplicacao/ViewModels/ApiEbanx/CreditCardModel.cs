using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.apiebanx.Models
{
    public class CreditCardModel
    {
        public string card_number { get; set; }
        public string card_name { get; set; }
        public string card_due_date { get; set; }
        public string card_cvv { get; set; }
        public string token { get; set; }
        public bool auto_capture { get; set; }
    }
}
