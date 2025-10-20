// Domain/Interfaces/ICustomerAddressRepository.cs
using OrderManagementSystem.Domain.Entities;

namespace OrderManagementSystem.Domain.Interfaces
{
    public interface ICustomerAddressRepository : IRepository<CustomerAddress>
    {
        Task<IEnumerable<CustomerAddress>> GetAddressesByCustomerIdAsync(int customerId);
        Task<IEnumerable<CustomerAddress>> GetAddressesByTypeAsync(int customerId, string addressType);
        Task<CustomerAddress> GetDeliveryAddressAsync(int customerId);
        Task<CustomerAddress> GetInvoiceAddressAsync(int customerId);
    }
}