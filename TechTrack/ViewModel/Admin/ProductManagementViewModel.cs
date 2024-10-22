using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TechTrack.Domain.Model;
using TechTrack.Helpers;
using TechTrack.Service;
using TechTrack.View.Admin;

namespace TechTrack.ViewModel.Admin
{
    public class ProductManagementViewModel : INotifyPropertyChanged
    {
        public ProductManagementPage productManagementPage { get; set; }
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Supplier> Suppliers { get; set; }
        public Supplier SelectedSupplierId { get; set; }
        public RelayCommand SubmitCommand => new RelayCommand(execute => Submit(), canExecute => CanSubmit());
        public ProductManagementViewModel(ProductManagementPage productManagementPage) 
        {
            this.productManagementPage = productManagementPage;
            Products = new ObservableCollection<Product>();
            Suppliers = new ObservableCollection<Supplier>();
            LoadProducts();
            foreach (Supplier s in SupplierService.GetInstance().GetAll())
            {
                Suppliers.Add(s);
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
        private string _productName;
        private int _quantity;
        private decimal _price;
        private string _description;

        public string ProductName
        {
            get => _productName;
            set
            {
                _productName = value;
                OnPropertyChanged(nameof(ProductName));
                ValidateProductName();
            }
        }
        public int ProductQuantity
        {
            get => _quantity;
            set
            {
                if (int.TryParse(value.ToString(), out int parsedValue) && parsedValue >= 0)
                {
                    _quantity = parsedValue;
                    OnPropertyChanged(nameof(ProductQuantity));
                    ValidateQuantity();
                }
                else if (string.IsNullOrWhiteSpace(ProductQuantity.ToString()))
                {
                    QuantityError = "Quantity is required.";
                    OnPropertyChanged(nameof(QuantityError));
                }
                else
                {
                    QuantityError = "Quantity must be a valid positive number.";
                    OnPropertyChanged(nameof(QuantityError));
                }
            }
        }

        public decimal ProductPrice
        {
            get => _price;
            set
            {
                if (decimal.TryParse(value.ToString(), out decimal parsedValue) && parsedValue >= 0)
                {
                    _price = parsedValue;
                    OnPropertyChanged(nameof(ProductPrice));
                    ValidatePrice();
                }
                else if (string.IsNullOrWhiteSpace(ProductPrice.ToString()))
                {
                    PriceError = "Price is required.";
                    OnPropertyChanged(nameof(PriceError));
                }
                else
                {
                    PriceError = "Price must be a valid positive number.";
                    OnPropertyChanged(nameof(PriceError));
                }
            }
        }
        public string ProductDescription
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(ProductDescription));
                ValidateDescription();
            }
        }
        public string ProductNameError { get; set; }
        public string QuantityError { get; set; }
        public string PriceError { get; set; }
        public string DescriptionError { get; set; }

        private void ValidateProductName()
        {
            ProductNameError = string.IsNullOrEmpty(ProductName) ? "Product name is required." : null;
            OnPropertyChanged(nameof(ProductNameError));
        }
        private void ValidateDescription()
        {
            DescriptionError = string.IsNullOrEmpty(ProductDescription) ? "Product description is required." : null;
            OnPropertyChanged(nameof(DescriptionError));
        }
        private void ValidateQuantity()
        {
            if (string.IsNullOrWhiteSpace(ProductQuantity.ToString()) || ProductQuantity < 0)
            {
                QuantityError = "Quantity must be a positive number.";
            }
            else
            {
                QuantityError = null;
            }
            OnPropertyChanged(nameof(QuantityError));
        }

        private void ValidatePrice()
        {
            if (string.IsNullOrWhiteSpace(ProductPrice.ToString()) || ProductPrice < 0)
            {
                PriceError = "Price must be a positive number.";
            }
            else
            {
                PriceError = null;
            }
            OnPropertyChanged(nameof(PriceError));
        }
        private bool CanSubmit()
        {
            return string.IsNullOrEmpty(ProductNameError) &&
                   string.IsNullOrEmpty(QuantityError) &&
                   string.IsNullOrEmpty(PriceError) &&
                   !string.IsNullOrEmpty(ProductName) &&
                   !string.IsNullOrEmpty(ProductDescription) &&
                   ProductQuantity > 0 &&
                   !string.IsNullOrWhiteSpace(ProductQuantity.ToString()) &&
                   !string.IsNullOrWhiteSpace(ProductPrice.ToString()) &&
                   ProductPrice > 0;
        }

        private void Submit()
        {
            var pr = ProductService.GetInstance().GetByNameAndSupplierId(ProductName, SelectedSupplierId.IdSupplier);
            if (pr == null)
            {
                Product product = new Product();
                product.Price = ProductPrice;
                product.Quantity = ProductQuantity;
                product.Name = ProductName;
                product.Description = ProductDescription;
                product.SupplierId = SelectedSupplierId.IdSupplier;

                ProductService.GetInstance().Add(product);
                LoadProducts();
            }
            else
            {
                MessageBox.Show("A product with that name and supplier already exists.");
            }
        }
        private void LoadProducts()
        {
            Products.Clear();
            foreach (Product p in ProductService.GetInstance().GetAll())
            {
                Products.Add(p);
            }
        }
    }
}
