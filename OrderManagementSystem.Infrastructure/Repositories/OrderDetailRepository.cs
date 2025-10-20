// Infrastructure/Repositories/OrderDetailRepository.cs
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Domain.Entities;
using OrderManagementSystem.Domain.Interfaces;
using OrderManagementSystem.Infrastructure.Data;

namespace OrderManagementSystem.Infrastructure.Repositories
{
    public class OrderDetailRepository : BaseRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            return await _context.OrderDetails
                .Include(od => od.Stock)
                .Where(od => od.OrderId == orderId /*&& od.IsActive*/)
                .ToListAsync();
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByStockIdAsync(int stockId)
        {
            return await _context.OrderDetails
                .Include(od => od.Order)
                .Include(od => od.Stock)
                .Where(od => od.StockId == stockId && od.IsActive)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalAmountByStockAsync(int stockId)
        {
            return await _context.OrderDetails
                .Where(od => od.StockId == stockId && od.IsActive)
                .SumAsync(od => od.Amount);
        }
    }
}