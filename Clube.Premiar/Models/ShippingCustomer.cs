using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clube.Premiar.Models
{
    public partial class ShippingCustomer
    {
        public Person Customer { get; set; }
        public Address ShippingAddress { get; set; }
    }
}
