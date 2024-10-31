using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTrack.Domain.IRepository;
using TechTrack.Domain.Model;

namespace TechTrack.Service
{
    public class ReportingService
    {
        private IReportingRepository ReportingRepository { get; set; }

        public ReportingService(IReportingRepository reportingRepository)
        {
            ReportingRepository = reportingRepository;
        }

        public static ReportingService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<ReportingService>();
        }

        public List<OrderReport> GetMonthlyOrderStatistics()
        {
            return ReportingRepository.GetMonthlyOrderStatistics();
        }

        public List<OrderReport> GetQuarterlyOrderStatistics()
        {
            return ReportingRepository.GetQuarterlyOrderStatistics();
        }

        public List<InventoryReport> GetStockAndTotalValue()
        {
            return ReportingRepository.GetStockAndTotalValue();
        }

        public List<BestSellingProduct> GetTopSoldProductsLast7Days()
        {
            return ReportingRepository.GetTopSoldProductsLast7Days();
        }

        public List<OrderReport> GetYearlyOrderStatistics()
        {
            return ReportingRepository.GetYearlyOrderStatistics();
        }
    }
}
