// Domain/Interfaces/IOrderDetailRepository.cs
using OrderManagementSystem.Domain.Entities;

namespace OrderManagementSystem.Domain.Interfaces
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId);
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByStockIdAsync(int stockId);
        Task<decimal> GetTotalAmountByStockAsync(int stockId);
    }
}