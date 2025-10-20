using OrderManagementSystem.Application.DTOs;

namespace OrderManagementSystem.Application.Interfaces
{
    public interface IStockService
    {
        Task<StockDto> GetStockByIdAsync(int stockId);
        Task<IEnumerable<StockDto>> GetAllStocksAsync();
        Task<StockDto> CreateStockAsync(StockDto stockDto);
        Task UpdateStockAsync(StockDto stockDto);
        Task DeleteStockAsync(int stockId);
        Task<StockDto> GetStockByBarcodeAsync(string barcode);
    }
}