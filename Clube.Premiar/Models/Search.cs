using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clube.Premiar.Models
{
    public partial class Search
    {
        public long LowerPrice { get; set; }
        public long HigherPrice { get; set; }
        public long LastPageIndex { get; set; }
        public Product[] Products { get; set; }
        public Vendor[] Vendors { get; set; }
        public Category[] Categories { get; set; }
        public Brand[] Brands { get; set; }
    }
}
