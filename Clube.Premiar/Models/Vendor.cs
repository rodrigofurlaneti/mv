using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clube.Premiar.Models
{
    public partial class Vendor
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Uri LogoUrl { get; set; }
        public VendorType Type { get; set; }
        public string Slug { get; set; }
    }

    public enum VendorType { Ecommerce, Externalvouchervirtual };
}
