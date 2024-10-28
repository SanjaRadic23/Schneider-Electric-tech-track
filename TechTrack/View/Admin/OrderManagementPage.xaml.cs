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
    /// Interaction logic for OrderManagementPage.xaml
    /// </summary>
    public partial class OrderManagementPage : Page
    {
        public OrderManagementViewModel orderManagementViewModel { get; set; }
        public AdminMainWindow AdminMainWindow { get; set; }
        public UserAccount UserAccount { get; set; }
        public OrderManagementPage(AdminMainWindow adminMainWindow, UserAccount userAccount)
        {
            InitializeComponent();
            MainGrid.Focus();
            UserAccount = userAccount;
            AdminMainWindow = adminMainWindow;
            orderManagementViewModel = new OrderManagementViewModel(this, userAccount);
            DataContext = orderManagementViewModel;
        }

        private void ProductListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            orderManagementViewModel.ProductListBox_SelectionChanged(sender, e);    
        }

        private void PButtonClick(object sender, RoutedEventArgs e)
        {
            OrderManagementPage orderManagementPage = new OrderManagementPage(AdminMainWindow, UserAccount);
            AdminMainWindow.mainFrame.Navigate(orderManagementPage);
        }

        private void UButtonClick(object sender, RoutedEventArgs e)
        {
            AdminManagesOrdersPage adminManagesOrdersPage = new AdminManagesOrdersPage(AdminMainWindow, UserAccount);
            AdminMainWindow.mainFrame.Navigate(adminManagesOrdersPage);
        }
    }
}
