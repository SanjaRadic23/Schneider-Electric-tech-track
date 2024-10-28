using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTrack.Domain.Model;

namespace TechTrack.Domain.IRepository
{
    public interface IPurchaseOrderItemRepository
    {
        List<PurchaseOrderItem> GetAll();
        void Add(PurchaseOrderItem purchaseOrderItem);
        PurchaseOrderItem? GetById(int purchaseOrderId, int productId);
        int NextId();
        bool Delete(int purchaseOrderId, int productId);
        List<PurchaseOrderItem>? GetByPurchaseOrderId(int purchaseOrderId);
    }
}
