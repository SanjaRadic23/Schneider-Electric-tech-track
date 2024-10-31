using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTrack.Domain.Model
{
    public class InventoryReport
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalValue { get; set; }

        public InventoryReport() { }

        public InventoryReport(string productName, int quantity, decimal totalValue)
        {
            ProductName = productName;
            Quantity = quantity;
            TotalValue = totalValue;
        }
    }
}
