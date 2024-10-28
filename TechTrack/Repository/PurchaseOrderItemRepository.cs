using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTrack.Domain.IRepository;
using TechTrack.Domain.Model;
using TechTrack.Helpers;

namespace TechTrack.Repository
{
    public class PurchaseOrderItemRepository : IPurchaseOrderItemRepository
    {
        public int NextId()
        {
            using (IDbConnection connection = DatabaseConncectionPooling.GetConnection())
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT MAX(purchase_order_id) FROM PurchasesOrderItems";
                    return Convert.ToInt32(command.ExecuteScalar()) + 1;
                }
            }
        }

        public void Add(PurchaseOrderItem purchaseOrderItem)
        {
            string query = "INSERT INTO PurchasesOrderItems (purchase_order_id, product_id, quantity, total_price) " +
                           "VALUES (:PurchaseOrderId, :ProductId, :Quantity, :TotalPrice)";

            using (IDbConnection connection = DatabaseConncectionPooling.GetConnection())
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    ParameterUtil.AddParameter(command, "PurchaseOrderId", DbType.Int32);
                    ParameterUtil.AddParameter(command, "ProductId", DbType.Int32);
                    ParameterUtil.AddParameter(command, "Quantity", DbType.Int32);
                    ParameterUtil.AddParameter(command, "TotalPrice", DbType.Decimal);

                    command.Prepare();

                    ParameterUtil.SetParameterValue(command, "PurchaseOrderId", purchaseOrderItem.PurchaseOrderId);
                    ParameterUtil.SetParameterValue(command, "ProductId", purchaseOrderItem.ProductId);
                    ParameterUtil.SetParameterValue(command, "Quantity", purchaseOrderItem.Quantity);
                    ParameterUtil.SetParameterValue(command, "TotalPrice", purchaseOrderItem.TotalPrice);

                    command.ExecuteNonQuery();
                }
            }
        }

        public bool Delete(int purchaseOrderId, int productId)
        {
            string query = "DELETE FROM PurchasesOrderItems WHERE purchase_order_id = :PurchaseOrderId AND product_id = :ProductId";

            using (IDbConnection connection = DatabaseConncectionPooling.GetConnection())
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    ParameterUtil.AddParameter(command, "PurchaseOrderId", DbType.Int32);
                    ParameterUtil.AddParameter(command, "ProductId", DbType.Int32);

                    command.Prepare();

                    ParameterUtil.SetParameterValue(command, "PurchaseOrderId", purchaseOrderId);
                    ParameterUtil.SetParameterValue(command, "ProductId", productId);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<PurchaseOrderItem> GetAll()
        {
            string query = "SELECT * FROM PurchasesOrderItems";
            List<PurchaseOrderItem> items = new List<PurchaseOrderItem>();

            using (IDbConnection connection = DatabaseConncectionPooling.GetConnection())
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Prepare();

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PurchaseOrderItem item = new PurchaseOrderItem(
                                reader.GetInt32(0),
                                reader.GetInt32(1),
                                reader.GetInt32(2),
                                reader.GetDecimal(3)
                            );
                            items.Add(item);
                        }
                    }
                }
            }
            return items;
        }

        public PurchaseOrderItem? GetById(int purchaseOrderId, int productId)
        {
            string query = "SELECT * FROM PurchaseOrderItems WHERE purchase_order_id = :PurchaseOrderId AND product_id = :ProductId";

            using (IDbConnection connection = DatabaseConncectionPooling.GetConnection())
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    ParameterUtil.AddParameter(command, "PurchaseOrderId", DbType.Int32);
                    ParameterUtil.AddParameter(command, "ProductId", DbType.Int32);

                    command.Prepare();

                    ParameterUtil.SetParameterValue(command, "PurchaseOrderId", purchaseOrderId);
                    ParameterUtil.SetParameterValue(command, "ProductId", productId);

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new PurchaseOrderItem(
                                reader.GetInt32(0),
                                reader.GetInt32(1),
                                reader.GetInt32(2),
                                reader.GetDecimal(3)
                            );
                        }
                    }
                }
            }
            return null;
        }

        public List<PurchaseOrderItem>? GetByPurchaseOrderId(int purchaseOrderId)
        {
            string query = "SELECT * FROM PurchasesOrderItems WHERE purchase_order_id = :PurchaseOrderId";
            List<PurchaseOrderItem> items = new List<PurchaseOrderItem>();

            using (IDbConnection connection = DatabaseConncectionPooling.GetConnection())
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    ParameterUtil.AddParameter(command, "PurchaseOrderId", DbType.Int32);
                    command.Prepare();
                    ParameterUtil.SetParameterValue(command, "PurchaseOrderId", purchaseOrderId);

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new PurchaseOrderItem(
                                reader.GetInt32(0), 
                                reader.GetInt32(1), 
                                reader.GetInt32(2), 
                                reader.GetDecimal(3) 
                            ));
                        }
                    }
                }
            }

            return items.Count > 0 ? items : null; // Return the list or null if empty
        }
    }
}
