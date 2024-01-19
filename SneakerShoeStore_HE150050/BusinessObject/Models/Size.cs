using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Size
    {
        public Size()
        {
            Carts = new HashSet<Cart>();
            OrderDetails = new HashSet<OrderDetail>();
            ProductSizes = new HashSet<ProductSize>();
        }

        public int SizeId { get; set; }
        public int? SizeNumber { get; set; }

        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ProductSize> ProductSizes { get; set; }
    }
}
