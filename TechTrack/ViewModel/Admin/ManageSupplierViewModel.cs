using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTrack.Domain.Model;
using TechTrack.Helpers;
using TechTrack.Service;
using TechTrack.View.Admin;

namespace TechTrack.ViewModel.Admin
{
    public class ManageSupplierViewModel : INotifyPropertyChanged
    {
        public ManageSupplierPage ManageSupplierPage { get; set; }
        public UserAccount UserAccount { get; set; }
        public ObservableCollection<Supplier> Suppliers { get; set; }
        public RelayCommand SubmitCommand => new RelayCommand(execute => Submit(), canExecute => CanSubmit());
        public RelayCommand SearchButton => new RelayCommand(execute => Search());

        public RelayCommand DeleteButton => new RelayCommand(execute => Delete(), canExecute => CanDelete());
        public ManageSupplierViewModel(ManageSupplierPage manageSupplierPage, UserAccount userAccount)
        { 
            ManageSupplierPage = manageSupplierPage;
            UserAccount = userAccount;
            Suppliers = new ObservableCollection<Supplier>();
            LoadSuppliers();
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
            get => _productPrice;
            set
            {
                _productPrice = value;
                OnPropertyChanged(nameof(ProductPrice));
            }
        }
        private Supplier _selectedSupplier;
        public Supplier SelectedSupplierId
        {
            get => _selectedSupplier;
            set
            {
                _selectedSupplier = value;
                OnPropertyChanged(nameof(SelectedSupplierId));

                if (_selectedSupplier != null)
                {
                    SupplierName = _selectedSupplier.Name;
                    SupplierPhone = _selectedSupplier.PhoneNumber;
                    SupplierEmail = _selectedSupplier.Email;
                    SupplierAddress = _selectedSupplier.Address;

                    var product = ProductService.GetInstance().GetBySupplierId(_selectedSupplier.IdSupplier);
                    if (product != null)
                    {
                        ProductName = product.Name;
                        ProductDescription = product.Description;
                        ProductPrice = product.Price;
                    }
                    else
                    {
                        ProductName = string.Empty;
                        ProductDescription = string.Empty;
                        ProductPrice = 0;
                    }
                }
                else
                {
                    SupplierName = string.Empty;
                    SupplierPhone = string.Empty;
                    SupplierEmail = string.Empty;
                    SupplierAddress = string.Empty;
                    ProductName = string.Empty;
                    ProductDescription = string.Empty;
                    ProductPrice = 0;
                }
            }
        }


        private bool CanSubmit()
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

        private void Submit()
        {
            Supplier supplier = SupplierService.GetInstance().GetById(SelectedSupplierId.IdSupplier);
            supplier.PhoneNumber = SupplierPhone;
            supplier.Name = SupplierName;
            supplier.Address = SupplierAddress;
            supplier.Email = SupplierEmail;

            Product product = ProductService.GetInstance().GetBySupplierId(SelectedSupplierId.IdSupplier);
            product.Price = ProductPrice;
            product.Description = ProductDescription;
            product.Name = ProductName;

            SupplierService.GetInstance().Update(supplier);
            ProductService.GetInstance().Update(product);
            LoadSuppliers();
        }

        private void Search()
        {
            if (string.IsNullOrWhiteSpace(ManageSupplierPage.searchTxtBox.Text) || string.IsNullOrEmpty(ManageSupplierPage.searchTxtBox.Text))
            {
                LoadSuppliers();
            }
            else
            {
                Suppliers.Clear();
                foreach (Supplier s in SupplierService.GetInstance().SearchSuppliers(ManageSupplierPage.searchTxtBox.Text))
                {
                    Suppliers.Add(s);
                }
            }
        }
        public bool CanDelete()
        {
            if (SelectedSupplierId != null)
            {
                return true;
            }
            return false;
        }

        public void Delete()
        {
            var isDeleted = false; 
            if (SelectedSupplierId != null)
            {

                if (string.IsNullOrEmpty(ProductName) && string.IsNullOrEmpty(ProductDescription) && ProductPrice == 0)
                {
                    isDeleted = SupplierService.GetInstance().DeleteSupplier(SelectedSupplierId.IdSupplier);
                }

                isDeleted = SupplierService.GetInstance().Delete(SelectedSupplierId.IdSupplier);

                LoadSuppliers();
            }
        }
    }
}
