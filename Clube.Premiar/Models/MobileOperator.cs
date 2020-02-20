using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clube.Premiar.Models
{
    public class MobileOperator
    {
        public string mobileOperator { get; set; }
        public string description { get; set; }
    }

    public class AreaCode
    {
        public string areaCode { get; set; }
        public string description { get; set; }
    }

    public class RechargeValue
    {
        public decimal rechargeValue { get; set; }
        public decimal pointsValue { get; set; }
        public string serviceId { get; set; }
    }
}
