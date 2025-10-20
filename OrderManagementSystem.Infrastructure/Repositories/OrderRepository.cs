// Infrastructure/Repositories/OrderRepository.cs
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Domain.Entities;
using OrderManagementSystem.Domain.Interfaces;
using OrderManagementSystem.Infrastructure.Data;

namespace OrderManagementSystem.Infrastructure.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Order> GetOrderWithDetailsAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.DeliveryAddress)
                .Include(o => o.InvoiceAddress)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Stock)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Stock)
                .Where(o => o.CustomerId == customerId && o.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersWithDifferentAddressesAsync()
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.DeliveryAddress)
                .Include(o => o.InvoiceAddress)
                .Where(o => o.DeliveryAddressId != o.InvoiceAddressId && o.IsActive)
                .ToListAsync();
        }

        public async Task<int> GetOrderCountByCityAsync(string city)
        {
            return await _context.Orders
                .Include(o => o.DeliveryAddress)
                .Where(o => o.DeliveryAddress.City == city && o.IsActive)
                .CountAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersWithProductAmountGreaterThanAsync(int stockId, int amount)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Stock)
                .Where(o => o.OrderDetails.Any(od => od.StockId == stockId && od.Amount > amount) && o.IsActive)
                .ToListAsync();
        }
    }
}