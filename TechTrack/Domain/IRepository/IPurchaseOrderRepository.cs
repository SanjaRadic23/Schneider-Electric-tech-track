using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTrack.Domain.Model;

namespace TechTrack.Domain.IRepository
{
    public interface IPurchaseOrderRepository
    {
        List<PurchaseOrder> GetAll();
        void Add(PurchaseOrder purchaseOrder);
        PurchaseOrder? GetById(int Id);
        int NextId();
        bool Delete(int id);
        void Update(PurchaseOrder purchaseOrder);
    }
}
