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
using TechTrack.ViewModel.Admin;

namespace TechTrack.View.Admin
{
    /// <summary>
    /// Interaction logic for ManageProductsPage.xaml
    /// </summary>
    public partial class ManageProductsPage : Page
    {
        public AdminMainWindow AdminMainWindow { get; set; }
        public ManageProductsViewModel manageProductsViewModel { get; set; }
        public ManageProductsPage(AdminMainWindow adminMainWindow)
        {
            InitializeComponent();
            AdminMainWindow = adminMainWindow;
            manageProductsViewModel = new ManageProductsViewModel(this);
            DataContext = manageProductsViewModel;
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            ProductManagementPage productManagementPage = new ProductManagementPage(AdminMainWindow);
            AdminMainWindow.mainFrame.Navigate(productManagementPage);
        }

        private void ManageProductButton_Click(object sender, RoutedEventArgs e)
        {
            ManageProductsPage manageProductsPage = new ManageProductsPage(AdminMainWindow);
            AdminMainWindow.mainFrame.Navigate(manageProductsPage);
        }
    }
}
