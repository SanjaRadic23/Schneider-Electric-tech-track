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
    public class UserOrderItemService
    {
        public IUserOrderItemRepository UserOrderItemRepository { get; set; }

        public UserOrderItemService(IUserOrderItemRepository userOrderItemRepository)
        {
            UserOrderItemRepository = userOrderItemRepository;
        }

        public static UserOrderItemService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<UserOrderItemService>();
        }

        public void Add(UserOrderItem newUserOrderItem)
        {
            UserOrderItemRepository.Add(newUserOrderItem);
        }

        public UserOrderItem? GetById(int userOrderId, int productId)
        {
            return UserOrderItemRepository.GetById(userOrderId, productId);
        }

        public List<UserOrderItem> GetAll()
        {
            return UserOrderItemRepository.GetAll();
        }

        public bool Delete(int userOrderId, int productId)
        {
            return UserOrderItemRepository.Delete(userOrderId, productId);
        }
        public List<UserOrderItem>? GetByUserOrderId(int userOrderId)
        {
            return UserOrderItemRepository.GetByUserOrderId(userOrderId);
        }
    }
}
