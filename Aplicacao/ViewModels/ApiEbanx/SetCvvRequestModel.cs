using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.apiebanx.Models
{
    public class SetCvvRequestModel
    {
        public string public_integration_key { get; set; }
        public string token { get; set; }
        public string card_cvv { get; set; }
    }
}
