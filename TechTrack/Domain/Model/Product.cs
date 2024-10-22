using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTrack.Domain.Model
{
    public class Product
    {
        public int IdProduct { get; set; } 
        public string Name { get; set; } 
        public string Description { get; set; }
        public int Quantity { get; set; } 
        public decimal Price { get; set; }

        public int SupplierId { get; set; }

        public Product() { }

        public Product(int idProduct, string name, string description, int quantity, decimal price, int supplierId)
        {
            IdProduct = idProduct;
            Name = name;
            Description = description;
            Quantity = quantity;
            Price = price;
            SupplierId = supplierId;
        }
    }
}
