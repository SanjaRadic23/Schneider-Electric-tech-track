using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TechTrack.Domain.Model;
using TechTrack.Helpers;
using TechTrack.Service;
using TechTrack.View.Admin;

namespace TechTrack.ViewModel.Admin
{
    public class SupplierManagementViewModel : INotifyPropertyChanged
    {

        public SupplierManagementPage SupplierManagementPage { get; set; }
        public UserAccount UserAccount { get; set; }

        public ObservableCollection<Supplier> Suppliers { get; set; }
        public RelayCommand SubmitCommand => new RelayCommand(execute => Submit(), canExecute => CanSubmit());
        public SupplierManagementViewModel(SupplierManagementPage supplierManagementPage, UserAccount userAccount)
        { 
            SupplierManagementPage = supplierManagementPage;
            UserAccount = userAccount;
            Suppliers = new ObservableCollection<Supplier>();
            LoadSuppliers();

            if (UserService.GetInstance().GetById(userAccount.UserId).Role.ToLower() == "employee")
            {
                supplierManagementPage.ManageSupplierButton.Visibility = Visibility.Collapsed;
            }
        }
        private string _supplierName;
        private string _supplierPhone;
        private string _supplierEmail;
        private string _supplierAddress;
        private string _productName;
        private string _productDescription;
        private decimal _productPrice;

        public string SupplierName
        {
            get => _supplierName;
            set
            {
                _supplierName = value;
                OnPropertyChanged(nameof(SupplierName));
            }
        }

        public string SupplierPhone
        {
            get => _supplierPhone;
            set
            {
                _supplierPhone = value;
                OnPropertyChanged(nameof(SupplierPhone));
            }
        }

        public string SupplierEmail
        {
            get => _supplierEmail;
            set
            {
                _supplierEmail = value;
                OnPropertyChanged(nameof(SupplierEmail));
            }
        }

        public string SupplierAddress
        {
            get => _supplierAddress;
            set
            {
                _supplierAddress = value;
                OnPropertyChanged(nameof(SupplierAddress));
            }
        }

        public string ProductName
        {
            get => _productName;
            set
            {
                _productName = value;
                OnPropertyChanged(nameof(ProductName));
            }
        }

        public string ProductDescription
        {
            get => _productDescription;
            set
            {
                _productDescription = value;
                OnPropertyChanged(nameof(ProductDescription));
            }
        }

        public decimal ProductPrice
        {
            get =>  _productPrice;
            set
            {
                _productPrice = value;
                OnPropertyChanged(nameof(ProductPrice));
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
        public void LoadSuppliers()
        {
            Suppliers.Clear();
            foreach (Supplier s in SupplierService.GetInstance().GetAll())
            {
                Suppliers.Add(s);
            }
        }

        public bool CanSubmit()
        {
            return !string.IsNullOrEmpty(SupplierName) &&
                       !string.IsNullOrEmpty(SupplierPhone) &&
                       !string.IsNullOrEmpty(SupplierEmail) &&
                       !string.IsNullOrEmpty(SupplierAddress) &&
                       !string.IsNullOrEmpty(ProductName) &&
                       !string.IsNullOrEmpty(ProductDescription) &&
                       !string.IsNullOrWhiteSpace(ProductPrice.ToString()) &&
                       ProductPrice > 0;
    }

        public void Submit() 
        {
            Supplier supplier = new Supplier();
            supplier.Name = SupplierName;
            supplier.Address = SupplierAddress;
            supplier.PhoneNumber = SupplierPhone;
            supplier.Email = SupplierEmail;

            SupplierService.GetInstance().Add(supplier);

            var a = supplier;

            if (a != null)
            {
                var s = SupplierService.GetInstance().GetById(supplier.IdSupplier);

                if (s != null)
                {
                    Product product = new Product();
                    product.Name = ProductName;
                    product.Description = ProductDescription;
                    product.Quantity = 0;
                    product.Price = ProductPrice;
                    product.SupplierId = s.IdSupplier;

                    ProductService.GetInstance().Add(product);

                    LoadSuppliers();
                }
            }

        }
    }
}
