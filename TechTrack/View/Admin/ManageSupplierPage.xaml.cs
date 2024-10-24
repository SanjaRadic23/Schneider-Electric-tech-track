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
    /// Interaction logic for ManageSupplierPage.xaml
    /// </summary>
    public partial class ManageSupplierPage : Page
    {
        public ManageSupplierViewModel ManageSupplierViewModel { get; set; }
        public UserAccount UserAccount { get; set; }
        public AdminMainWindow AdminMainWindow { get; set; }
        public ManageSupplierPage(AdminMainWindow adminMainWindow, UserAccount userAccount)
        {
            InitializeComponent();
            AdminMainWindow = adminMainWindow;
            UserAccount = userAccount;
            ManageSupplierViewModel = new ManageSupplierViewModel(this, userAccount);
            DataContext = ManageSupplierViewModel;
        }

        private void AddSupplier_Click(object sender, RoutedEventArgs e)
        {
            SupplierManagementPage supplierManagementPage = new SupplierManagementPage(AdminMainWindow, UserAccount);
            AdminMainWindow.mainFrame.Navigate(supplierManagementPage);
        }

        private void ManageSupplier_Click(object sender, RoutedEventArgs e)
        {
            ManageSupplierPage manageSupplierPage = new ManageSupplierPage(AdminMainWindow, UserAccount);
            AdminMainWindow.mainFrame.Navigate(manageSupplierPage);
        }
    }
}
