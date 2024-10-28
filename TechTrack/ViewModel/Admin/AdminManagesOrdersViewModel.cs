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
    public class AdminManagesOrdersViewModel : INotifyPropertyChanged
    {
        public AdminManagesOrdersPage AdminManagesOrdersPage { get; set; }
        public UserAccount UserAccount { get; set; }
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Product> AddProducts { get; set; }
        public ObservableCollection<UserOrder> uOrders { get; set; }

        public ObservableCollection<UserOrderItem> uOrderItems { get; set; }
        public RelayCommand AddItem => new RelayCommand(execute => AddOrderItem(), canExecute => CanAddOrderItem());
        public RelayCommand CreateOrder => new RelayCommand(execute => CreatePOrder(), canExecute => CanCreateOrder());
        private decimal totalPrice;

        public AdminManagesOrdersViewModel(AdminManagesOrdersPage adminManagesOrdersPage, UserAccount userAccount)
        {
            AdminManagesOrdersPage = adminManagesOrdersPage;
            UserAccount = userAccount;
            Products = new ObservableCollection<Product>();
            AddProducts = new ObservableCollection<Product>();
            uOrders = new ObservableCollection<UserOrder>();
            uOrderItems = new ObservableCollection<UserOrderItem>();
            totalPrice = 0m;
            LoadProducts();
            LoadOrders();
            UpdateOrderStatusCommand = new RelayCommand(
            execute => UpdateOrderStatus(),
            canExecute => SelectedOrder != null && !string.IsNullOrEmpty(SelectedOrderStatus));

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

        private UserOrder _selectedOrder;
        public UserOrder SelectedOrder
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
                if (p.Quantity > 0)
                    Products.Add(p);
            }
        }

        public void LoadOrders()
        {
            uOrders.Clear();
            foreach (var order in UserOrderService.GetInstance().GetAll())
            {
                uOrders.Add(order);
                foreach (var item in UserOrderItemService.GetInstance().GetByUserOrderId(order.IdOrder))
                {
                    uOrderItems.Add(item);
                }
            }
        }

        public void ProductListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            AdminManagesOrdersPage.QuantityTextBox.Clear();
            if (AdminManagesOrdersPage.ProductListBox.SelectedItem is Product selectedProduct)
            {
                AdminManagesOrdersPage.SelectedProductTextBlock.Text = $"{selectedProduct.Name} - {selectedProduct.Price} EUR";
            }
        }
        private void AddOrderItem()
        {
            if (AdminManagesOrdersPage.ProductListBox.SelectedItem is Product selectedProduct &&
                int.TryParse(AdminManagesOrdersPage.QuantityTextBox.Text, out int quantity) && quantity > 0)
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
                AdminManagesOrdersPage.TotalPriceTextBlock.Text = $"{totalPrice:0.00} EUR";
            }
        }
        private void CreatePOrder()
        {
            if (AddProducts.Count > 0)
            {
                UserOrder userOrder = new UserOrder();
                userOrder.CreationDate = DateTime.Now;
                userOrder.Status = "created";
                userOrder.UserId = UserAccount.UserId;

                UserOrderService.GetInstance().Add(userOrder);

                foreach (var product in AddProducts)
                {
                    var qua = ProductService.GetInstance().GetById(product.IdProduct).Quantity;
                    int.TryParse(AdminManagesOrdersPage.QuantityTextBox.Text, out int quantity);
                    if (qua - quantity >= 0)
                    {
                        var o = UserOrderService.GetInstance().GetById(userOrder.IdOrder);

                        UserOrderItem userOrderItem = new UserOrderItem();
                        userOrderItem.UserOrderId = o.IdOrder;
                        userOrderItem.ProductId = product.IdProduct;
                        userOrderItem.Quantity = quantity;
                        userOrderItem.TotalPrice = (product.Price * quantity);

                        UserOrderItemService.GetInstance().Add(userOrderItem);

                        var p = ProductService.GetInstance().GetById(product.IdProduct);
                        p.Quantity = p.Quantity - quantity;
                        ProductService.GetInstance().Update(p);
                    }
                    else
                    {
                        MessageBox.Show("The product" + product.Name + " is out of stock.");
                    }
                }

                AddProducts.Clear();
                totalPrice = 0;
                AdminManagesOrdersPage.TotalPriceTextBlock.Text = "0.00 EUR";
                LoadOrders();
            }
        }
        private bool CanAddOrderItem()
        {
            return AdminManagesOrdersPage.ProductListBox.SelectedItem != null &&
                   int.TryParse(AdminManagesOrdersPage.QuantityTextBox.Text, out int quantity) && quantity > 0;
        }
        private bool CanCreateOrder()
        {
            return AddProducts.Count > 0;
        }
        private void UpdateOrderStatus()
        {
            
        }
    }
}
