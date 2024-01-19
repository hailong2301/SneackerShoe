using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Product
    {
        public Product()
        {
            Carts = new HashSet<Cart>();
            OrderDetails = new HashSet<OrderDetail>();
            ProductSizes = new HashSet<ProductSize>();
        }

        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public double? Price { get; set; }
        public double? Discount { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public int BrandId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Brand Brand { get; set; } = null!;
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ProductSize> ProductSizes { get; set; }
    }
}
