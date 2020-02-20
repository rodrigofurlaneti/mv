using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clube.Premiar.Models
{
    public partial class Cart
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public long PriceCash { get; set; }
        public CartItem[] Items { get; set; }
        public object Customer { get; set; }
        public object ShippingAddress { get; set; }
        public DateTimeOffset ExpireDate { get; set; }
        public object Configuration { get; set; }
        public object Coupon { get; set; }
    }

    public partial class CartItem : Product
    {
        public decimal Price { get; set; }
        public decimal RegularPrice { get; set; }
        public long Quantity { get; set; }
        public Points Points { get; set; }
        public object Cash { get; set; }
    }

    public partial class Points
    {
        public decimal SellingPrice { get; set; }
        public decimal DefaultPrice { get; set; }
    }
}
