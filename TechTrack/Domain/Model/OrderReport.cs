using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTrack.Domain.Model
{
    public class OrderReport
    {
        public string Period { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }

        public decimal AverageOrderValue { get; set; }

        public OrderReport() { }

        public OrderReport(string period, int totalOrders, decimal totalRevenue, decimal average)
        {
            Period = period;
            TotalOrders = totalOrders;
            TotalRevenue = totalRevenue; 
            AverageOrderValue = average;
        }
    }
}
