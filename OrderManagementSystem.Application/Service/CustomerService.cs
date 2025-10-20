// Application/Services/CustomerService.cs
using AutoMapper;
using Microsoft.Extensions.Logging;
using OrderManagementSystem.Application.DTOs;
using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Domain.Interfaces;


namespace OrderManagementSystem.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CustomerService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CustomerDto> GetCustomerByIdAsync(int customerId)
        {
            try
            {
                var customer = await _unitOfWork.Customers.GetCustomerWithAddressesAsync(customerId);
                return _mapper.Map<CustomerDto>(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customer by ID: {CustomerId}", customerId);
                throw;
            }
        }

        public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
        {
            try
            {
                var customers = await _unitOfWork.Customers.GetActiveCustomersAsync();
                return _mapper.Map<IEnumerable<CustomerDto>>(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all customers");
                throw;
            }
        }

        public async Task<CustomerDto> CreateCustomerAsync(CustomerDto customerDto)
        {
            try
            {
                var customer = _mapper.Map<Domain.Entities.Customer>(customerDto);
                customer.IsActive = true;

                await _unitOfWork.Customers.AddAsync(customer);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<CustomerDto>(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating customer");
                throw;
            }
        }

        public async Task UpdateCustomerAsync(CustomerDto customerDto)
        {
            try
            {
                var customer = await _unitOfWork.Customers.GetByIdAsync(customerDto.CustomerId);
                if (customer != null)
                {
                    _mapper.Map(customerDto, customer);
                    await _unitOfWork.Customers.UpdateAsync(customer);
                    await _unitOfWork.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating customer: {CustomerId}", customerDto.CustomerId);
                throw;
            }
        }

        public async Task DeleteCustomerAsync(int customerId)
        {
            try
            {
                var customer = await _unitOfWork.Customers.GetByIdAsync(customerId);
                if (customer != null)
                {
                    customer.IsActive = false;
                    await _unitOfWork.Customers.UpdateAsync(customer);
                    await _unitOfWork.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting customer: {CustomerId}", customerId);
                throw;
            }
        }

        public async Task<IEnumerable<CustomerDto>> GetCustomersWithDifferentAddressesAsync()
        {
            try
            {
                // Fatura ve teslimat adresi farklı olan müşterileri bul
                var customers = await _unitOfWork.Customers.GetAllAsync();
                var result = new List<Domain.Entities.Customer>();

                foreach (var customer in customers)
                {
                    var deliveryAddress = await _unitOfWork.CustomerAddresses.GetDeliveryAddressAsync(customer.CustomerId);
                    var invoiceAddress = await _unitOfWork.CustomerAddresses.GetInvoiceAddressAsync(customer.CustomerId);

                    if (deliveryAddress != null && invoiceAddress != null &&
                        deliveryAddress.AddressId != invoiceAddress.AddressId)
                    {
                        result.Add(customer);
                    }
                }

                return _mapper.Map<IEnumerable<CustomerDto>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customers with different addresses");
                throw;
            }
        }
    }
}