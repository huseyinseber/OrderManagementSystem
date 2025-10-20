using AutoMapper;
using OrderManagementSystem.Application.DTOs;
using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace OrderManagementSystem.Application.Services
{
    public class StockService : IStockService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<StockService> _logger;

        public StockService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<StockService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<StockDto> GetStockByIdAsync(int stockId)
        {
            try
            {
                var stock = await _unitOfWork.Stocks.GetByIdAsync(stockId);
                return _mapper.Map<StockDto>(stock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting stock by ID: {StockId}", stockId);
                throw;
            }
        }

        public async Task<IEnumerable<StockDto>> GetAllStocksAsync()
        {
            try
            {
                var stocks = await _unitOfWork.Stocks.GetAllAsync();
                return _mapper.Map<IEnumerable<StockDto>>(stocks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all stocks");
                throw;
            }
        }

        public async Task<StockDto> CreateStockAsync(StockDto stockDto)
        {
            try
            {
                var stock = _mapper.Map<Domain.Entities.Stock>(stockDto);
                stock.IsActive = true;

                await _unitOfWork.Stocks.AddAsync(stock);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<StockDto>(stock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating stock");
                throw;
            }
        }

        public async Task UpdateStockAsync(StockDto stockDto)
        {
            try
            {
                var stock = await _unitOfWork.Stocks.GetByIdAsync(stockDto.StockId);
                if (stock != null)
                {
                    _mapper.Map(stockDto, stock);
                    await _unitOfWork.Stocks.UpdateAsync(stock);
                    await _unitOfWork.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating stock: {StockId}", stockDto.StockId);
                throw;
            }
        }

        public async Task DeleteStockAsync(int stockId)
        {
            try
            {
                var stock = await _unitOfWork.Stocks.GetByIdAsync(stockId);
                if (stock != null)
                {
                    stock.IsActive = false;
                    await _unitOfWork.Stocks.UpdateAsync(stock);
                    await _unitOfWork.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting stock: {StockId}", stockId);
                throw;
            }
        }

        public async Task<StockDto> GetStockByBarcodeAsync(string barcode)
        {
            try
            {
                var stock = await _unitOfWork.Stocks.GetByBarcodeAsync(barcode);
                return _mapper.Map<StockDto>(stock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting stock by barcode: {Barcode}", barcode);
                throw;
            }
        }
    }
}