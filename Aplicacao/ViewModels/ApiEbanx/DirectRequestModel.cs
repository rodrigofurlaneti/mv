using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.apiebanx.Models
{
    public class DirectRequestModel : RequestBaseModel
    {
        public string operation { get; set; }
        public string mode { get; set; }
        //bypass_boleto_screen
        public PaymentModel payment { get; set; }
    }
}
