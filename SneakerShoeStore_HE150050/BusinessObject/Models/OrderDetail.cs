using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public DateTime CreateDate { get; set; }
        public double Amount { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
        public virtual Size Size { get; set; } = null!;
    }
}
