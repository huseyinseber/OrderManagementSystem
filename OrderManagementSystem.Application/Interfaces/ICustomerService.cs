// Application/Interfaces/ICustomerService.cs
using OrderManagementSystem.Application.DTOs;

namespace OrderManagementSystem.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerDto> GetCustomerByIdAsync(int customerId);
        Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
        Task<CustomerDto> CreateCustomerAsync(CustomerDto customerDto);
        Task UpdateCustomerAsync(CustomerDto customerDto);
        Task DeleteCustomerAsync(int customerId);

        // Sorgular için ek metodlar
        Task<IEnumerable<CustomerDto>> GetCustomersWithDifferentAddressesAsync();
    }
}