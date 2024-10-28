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
    public class PurchaseOrderItemService
    {
        public IPurchaseOrderItemRepository PurchaseOrderItemRepository { get; set; }

        public PurchaseOrderItemService(IPurchaseOrderItemRepository purchaseOrderItemRepository)
        {
            PurchaseOrderItemRepository = purchaseOrderItemRepository;
        }

        public static PurchaseOrderItemService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<PurchaseOrderItemService>();
        }

        public void Add(PurchaseOrderItem newPurchaseOrderItem)
        {
            PurchaseOrderItemRepository.Add(newPurchaseOrderItem);
        }

        public PurchaseOrderItem? GetById(int purchaseOrderId, int productId)
        {
            return PurchaseOrderItemRepository.GetById(purchaseOrderId, productId);
        }

        public List<PurchaseOrderItem> GetAll()
        {
            return PurchaseOrderItemRepository.GetAll();
        }
        public bool Delete(int purchaseOrderId, int productId)
        {
            return PurchaseOrderItemRepository.Delete(purchaseOrderId, productId);
        }
        public List<PurchaseOrderItem>? GetByPurchaseOrderId(int purchaseOrderId)
        {
            return PurchaseOrderItemRepository.GetByPurchaseOrderId(purchaseOrderId);
        }
    }
}
