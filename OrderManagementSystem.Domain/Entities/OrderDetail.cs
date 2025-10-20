using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Domain.Entities
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int StockId { get; set; }
        public int Amount { get; set; }
        public bool IsActive { get; set; } = true; // Default değer eklendi

        // Navigation Properties
        public virtual Order Order { get; set; }
        public virtual Stock Stock { get; set; }
    }
}
