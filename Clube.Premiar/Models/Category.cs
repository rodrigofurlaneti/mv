using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clube.Premiar.Models
{
    public partial class Category
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public long Quantity { get; set; }
        public Category[] Subcategories { get; set; }
    }
}
