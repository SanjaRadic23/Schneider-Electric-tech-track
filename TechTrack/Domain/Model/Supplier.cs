using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTrack.Domain.Model
{
    public class Supplier
    {
        public int IdSupplier { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public Supplier() { }

        public Supplier(int idSupplier, string name, string phoneNumber, string email, string address)
        {
            IdSupplier = idSupplier;
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
            Address = address;
        }
    }
}
