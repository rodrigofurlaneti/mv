using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clube.Premiar.Models
{
    public partial class Product
    {
        public string Id { get; set; }
        public long OriginalProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Sku { get; set; }
        public OriginalId OriginalId { get; set; }
        public Uri ImageUrl { get; set; }
        public Category Category { get; set; }
        public Brand Brand { get; set; }
        public Section[] Sections { get; set; }
        public Skus[] Skus { get; set; }
        public Feature[] Features { get; set; }
        public Vendor Vendor { get; set; }
    }

    public partial struct OriginalId
    {
        public long? Integer;
        public Guid? Uuid;

        public static implicit operator OriginalId(long Integer) => new OriginalId { Integer = Integer };
        public static implicit operator OriginalId(Guid Uuid) => new OriginalId { Uuid = Uuid };
    }

    public partial class Feature
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
    }

    public partial class Section
    {
        public long Id { get; set; }
        public string SectionType { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
    }

    public partial class Skus
    {
        public string Sku { get; set; }
        public long OriginalId { get; set; }
        public string Status { get; set; }
        public string Ean { get; set; }
        public Image[] Images { get; set; }
        public object[] SkuFeatures { get; set; }
    }
}