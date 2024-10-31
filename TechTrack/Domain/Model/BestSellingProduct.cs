using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTrack.Domain.Model
{
    public class BestSellingProduct
    {
        public string ProductName { get; set; }
        public int TotalQuantity { get; set; }

        public BestSellingProduct() { }

        public BestSellingProduct(string productName, int totalQuantity) { ProductName = productName; TotalQuantity = totalQuantity; }
    }
}
