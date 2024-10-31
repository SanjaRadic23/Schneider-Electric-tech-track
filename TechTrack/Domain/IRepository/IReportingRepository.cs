using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTrack.Domain.Model;

namespace TechTrack.Domain.IRepository
{
    public interface IReportingRepository
    {
        public List<BestSellingProduct> GetTopSoldProductsLast7Days();

        public List<InventoryReport> GetStockAndTotalValue();

        public List<OrderReport> GetMonthlyOrderStatistics();
        public List<OrderReport> GetQuarterlyOrderStatistics();

        public List<OrderReport> GetYearlyOrderStatistics();
    }
}
