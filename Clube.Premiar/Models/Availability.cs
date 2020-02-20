using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clube.Premiar.Models
{
    public partial class Availability
    {
        public bool Available { get; set; }
        public decimal RegularPrice { get; set; }
        public decimal Price { get; set; }
        public PointsCash PointsCash { get; set; }
        public object Cash { get; set; }
    }

    public partial class PointsCash
    {
        public decimal PartialPoints { get; set; }
        public object[] Installments { get; set; }
    }
}
