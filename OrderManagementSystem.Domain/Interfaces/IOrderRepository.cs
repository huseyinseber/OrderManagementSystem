using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManagementSystem.Domain.Entities;

namespace OrderManagementSystem.Domain.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order> GetOrderWithDetailsAsync(int orderId);
        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId);
        Task<IEnumerable<Order>> GetOrdersWithDifferentAddressesAsync();
        Task<int> GetOrderCountByCityAsync(string city);
        Task<IEnumerable<Order>> GetOrdersWithProductAmountGreaterThanAsync(int stockId, int amount);
    }
}
