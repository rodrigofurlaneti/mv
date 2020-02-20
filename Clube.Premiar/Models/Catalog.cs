using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clube.Premiar.Models
{
    public class Catalog
    {
        public int id { get; set; }
        public string name { get; set; }
        public string redeemType { get; set; }
        public double minimumPercentagePoints { get; set; }
        public CurrencyConfiguration currencyConfiguration { get; set; }
        public int maximumInstallments { get; set; }
        public double markupForPurchasePoints { get; set; }
        public bool onlyCash { get; set; }
    }

    public class CurrencyConfiguration
    {
        public string pointsName { get; set; }
        public double conversionRate { get; set; }
        public int decimalPlaces { get; set; }
    }
}
