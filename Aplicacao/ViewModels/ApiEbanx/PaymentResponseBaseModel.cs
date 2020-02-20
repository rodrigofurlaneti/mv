using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.apiebanx.Models
{
    public abstract class PaymentResponseBaseModel : ResponseBaseModel, IStatusResponse
    {
        public PaymentMadeModel payment { get; set; }

        public string Code
        {
            get
            {
                return payment?.transaction_status?.code ?? (string.IsNullOrEmpty(status_code) ? status : status_code);
            }
        }

        public string Message
        {
            get
            {
                return payment?.transaction_status?.description ?? (string.IsNullOrEmpty(status_message) ? status : status_message);
            }
        }
    }
}