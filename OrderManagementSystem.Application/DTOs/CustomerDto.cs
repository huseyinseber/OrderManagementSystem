// Application/DTOs/CustomerDto.cs
namespace OrderManagementSystem.Application.DTOs
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public bool IsActive { get; set; }
        public List<CustomerAddressDto> Addresses { get; set; } = new();
    }

    public class CustomerAddressDto
    {
        public int AddressId { get; set; }
        public string AddressType { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PostalCode { get; set; }
    }
}