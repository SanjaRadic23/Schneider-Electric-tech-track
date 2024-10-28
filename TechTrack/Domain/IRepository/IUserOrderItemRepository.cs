using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTrack.Domain.Model;

namespace TechTrack.Domain.IRepository
{
    public interface IUserOrderItemRepository
    {
        List<UserOrderItem> GetAll();
        void Add(UserOrderItem userOrderItem);
        UserOrderItem? GetById(int userOrderId, int productId);
        int NextId();
        bool Delete(int userOrderId, int productId);
        List<UserOrderItem>? GetByUserOrderId(int userOrderId);
    }
}
