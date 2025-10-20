using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderManagementSystem.Domain.Entities;
using OrderManagementSystem.Domain.Interfaces;
using OrderManagementSystem.Infrastructure.Data;

namespace OrderManagementSystem.Infrastructure.Repositories
{
    public class CustomerAddressRepository : BaseRepository<CustomerAddress>, ICustomerAddressRepository
    {
        public CustomerAddressRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<IEnumerable<CustomerAddress>> GetAddressesByCustomerIdAsync(int customerId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CustomerAddress>> GetAddressesByTypeAsync(int customerId, string addressType)
        {
            throw new NotImplementedException();
        }

        public async Task<CustomerAddress> GetDeliveryAddressAsync(int customerId)
        {
            return await _context.CustomerAddresses
                .FirstOrDefaultAsync(a => a.CustomerId == customerId && 
                                         a.IsActive && 
                                         (a.AddressType == "Delivery" || a.AddressType == "Both"));
        }

        public async Task<CustomerAddress> GetInvoiceAddressAsync(int customerId)
        {
            return await _context.CustomerAddresses
                .FirstOrDefaultAsync(a => a.CustomerId == customerId && 
                                         a.IsActive && 
                                         (a.AddressType == "Invoice" || a.AddressType == "Both"));
        }
    }
}