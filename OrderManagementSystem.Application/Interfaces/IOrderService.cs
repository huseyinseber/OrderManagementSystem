// Application/Interfaces/IOrderService.cs
using OrderManagementSystem.Application.DTOs;

namespace OrderManagementSystem.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto);
        Task UpdateOrderAsync(OrderDto orderDto);
        Task DeleteOrderAsync(int orderId);

        // Sorgular
        Task<IEnumerable<CustomerDto>> GetCustomersByProductAsync(int stockId);
        Task<IEnumerable<OrderDto>> GetOrdersWithProductAmountGreaterThanAsync(int stockId, int amount);
        Task<IEnumerable<OrderDto>> GetOrdersWithDifferentAddressesAsync();
        Task<IEnumerable<OrderDto>> GetOrdersByCustomerNameAsync(string customerName);
        Task<int> GetOrderCountByCityAsync(string city);
    }
}