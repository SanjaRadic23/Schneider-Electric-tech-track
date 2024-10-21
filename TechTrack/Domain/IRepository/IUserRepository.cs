using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTrack.Domain.Model;

namespace TechTrack.Domain.IRepository
{
    public interface IUserRepository
    {
        List<User> GetAll();
        void Add(User user);
        User? GetById(int Id);
        int NextId();
        bool Delete(int id);
    }
}
