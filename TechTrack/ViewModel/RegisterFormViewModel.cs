using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TechTrack.Helpers;
using TechTrack.View;

namespace TechTrack.ViewModel
{
    public class RegisterFormViewModel : INotifyPropertyChanged
    {
        public RegisterForm RegisterForm { get; set; }
        public RelayCommand RegisterButton => new RelayCommand(execute => Register(), canExecute => CanRegister());
        public RegisterFormViewModel(RegisterForm registerForm) 
        {
            RegisterForm = registerForm;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }
        private string _firstName;
        private string _lastName;
        private string _phoneNumber;
        private string _email;
        private string _username;
        private string _role;

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
                ValidateFirstName();
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
                ValidateLastName();
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
                ValidatePhoneNumber();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
                ValidateEmail();
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
                ValidateUsername();
            }
        }

        public string Role
        {
            get => _role;
            set
            {
                _role = value;
                OnPropertyChanged(nameof(Role));
            }
        }
        public string FirstNameError { get; set; }
        public string LastNameError { get; set; }
        public string PhoneNumberError { get; set; }
        public string EmailError { get; set; }
        public string UsernameError { get; set; }
        public string PasswordError { get; set; }
        public string ConfirmPasswordError { get; set; }
        public bool CanRegister()
        {
            return !string.IsNullOrEmpty(FirstName) &&
                   !string.IsNullOrEmpty(LastName) &&
                   !string.IsNullOrEmpty(PhoneNumber) &&
                   !string.IsNullOrEmpty(Email) &&
                   !string.IsNullOrEmpty(Username) &&
                   !string.IsNullOrEmpty(RegisterForm.PasswordBox.Password) &&
                   !string.IsNullOrEmpty(RegisterForm.ConfirmPasswordBox.Password) &&
                   string.IsNullOrEmpty(FirstNameError) &&
                   string.IsNullOrEmpty(LastNameError) &&
                   string.IsNullOrEmpty(PhoneNumberError) &&
                   string.IsNullOrEmpty(EmailError) &&
                   string.IsNullOrEmpty(UsernameError);
        }

        public void Register()
        {
            ValidateConfirmPassword();
        }

        private void ValidateFirstName()
        {
            FirstNameError = string.IsNullOrEmpty(FirstName) || !Regex.IsMatch(FirstName, @"^[a-zA-Z]+$") ? "First name can only contain letters." : null;
            OnPropertyChanged(nameof(FirstNameError));
        }

        private void ValidateLastName()
        {
            LastNameError = string.IsNullOrEmpty(LastName) || !Regex.IsMatch(LastName, @"^[a-zA-Z]+$") ? "Last name can only contain letters." : null;
            OnPropertyChanged(nameof(LastNameError));
        }

        private void ValidatePhoneNumber()
        {
            PhoneNumberError = string.IsNullOrEmpty(PhoneNumber) || !Regex.IsMatch(PhoneNumber, @"^\d{9,10}$") ? "Phone number can only contain 9 or 10 digits." : null;
            OnPropertyChanged(nameof(PhoneNumberError));
        }

        private void ValidateEmail()
        {
            EmailError = string.IsNullOrEmpty(Email) || !Regex.IsMatch(Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$") ? "Invalid email format." : null;
            OnPropertyChanged(nameof(EmailError));
        }

        private void ValidateUsername()
        {
            UsernameError = string.IsNullOrEmpty(Username) || !Regex.IsMatch(Username, @"^[a-zA-Z0-9._-]{3,20}$") ? "Username can contain letters, digits, and symbols." : null;
            OnPropertyChanged(nameof(UsernameError));
        }

        private void ValidateConfirmPassword()
        {
            ConfirmPasswordError = string.IsNullOrEmpty(RegisterForm.ConfirmPasswordBox.Password) || RegisterForm.ConfirmPasswordBox.Password != RegisterForm.PasswordBox.Password ? "Passwords do not match." : null;
            OnPropertyChanged(nameof(ConfirmPasswordError));
        }
    }
}
