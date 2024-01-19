namespace SneakerShoeStoreAPI.DTO
{
    public class OrderDetailDTO
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public DateTime CreateDate { get; set; }
        public int Amount { get; set; }
    }
}
