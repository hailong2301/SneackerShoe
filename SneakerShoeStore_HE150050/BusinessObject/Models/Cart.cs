using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Cart
    {
        public int RecordId { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public int Quantity { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual Size Size { get; set; } = null!;
    }
}
