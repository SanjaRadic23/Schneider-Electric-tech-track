using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TechTrack.Domain.Model;
using TechTrack.Helpers;
using TechTrack.Service;
using TechTrack.View.Admin;

namespace TechTrack.ViewModel.Admin
{
    public class OrderManagementViewModel : INotifyPropertyChanged
    {
        public OrderManagementPage OrderManagementPage { get; set; }
        public UserAccount UserAccount { get; set; }
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Product> AddProducts { get; set; }
        public ObservableCollection<PurchaseOrder> Orders { get; set; }

        public ObservableCollection<PurchaseOrderItem> OrderItems { get; set; }
        public RelayCommand AddItem => new RelayCommand(execute => AddOrderItem(), canExecute => CanAddOrderItem());
        public RelayCommand CreateOrder => new RelayCommand(execute => CreatePOrder(), canExecute => CanCreateOrder());
        private decimal totalPrice;

        public OrderManagementViewModel(OrderManagementPage orderManagementPage, UserAccount userAccount)
        { 
            OrderManagementPage = orderManagementPage;
            UserAccount = userAccount;
            Products = new ObservableCollection<Product>();
            AddProducts = new ObservableCollection<Product>();
            Orders = new ObservableCollection<PurchaseOrder>();
            OrderItems = new ObservableCollection<PurchaseOrderItem>();
            totalPrice = 0m;
            LoadProducts();
            LoadOrders();
            UpdateOrderStatusCommand = new RelayCommand(
            execute => UpdateOrderStatus(),
            canExecute => SelectedOrder != null && !string.IsNullOrEmpty(SelectedOrderStatus));

            if (UserService.GetInstance().GetById(userAccount.UserId).Role.ToLower() == "employee")
            {
                orderManagementPage.CreateUserOrderButton.Visibility = Visibility.Collapsed;
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
        public ObservableCollection<string> OrderStatusList { get; set; } = new ObservableCollection<string>
        {
        "created", "sent", "delivered"
        };

        private PurchaseOrder _selectedOrder;
        public PurchaseOrder SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                OnPropertyChanged(nameof(SelectedOrder));
            }
        }

        private string _selectedOrderStatus;
        public string SelectedOrderStatus
        {
            get => _selectedOrderStatus;
            set
            {
                _selectedOrderStatus = value;
                OnPropertyChanged(nameof(SelectedOrderStatus));
            }
        }

        public RelayCommand UpdateOrderStatusCommand { get; }
        public void LoadProducts()
        {
            Products.Clear();
            foreach (Product p in ProductService.GetInstance().GetAll())
            {
                Products.Add(p);
            }
        }
        public void LoadOrders()
        {
            Orders.Clear();
            foreach (var order in PurchaseOrderService.GetInstance().GetAll())
            {

                Orders.Add(order);
                foreach(var item in PurchaseOrderItemService.GetInstance().GetByPurchaseOrderId(order.IdOrder))
                {
                    OrderItems.Add(item);
                }
            }
        }
        public void ProductListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            OrderManagementPage.QuantityTextBox.Clear();
            if (OrderManagementPage.ProductListBox.SelectedItem is Product selectedProduct)
            {
                OrderManagementPage.SelectedProductTextBlock.Text = $"{selectedProduct.Name} - {selectedProduct.Price} EUR";
            }
        }
        private void AddOrderItem()
        {
            if (OrderManagementPage.ProductListBox.SelectedItem is Product selectedProduct &&
                int.TryParse(OrderManagementPage.QuantityTextBox.Text, out int quantity) && quantity > 0)
            {
                var productToAdd = new Product
                {
                    IdProduct = selectedProduct.IdProduct,
                    Description = selectedProduct.Description,
                    Name = selectedProduct.Name,
                    Price = selectedProduct.Price,
                    Quantity = quantity,
                    SupplierId = selectedProduct.SupplierId
                };

                AddProducts.Add(productToAdd);

                totalPrice += selectedProduct.Price * quantity;
                OrderManagementPage.TotalPriceTextBlock.Text = $"{totalPrice:0.00} EUR";
            }
        }
        private void CreatePOrder()
        {
            if (AddProducts.Count > 0)
            {
                foreach (var product in AddProducts)
                {
                    var qua = ProductService.GetInstance().GetById(product.IdProduct).Quantity;
                    int.TryParse(OrderManagementPage.QuantityTextBox.Text, out int quantity);
                }

                PurchaseOrder purchaseOrder = new PurchaseOrder
                {
                    CreationDate = DateTime.Now,
                    Status = "created",
                    UserId = UserAccount.UserId
                };

                PurchaseOrderService.GetInstance().Add(purchaseOrder);

                foreach (var product in AddProducts)
                {
                    int.TryParse(OrderManagementPage.QuantityTextBox.Text, out int quantity);

                    PurchaseOrderItem purchaseOrderItem = new PurchaseOrderItem
                    {
                        PurchaseOrderId = purchaseOrder.IdOrder,
                        ProductId = product.IdProduct,
                        Quantity = quantity,
                        TotalPrice = product.Price * quantity
                    };

                    PurchaseOrderItemService.GetInstance().Add(purchaseOrderItem);

                    var p = ProductService.GetInstance().GetById(product.IdProduct);
                    p.Quantity += quantity;
                    ProductService.GetInstance().Update(p);
                }

                AddProducts.Clear();
                totalPrice = 0;
                OrderManagementPage.TotalPriceTextBlock.Text = "0.00 EUR";
                LoadOrders();
                LoadProducts();
            }
        }
        private bool CanAddOrderItem()
        {
            return OrderManagementPage.ProductListBox.SelectedItem != null &&
                   int.TryParse(OrderManagementPage.QuantityTextBox.Text, out int quantity) && quantity > 0;
        }
        private bool CanCreateOrder()
        {
            return AddProducts.Count > 0;
        }
        private void UpdateOrderStatus()
        {
            SelectedOrder.Status = SelectedOrderStatus;
            SelectedOrder.CreationDate = DateTime.Now;
            PurchaseOrderService.GetInstance().Update(SelectedOrder);
            LoadOrders();
            LoadProducts();
        }
    }
}
