using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTrack.Domain.Model;

namespace TechTrack.Domain.IRepository
{
    public interface IUserOrderRepository
    {
        List<UserOrder> GetAll();
        void Add(UserOrder userOrder);
        UserOrder? GetById(int Id);
        int NextId();
        bool Delete(int id);
        void Update(UserOrder userOrder);
    }
}
