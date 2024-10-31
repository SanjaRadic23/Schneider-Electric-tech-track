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
    /// Interaction logic for ReportManagementPage.xaml
    /// </summary>
    public partial class ReportManagementPage : Page
    {
        public ReportManagementViewModel ReportManagementViewModel { get; set; }
        public ReportManagementPage(AdminMainWindow adminMainWindow, UserAccount userAccount)
        {
            InitializeComponent();
            ReportManagementViewModel = new ReportManagementViewModel(this);
            DataContext = ReportManagementViewModel;
        }

        private void PeriodComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ReportManagementViewModel.PeriodComboBox(sender, e);
        }
    }
}
