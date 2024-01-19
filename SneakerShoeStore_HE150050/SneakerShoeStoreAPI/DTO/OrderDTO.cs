namespace SneakerShoeStoreAPI.DTO
{
    public class OrderDTO
    {
        public int UserId { get; set; }
        public int? Quantity { get; set; }
        public double? TotalPrice { get; set; }
        public DateTime? OrderDate { get; set; }
    }
}
