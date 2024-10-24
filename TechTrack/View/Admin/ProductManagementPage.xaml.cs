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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TechTrack.Domain.Model;
using TechTrack.ViewModel.Admin;

namespace TechTrack.View.Admin
{
    /// <summary>
    /// Interaction logic for ProductManagementPage.xaml
    /// </summary>
    public partial class ProductManagementPage : Page
    {
        public ProductManagementViewModel ProductManagementViewModel { get; set; }

        public UserAccount UserAccount { get; set; }
        public AdminMainWindow AdminMainWindow { get; set; }
        public ProductManagementPage(AdminMainWindow adminMainWindow, UserAccount userAccount)
        {
            InitializeComponent();
            MainGrid.Focus();
            UserAccount = userAccount;
            AdminMainWindow=adminMainWindow;
            ProductManagementViewModel = new ProductManagementViewModel(this, UserAccount);
            DataContext = ProductManagementViewModel;
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            ProductManagementPage productManagementPage = new ProductManagementPage(AdminMainWindow, UserAccount);
            AdminMainWindow.mainFrame.Navigate(productManagementPage);
        }

        private void ManageProductButton_Click(object sender, RoutedEventArgs e)
        {
            
            ManageProductsPage manageProductsPage = new ManageProductsPage(AdminMainWindow, UserAccount);
            AdminMainWindow.mainFrame.Navigate(manageProductsPage);
        }
    }
}
