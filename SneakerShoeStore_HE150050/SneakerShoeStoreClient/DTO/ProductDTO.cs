namespace SneakerShoeStoreAPI.DTO
{
    public class ProductDTO
    {
        public string? ProductName { get; set; }
        public double? Price { get; set; }
        public double? Discount { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public int BrandId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
