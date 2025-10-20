using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Domain.Entities;
using OrderManagementSystem.Domain.Interfaces;
using OrderManagementSystem.Infrastructure.Data;

namespace OrderManagementSystem.Infrastructure.Repositories
{
    public class StockRepository : BaseRepository<Stock>, IStockRepository
    {
        public StockRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Stock> GetByBarcodeAsync(string barcode)
        {
            return await _context.Stocks
                .FirstOrDefaultAsync(s => s.Barcode == barcode && s.IsActive);
        }

        public async Task<IEnumerable<Customer>> GetCustomersByProductAsync(int stockId)
        {
            return await _context.Customers
                .Include(c => c.CustomerAddresses)
                .Where(c => c.Orders.Any(o => o.OrderDetails.Any(od => od.StockId == stockId && od.IsActive)))
                .ToListAsync();
        }
    }
}