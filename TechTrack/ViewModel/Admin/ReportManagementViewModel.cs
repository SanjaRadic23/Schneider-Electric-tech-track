using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TechTrack.Helpers;
using TechTrack.Service;
using TechTrack.View.Admin;

namespace TechTrack.ViewModel.Admin
{
    public class ReportManagementViewModel
    {
        public ReportManagementPage ReportManagementPage { get; set; }

        public RelayCommand BestSellingButton => new RelayCommand(execute => BestSelling());
        public RelayCommand InventoryButton => new RelayCommand(execute => Inventory());
        public RelayCommand OrdersButton => new RelayCommand(execute => Orders());
        public RelayCommand BestProductButton => new RelayCommand(execute => BestProduct());
        string selected { get; set; }
        public ReportManagementViewModel(ReportManagementPage reportManagementPage)
        {
            ReportManagementPage = reportManagementPage;
            LoadInventoryReport();
        }

        private void BestSelling()
        {
            ReportManagementPage.BestSellingProductPanel.Visibility = System.Windows.Visibility.Visible;
            ReportManagementPage.InventoryReportPanel.Visibility = System.Windows.Visibility.Collapsed;
            ReportManagementPage.OrdersByPeriodPanel.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void Inventory()
        {
            ReportManagementPage.BestSellingProductPanel.Visibility = System.Windows.Visibility.Collapsed;
            ReportManagementPage.InventoryReportPanel.Visibility = System.Windows.Visibility.Visible;
            ReportManagementPage.OrdersByPeriodPanel.Visibility = System.Windows.Visibility.Collapsed;
            LoadInventoryReport();
        }

        private void Orders()
        {
            ReportManagementPage.BestSellingProductPanel.Visibility = System.Windows.Visibility.Collapsed;
            ReportManagementPage.InventoryReportPanel.Visibility = System.Windows.Visibility.Collapsed;
            ReportManagementPage.OrdersByPeriodPanel.Visibility = System.Windows.Visibility.Visible;
            LoadOrderReports(selected);
        }

        private void BestProduct()
        {
            var bestSellingProducts = ReportingService.GetInstance().GetTopSoldProductsLast7Days();

            if (bestSellingProducts != null && bestSellingProducts.Any())
            {
                ReportManagementPage.BestSellingDataGrid.ItemsSource = bestSellingProducts;
            }
        }

        private void LoadInventoryReport()
        {
            var inventoryData = ReportingService.GetInstance().GetStockAndTotalValue();
            ReportManagementPage.InventoryDataGrid.ItemsSource = inventoryData;
        }
        public void PeriodComboBox(object sender, SelectionChangedEventArgs e)
        {
            string selectedPeriod = (string)((ComboBoxItem)ReportManagementPage.PeriodComboBox.SelectedItem).Content;
            selected = selectedPeriod;
            LoadOrderReports(selectedPeriod);
        }

        private void LoadOrderReports(string selectedPeriod)
        {
            if(selectedPeriod == "Monthly")
            {
                var month = ReportingService.GetInstance().GetMonthlyOrderStatistics();
                ReportManagementPage.OrdersDataGrid.ItemsSource = month;
            }
            else if(selectedPeriod == "Quarterly")
            {
                var quarter = ReportingService.GetInstance().GetQuarterlyOrderStatistics();
                ReportManagementPage.OrdersDataGrid.ItemsSource = quarter;
            }
            else if(selectedPeriod == "Yearly")
            {
                var year = ReportingService.GetInstance().GetYearlyOrderStatistics();
                ReportManagementPage.OrdersDataGrid.ItemsSource = year;
            }
        }
    }
}
