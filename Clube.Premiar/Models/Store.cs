using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clube.Premiar.Models
{
    public class Store
    {
        public int id { get; set; }
        public string name { get; set; }
        public string logoUrl { get; set; }
        public object imageBanner { get; set; }
        public int priority { get; set; }
        public int vendorId { get; set; }
        public string type { get; set; }
        public string slug { get; set; }
    }
}
