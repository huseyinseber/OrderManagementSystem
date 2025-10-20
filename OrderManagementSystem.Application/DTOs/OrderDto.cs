// Application/DTOs/OrderDto.cs
namespace OrderManagementSystem.Application.DTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderNo { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Tax { get; set; }
        public int DeliveryAddressId { get; set; }
        public int InvoiceAddressId { get; set; }
        public bool IsActive { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; } = new();
    }

    public class OrderDetailDto
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int StockId { get; set; }
        public int Amount { get; set; }
        public string StockName { get; set; }
        public decimal Price { get; set; }
    }

    public class CreateOrderDto
    {
        public int CustomerId { get; set; }
        public string OrderNo { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Tax { get; set; }
        public int DeliveryAddressId { get; set; }
        public int InvoiceAddressId { get; set; }
        public List<CreateOrderDetailDto> OrderDetails { get; set; } = new();
    }

    public class CreateOrderDetailDto
    {
        public int StockId { get; set; }
        public int Amount { get; set; }
    }
}