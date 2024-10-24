using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TechTrack.Domain.Model;
using TechTrack.ViewModel.Admin;

namespace TechTrack.View.Admin
{
    /// <summary>
    /// Interaction logic for AdminMainWindow.xaml
    /// </summary>
    public partial class AdminMainWindow : Window
    {
        public ProductManagementPage ProductManagementPage { get; set; }
        public ProductManagementViewModel ProductManagementViewModel { get; set; }
        public UserAccount UserAccount { get; set; }
        public AdminMainWindow(UserAccount userAccount)
        {
            InitializeComponent();
            UserAccount = userAccount;
            ProductManagementPage = new ProductManagementPage(this, userAccount);
            mainFrame.Navigate(ProductManagementPage);

        }

        private void ProductPageClick(object sender, RoutedEventArgs e)
        {
            ProductManagementViewModel = new ProductManagementViewModel(ProductManagementPage, UserAccount);
            ProductManagementViewModel.LoadProducts();
            ProductManagementPage = new ProductManagementPage(this, UserAccount);
            mainFrame.Navigate(ProductManagementPage);
        }

        private void SupplierPageClick(object sender, RoutedEventArgs e)
        {
            SupplierManagementPage supplierManagementPage = new SupplierManagementPage(this, UserAccount);
            mainFrame.Navigate(supplierManagementPage);
        }

        private void OrderPageClick(object sender, RoutedEventArgs e)
        {

        }

        private void ReportPageClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
