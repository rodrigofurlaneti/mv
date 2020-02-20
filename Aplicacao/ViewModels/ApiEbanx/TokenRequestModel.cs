using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.apiebanx.Models
{
    public class TokenRequestModel : RequestBaseModel
    {
        public string public_integration_key { get; set; }
        public string payment_type_code { get; set; }
        public string country { get; set; }
        public CreditCardModel creditcard { get; set; }
    }
}
