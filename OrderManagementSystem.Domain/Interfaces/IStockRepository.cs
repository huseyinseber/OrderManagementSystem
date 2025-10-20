using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManagementSystem.Domain.Entities;

namespace OrderManagementSystem.Domain.Interfaces
{
    public interface IStockRepository : IRepository<Stock>
    {
        Task<Stock> GetByBarcodeAsync(string barcode);
        Task<IEnumerable<Customer>> GetCustomersByProductAsync(int stockId);
    }
}
