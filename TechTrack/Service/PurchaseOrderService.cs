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
    public class PurchaseOrderService
    {
        public IPurchaseOrderRepository PurchaseOrderRepository { get; set; }

        public PurchaseOrderService(IPurchaseOrderRepository purchaseOrderRepository)
        {
            PurchaseOrderRepository = purchaseOrderRepository;
        }

        public static PurchaseOrderService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<PurchaseOrderService>();
        }

        public void Add(PurchaseOrder newPurchaseOrder)
        {
            PurchaseOrderRepository.Add(newPurchaseOrder);
        }

        public PurchaseOrder? GetById(int id)
        {
            return PurchaseOrderRepository.GetById(id);
        }

        public List<PurchaseOrder> GetAll()
        {
            return PurchaseOrderRepository.GetAll();
        }

        public bool Delete(int id)
        {
            return PurchaseOrderRepository.Delete(id);
        }

        public int NextId()
        {
            return PurchaseOrderRepository.NextId();
        }

        public void Update(PurchaseOrder purchaseOrder)
        {
            PurchaseOrderRepository.Update(purchaseOrder);
        }
    }
}
