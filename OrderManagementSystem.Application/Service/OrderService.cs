// Application/Services/OrderService.cs
using AutoMapper;
using Microsoft.Extensions.Logging;
using OrderManagementSystem.Application.DTOs;
using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Domain.Entities;
using OrderManagementSystem.Domain.Interfaces;

namespace OrderManagementSystem.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<OrderService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OrderDto> GetOrderByIdAsync(int orderId)
        {
            try
            {
                var order = await _unitOfWork.Orders.GetOrderWithDetailsAsync(orderId);
                return _mapper.Map<OrderDto>(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order by ID: {OrderId}", orderId);
                throw;
            }
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            try
            {
                var orders = await _unitOfWork.Orders.GetAllAsync();
                return _mapper.Map<IEnumerable<OrderDto>>(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all orders");
                throw;
            }
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            try
            {
                Console.WriteLine($"🔄 Creating new order with {createOrderDto.OrderDetails?.Count} details");

                // Order'ı oluştur
                var order = _mapper.Map<Domain.Entities.Order>(createOrderDto);
                order.OrderDate = DateTime.UtcNow;
                order.IsActive = true;

                // OrderDetails'ı manuel olarak ekle
                if (createOrderDto.OrderDetails != null && createOrderDto.OrderDetails.Any())
                {
                    order.OrderDetails = new List<OrderDetail>();

                    foreach (var detailDto in createOrderDto.OrderDetails)
                    {
                        var orderDetail = new OrderDetail
                        {
                            StockId = detailDto.StockId,
                            Amount = detailDto.Amount,
                            IsActive = true
                        };
                        order.OrderDetails.Add(orderDetail);
                        Console.WriteLine($"➕ Adding order detail - StockId: {detailDto.StockId}, Amount: {detailDto.Amount}");
                    }
                }

                // Order'ı veritabanına ekle (OrderDetails otomatik olarak eklenecek)
                await _unitOfWork.Orders.AddAsync(order);
                await _unitOfWork.CommitAsync();

                Console.WriteLine($"✅ Order created successfully with ID: {order.OrderId}");

                // Order'ı detaylarıyla birlikte tekrar getir
                var createdOrder = await _unitOfWork.Orders.GetOrderWithDetailsAsync(order.OrderId);
                return _mapper.Map<OrderDto>(createdOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order");
                throw;
            }
        }

        public async Task UpdateOrderAsync(OrderDto orderDto)
        {
            try
            {
                // Order'ı detaylarıyla birlikte getir
                var order = await _unitOfWork.Orders.GetOrderWithDetailsAsync(orderDto.OrderId);
                if (order != null)
                {
                    // Temel alanları manuel map et (navigation property'leri bozmamak için)
                    order.CustomerId = orderDto.CustomerId;
                    order.OrderDate = orderDto.OrderDate;
                    order.OrderNo = orderDto.OrderNo;
                    order.TotalPrice = orderDto.TotalPrice;
                    order.Tax = orderDto.Tax;
                    order.DeliveryAddressId = orderDto.DeliveryAddressId;
                    order.InvoiceAddressId = orderDto.InvoiceAddressId;
                    order.IsActive = orderDto.IsActive;

                    // OrderDetails'ı doğru şekilde güncelle
                    await UpdateOrderDetailsAsync(order, orderDto.OrderDetails);

                    await _unitOfWork.Orders.UpdateAsync(order);
                    await _unitOfWork.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order: {OrderId}", orderDto.OrderId);
                throw;
            }
        }

        private async Task UpdateOrderDetailsAsync(Order order, List<OrderDetailDto> orderDetailDtos)
        {
            if (orderDetailDtos == null || !orderDetailDtos.Any())
            {
                // Eğer order details null veya boşsa, mevcut detayları temizle
                order.OrderDetails.Clear();
                return;
            }

            // Mevcut order details'ı temizle
            order.OrderDetails.Clear();

            // Yeni order details ekle
            foreach (var detailDto in orderDetailDtos)
            {
                var orderDetail = new OrderDetail
                {
                    StockId = detailDto.StockId,
                    Amount = detailDto.Amount,
                    OrderId = order.OrderId,
                    IsActive = true // Yeni eklenen detaylar aktif olmalı
                };

                order.OrderDetails.Add(orderDetail);
            }
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            try
            {
                var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
                if (order != null)
                {
                    order.IsActive = false;
                    await _unitOfWork.Orders.UpdateAsync(order);
                    await _unitOfWork.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order: {OrderId}", orderId);
                throw;
            }
        }

        // Sorgular
        public async Task<IEnumerable<CustomerDto>> GetCustomersByProductAsync(int stockId)
        {
            try
            {
                var customers = await _unitOfWork.Stocks.GetCustomersByProductAsync(stockId);
                return _mapper.Map<IEnumerable<CustomerDto>>(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customers by product: {StockId}", stockId);
                throw;
            }
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersWithProductAmountGreaterThanAsync(int stockId, int amount)
        {
            try
            {
                var orders = await _unitOfWork.Orders.GetOrdersWithProductAmountGreaterThanAsync(stockId, amount);
                return _mapper.Map<IEnumerable<OrderDto>>(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders with product amount greater than: {StockId}, {Amount}", stockId, amount);
                throw;
            }
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersWithDifferentAddressesAsync()
        {
            try
            {
                var orders = await _unitOfWork.Orders.GetOrdersWithDifferentAddressesAsync();
                return _mapper.Map<IEnumerable<OrderDto>>(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders with different addresses");
                throw;
            }
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByCustomerNameAsync(string customerName)
        {
            try
            {
                var customers = await _unitOfWork.Customers.GetAllAsync();
                var customer = customers.FirstOrDefault(c => c.CustomerName.Contains(customerName));

                if (customer != null)
                {
                    var orders = await _unitOfWork.Orders.GetOrdersByCustomerIdAsync(customer.CustomerId);
                    return _mapper.Map<IEnumerable<OrderDto>>(orders);
                }

                return new List<OrderDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders by customer name: {CustomerName}", customerName);
                throw;
            }
        }

        public async Task<int> GetOrderCountByCityAsync(string city)
        {
            try
            {
                return await _unitOfWork.Orders.GetOrderCountByCityAsync(city);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order count by city: {City}", city);
                throw;
            }
        }
    }
}