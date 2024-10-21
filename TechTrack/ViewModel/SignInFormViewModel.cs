using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTrack.Helpers;
using TechTrack.Repository;
using TechTrack.Service;
using TechTrack.View.Admin;
using TechTrack.View.Employee;

namespace TechTrack.ViewModel
{
    public class SignInFormViewModel : INotifyPropertyChanged
    {
        public SignInForm signInForm { get; set; }
       // public UserAccountRepository userAccountRepository { get; set; }
        public RelayCommand SignInButton => new RelayCommand(execute => SignIn());

        private string _username;
        private string _password;
        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                if (value != _password)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }
        public SignInFormViewModel(SignInForm signInForm)
        {
            this.signInForm = signInForm;
           // userAccountRepository = new UserAccountRepository();
        }


        public void SignIn() 
        {
            var userAccount = UserAccountService.GetInstance().GetByUsername(Username);
            if(userAccount != null)
            {
                var isCorrect = UserAccountService.GetInstance().ValidateUser(userAccount.Username, userAccount.Password);
                if (isCorrect)
                {
                    var user = UserService.GetInstance().GetById(userAccount.UserId);
                    if(user.Role == "admin")
                    {
                        AdminMainWindow adminMainWindow = new AdminMainWindow();
                        adminMainWindow.Show();
                    }
                    else if(user.Role == "employee")
                    {
                        EmployeeMainWindow employeeMainWindow = new EmployeeMainWindow();
                        employeeMainWindow.Show();
                    }
                }
                else
                {
                    //ako nije validan user restartovati polje password i username da budu prazna
                    signInForm.txtPassword.Clear();
                    signInForm.txtUsername.Clear();
                    signInForm.Close();
                }
            }
            else
            {
                signInForm.txtPassword.Clear();
                signInForm.txtUsername.Clear();
                signInForm.Close();
            }
        }
    }
}
