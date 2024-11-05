using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TechTrack.Domain.Model;
using TechTrack.Helpers;
using TechTrack.Service;
using TechTrack.View.Admin;

namespace TechTrack.ViewModel.Admin
{
    public class ManageProductsViewModel : INotifyPropertyChanged
    {
        public ManageProductsPage ManageProductsPage { get; set; }
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Supplier> Suppliers { get; set; }
        public Supplier SelectedSupplierId { get; set; }
        //public Product SelectedProductId { get; set; }
        public UserAccount UserAccount { get; set; }
        public RelayCommand SubmitCommand => new RelayCommand(execute => Submit(), canExecute => CanSubmit());
        public RelayCommand SearchButton => new RelayCommand(execute => Search());

        public RelayCommand DeleteButton => new RelayCommand(execute => Delete(), canExecute => CanDelete());


        public RelayCommand AvailabeButton => new RelayCommand(execute => Available());

        public RelayCommand FilterByPriceCommand => new RelayCommand(execute => FilterByPrice());
        public ManageProductsViewModel(ManageProductsPage manageProductsPage, UserAccount userAccount)
        {
            ManageProductsPage = manageProductsPage;
            Products = new ObservableCollection<Product>();
            Suppliers = new ObservableCollection<Supplier>();
            LoadProducts();
            foreach (Supplier s in SupplierService.GetInstance().GetAll())
            {
                Suppliers.Add(s);
            }

            UserAccount = userAccount;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }

        private Product _selectedProduct;
        public Product SelectedProductId
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProductId));

                if (_selectedProduct != null)
                {
                    ProductName = _selectedProduct.Name;
                    ProductDescription = _selectedProduct.Description;
                    ProductQuantity = _selectedProduct.Quantity;
                    ProductPrice = _selectedProduct.Price;
                    //ovo ne radi
                    SelectedSupplierId = SupplierService.GetInstance().GetById(_selectedProduct.SupplierId);
                }
                else
                {
                    ProductName = string.Empty;
                    ProductDescription = string.Empty;
                    ProductQuantity = 0;
                    ProductPrice = 0;
                    SelectedSupplierId = null;
                }
            }
        }

        private decimal? _selectedMinPrice;
        private decimal? _selectedMaxPrice;

        // Minimalna cena
        public decimal? SelectedMinPrice
        {
            get => _selectedMinPrice;
            set
            {
                if (_selectedMinPrice != value)
                {
                    _selectedMinPrice = value;
                    OnPropertyChanged(nameof(SelectedMinPrice));
                }
            }
        }

        // Maksimalna cena
        public decimal? SelectedMaxPrice
        {
            get => _selectedMaxPrice;
            set
            {
                if (_selectedMaxPrice != value)
                {
                    _selectedMaxPrice = value;
                    OnPropertyChanged(nameof(SelectedMaxPrice));
                }
            }
        }

        private ObservableCollection<decimal> _priceOptions;
        public ObservableCollection<decimal> PriceOptions
        {
            get
            {
                if (_priceOptions == null)
                {
                    _priceOptions = new ObservableCollection<decimal>();
                    _priceOptions.Add(0);
                    _priceOptions.Add(100);
                    _priceOptions.Add(500);
                    _priceOptions.Add(1000);
                }
                return _priceOptions;
            }
        }

        private ObservableCollection<decimal> _maxPriceOptions;
        public ObservableCollection<decimal> MaxPriceOptions
        {
            get
            {
                if (_maxPriceOptions == null)
                {
                    _maxPriceOptions = new ObservableCollection<decimal>();
                    _maxPriceOptions.Add(100);
                    _maxPriceOptions.Add(500);
                    _maxPriceOptions.Add(1000);
                    _maxPriceOptions.Add(1500);
                }
                return _maxPriceOptions;
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
        private void LoadProducts()
        {
            Products.Clear();
            foreach (Product p in ProductService.GetInstance().GetAll())
            {
                Products.Add(p);
            }
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
            Product product = new Product();
            product.IdProduct = SelectedProductId.IdProduct;
            product.Quantity = ProductQuantity;
            product.Price = ProductPrice;
            product.Description = ProductDescription;
            product.Name = ProductName;
            product.SupplierId = SelectedSupplierId.IdSupplier;
            ProductService.GetInstance().Update(product);
            LoadProducts();
        }

        private void Search()
        { 
            if(string.IsNullOrWhiteSpace(ManageProductsPage.searchTxtBox.Text) || string.IsNullOrEmpty(ManageProductsPage.searchTxtBox.Text))
            {
                LoadProducts();
            }
            else
            {
                Products.Clear();
                foreach (Product p in ProductService.GetInstance().Search(ManageProductsPage.searchTxtBox.Text))
                {
                    Products.Add(p);
                }
            }
        }
        public bool CanDelete() {
            if (SelectedProductId != null)
            {
                return true;
            }
            return false;
        }

        public void Delete()
        {
            var isDeleted = ProductService.GetInstance().Delete(SelectedProductId.IdProduct);
            LoadProducts();
        }


        private void FilterByPrice()
        {
            if (SelectedMinPrice.HasValue && SelectedMaxPrice.HasValue)
            {
                Products.Clear();
                ObservableCollection<Product> products = new ObservableCollection<Product>(
                    ProductService.GetInstance().GetProductsInPriceRange(SelectedMinPrice.Value, SelectedMaxPrice.Value));
                foreach (Product p in products)
                {
                    Products.Add(p);
                }
            }
            else
            {
                if(ManageProductsPage.searchTxtBox.Text != "")
                Products.Clear();
                ObservableCollection<Product> pro = new ObservableCollection<Product>(
                    ProductService.GetInstance().GetProductsByName(ManageProductsPage.searchTxtBox.Text));
                foreach (Product p in pro)
                {
                    Products.Add(p);
                }
            }
        }
        private void Available()
        {
            Products.Clear();
            ObservableCollection<Product> pro = new ObservableCollection<Product>(
                ProductService.GetInstance().GetAvailableProducts());
            foreach (Product p in pro)
            {
                Products.Add(p);
            }
        }
    }
}
