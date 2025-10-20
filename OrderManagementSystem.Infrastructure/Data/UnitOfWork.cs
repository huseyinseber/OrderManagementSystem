// Infrastructure/Data/UnitOfWork.cs
using OrderManagementSystem.Domain.Interfaces;
using OrderManagementSystem.Infrastructure.Repositories;

namespace OrderManagementSystem.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        private ICustomerRepository _customers;
        private ICustomerAddressRepository _customerAddresses;
        private IStockRepository _stocks;
        private IOrderRepository _orders;
        private IOrderDetailRepository _orderDetails;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICustomerRepository Customers =>
            _customers ??= new CustomerRepository(_context);

        public ICustomerAddressRepository CustomerAddresses =>
            _customerAddresses ??= new CustomerAddressRepository(_context);

        public IStockRepository Stocks =>
            _stocks ??= new StockRepository(_context);

        public IOrderRepository Orders =>
            _orders ??= new OrderRepository(_context);

        public IOrderDetailRepository OrderDetails =>
            _orderDetails ??= new OrderDetailRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}