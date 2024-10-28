using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTrack.Domain.Model
{
    public class UserOrderItem
    {
        public int UserOrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }

        public UserOrderItem() { }

        public UserOrderItem(int userOrderId, int productId, int quantity, decimal totalPrice)
        {
            UserOrderId = userOrderId;
            ProductId = productId;
            Quantity = quantity;
            TotalPrice = totalPrice;
        }
    }
}
