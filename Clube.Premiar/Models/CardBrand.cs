using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clube.Premiar.Models
{
    public class CardBrand
    {
        public int id { get; set; }
        public string name { get; set; }
        public string imageUrl { get; set; }
        public string regexValidation { get; set; }
        public string paymentCode { get; set; }
    }
}
