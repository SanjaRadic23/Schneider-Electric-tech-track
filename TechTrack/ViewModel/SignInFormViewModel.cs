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

namespace TechTrack.ViewModel
{
    public class SignInFormViewModel : INotifyPropertyChanged
    {
        public SignInForm signInForm { get; set; }
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
        }


        public void SignIn() 
        {
            var userAccount = UserAccountService.GetInstance().GetByUsername(Username);
            if(userAccount != null)
            {
                var isCorrect = UserAccountService.GetInstance().ValidateUser(userAccount.Username, signInForm.txtPassword.Password);
                if(isCorrect)
                {
                    var user = UserService.GetInstance().GetById(userAccount.UserId);
                    if(user.Role == "admin")
                    {
                        AdminMainWindow adminMainWindow = new AdminMainWindow(userAccount);
                        adminMainWindow.Show();
                    }
                }
                else
                {
                    signInForm.txtPassword.Clear();
                    signInForm.txtUsername.Clear();
                }
            }
            else
            {
                signInForm.txtPassword.Clear();
                signInForm.txtUsername.Clear();
            }
        }
    }
}
