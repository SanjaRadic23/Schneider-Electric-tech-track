using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTrack.Domain.Model
{
    public class PurchaseOrder
    {
        public int IdOrder { get; set; }
        public DateTime CreationDate { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }

        public PurchaseOrder() { }

        public PurchaseOrder(int idOrder, DateTime creationDate, string status, int userId)
        {
            IdOrder = idOrder;
            CreationDate = creationDate;
            Status = status;
            UserId = userId;
        }
    }
}
