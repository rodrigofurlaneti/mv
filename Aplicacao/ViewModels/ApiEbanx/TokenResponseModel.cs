using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.apiebanx.Models
{
    public class TokenResponseModel : ResponseBaseModel
    {
        public string payment_type_code { get; set; }
        public string token { get; set; }
        public string masked_card_number { get; set; }
    }
}
