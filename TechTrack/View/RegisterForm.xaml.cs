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
using TechTrack.ViewModel;

namespace TechTrack.View
{
    /// <summary>
    /// Interaction logic for RegisterForm.xaml
    /// </summary>
    public partial class RegisterForm : Window
    {
        public RegisterFormViewModel RegisterFormViewModel { get; set; }
        public RegisterForm()
        {
            InitializeComponent();
            RegisterFormViewModel = new RegisterFormViewModel(this);
            DataContext = RegisterFormViewModel;
        }

        private void OnSubmitClick(object sender, RoutedEventArgs e)
        {

        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                RegisterFormViewModel.RegisterForm.PasswordBox.Password = passwordBox.Password;
            }
        }

        private void PasswordBox_ConfirmPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                RegisterFormViewModel.RegisterForm.ConfirmPasswordBox.Password = passwordBox.Password;
            }
        }
    }
}
