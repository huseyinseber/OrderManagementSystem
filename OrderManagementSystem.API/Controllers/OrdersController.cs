using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Application.DTOs;
using OrderManagementSystem.Application.Interfaces;

namespace OrderManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null)
                {
                    return NotFound();
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order: {OrderId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderDto createOrderDto)
        {
            try
            {
                var order = await _orderService.CreateOrderAsync(createOrderDto);
                return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, OrderDto orderDto)
        {
            try
            {
                if (id != orderDto.OrderId)
                {
                    return BadRequest();
                }

                await _orderService.UpdateOrderAsync(orderDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order: {OrderId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order: {OrderId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // Sorgu Endpointleri

        [HttpGet("customers-by-product/{stockId}")]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomersByProduct(int stockId)
        {
            try
            {
                var customers = await _orderService.GetCustomersByProductAsync(stockId);
                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customers by product: {StockId}", stockId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("orders-with-high-quantity/{stockId}/{amount}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersWithProductAmountGreaterThan(int stockId, int amount)
        {
            try
            {
                var orders = await _orderService.GetOrdersWithProductAmountGreaterThanAsync(stockId, amount);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders with product amount greater than: {StockId}, {Amount}", stockId, amount);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("orders-with-different-addresses")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersWithDifferentAddresses()
        {
            try
            {
                var orders = await _orderService.GetOrdersWithDifferentAddressesAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders with different addresses");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("orders-by-customer/{customerName}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByCustomerName(string customerName)
        {
            try
            {
                var orders = await _orderService.GetOrdersByCustomerNameAsync(customerName);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders by customer name: {CustomerName}", customerName);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("order-count-by-city/{city}")]
        public async Task<ActionResult<int>> GetOrderCountByCity(string city)
        {
            try
            {
                var count = await _orderService.GetOrderCountByCityAsync(city);
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order count by city: {City}", city);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}