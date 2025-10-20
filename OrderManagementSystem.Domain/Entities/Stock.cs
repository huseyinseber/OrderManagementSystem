using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Domain.Entities
{
    public class Stock
    {
        public int StockId { get; set; }
        public string StockName { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
        public string Barcode { get; set; }
        public bool IsActive { get; set; }

        // Navigation Properties
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
