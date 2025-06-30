
namespace Ecommerce.Core.Entities.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int LocalUserId { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderDetailsDTO> OrderDetails { get; set; }
    }

    public class OrderDetailsDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
    }
}
