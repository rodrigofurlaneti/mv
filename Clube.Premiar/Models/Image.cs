using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clube.Premiar.Models
{
    public partial class Image
    {
        public Uri SmallImage { get; set; }
        public Uri MediumImage { get; set; }
        public string LargeImage { get; set; }
        public long Order { get; set; }
    }
}
