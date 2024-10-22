using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTrack.Domain.Model
{
    public class User
    {
        public int IdUser { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; } 
        public string PhoneNumber { get; set; } 
        public string Email { get; set; }
        public string Role { get; set; }

        public User() { }
        public User(int idUser, string firstName, string lastName, string phoneNumber, string email, string role)
        {
            IdUser = idUser;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            Role = role;
        }
    }
}
