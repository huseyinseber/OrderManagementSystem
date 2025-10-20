using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Application.DTOs;
using OrderManagementSystem.Application.Interfaces;

namespace OrderManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StocksController : ControllerBase
    {
        private readonly IStockService _stockService;
        private readonly ILogger<StocksController> _logger;

        public StocksController(IStockService stockService, ILogger<StocksController> logger)
        {
            _stockService = stockService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockDto>>> GetStocks()
        {
            try
            {
                var stocks = await _stockService.GetAllStocksAsync();
                return Ok(stocks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting stocks");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StockDto>> GetStock(int id)
        {
            try
            {
                var stock = await _stockService.GetStockByIdAsync(id);
                if (stock == null)
                {
                    return NotFound();
                }
                return Ok(stock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting stock: {StockId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<StockDto>> CreateStock(StockDto stockDto)
        {
            try
            {
                var stock = await _stockService.CreateStockAsync(stockDto);
                return CreatedAtAction(nameof(GetStock), new { id = stock.StockId }, stock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating stock");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStock(int id, StockDto stockDto)
        {
            try
            {
                if (id != stockDto.StockId)
                {
                    return BadRequest();
                }

                await _stockService.UpdateStockAsync(stockDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating stock: {StockId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock(int id)
        {
            try
            {
                await _stockService.DeleteStockAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting stock: {StockId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("by-barcode/{barcode}")]
        public async Task<ActionResult<StockDto>> GetStockByBarcode(string barcode)
        {
            try
            {
                var stock = await _stockService.GetStockByBarcodeAsync(barcode);
                if (stock == null)
                {
                    return NotFound();
                }
                return Ok(stock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting stock by barcode: {Barcode}", barcode);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}