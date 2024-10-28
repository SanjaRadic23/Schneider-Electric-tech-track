using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTrack.Domain.IRepository;
using TechTrack.Domain.Model;

namespace TechTrack.Service
{
    public class UserOrderService
    {
        public IUserOrderRepository UserOrderRepository { get; set; }

        public UserOrderService(IUserOrderRepository userOrderRepository)
        {
            UserOrderRepository = userOrderRepository;
        }

        public static UserOrderService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<UserOrderService>();
        }

        public void Add(UserOrder newUserOrder)
        {
            UserOrderRepository.Add(newUserOrder);
        }

        public UserOrder? GetById(int id)
        {
            return UserOrderRepository.GetById(id);
        }

        public List<UserOrder> GetAll()
        {
            return UserOrderRepository.GetAll();
        }

        public bool Delete(int id)
        {
            return UserOrderRepository.Delete(id);
        }

        public int NextId()
        {
            return UserOrderRepository.NextId();
        }
    }
}
