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
    public class UserOrderItemRepository : IUserOrderItemRepository
    {
        public int NextId()
        {
            using (IDbConnection connection = DatabaseConncectionPooling.GetConnection())
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT MAX(user_order_id) FROM UsersOrderItems";
                    return Convert.ToInt32(command.ExecuteScalar()) + 1;
                }
            }
        }

        public void Add(UserOrderItem userOrderItem)
        {
            string query = "INSERT INTO UsersOrderItems (user_order_id, product_id, quantity, total_price) " +
                           "VALUES (:UserOrderId, :ProductId, :Quantity, :TotalPrice)";

            using (IDbConnection connection = DatabaseConncectionPooling.GetConnection())
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    ParameterUtil.AddParameter(command, "UserOrderId", DbType.Int32);
                    ParameterUtil.AddParameter(command, "ProductId", DbType.Int32);
                    ParameterUtil.AddParameter(command, "Quantity", DbType.Int32);
                    ParameterUtil.AddParameter(command, "TotalPrice", DbType.Decimal);

                    command.Prepare();

                    ParameterUtil.SetParameterValue(command, "UserOrderId", userOrderItem.UserOrderId);
                    ParameterUtil.SetParameterValue(command, "ProductId", userOrderItem.ProductId);
                    ParameterUtil.SetParameterValue(command, "Quantity", userOrderItem.Quantity);
                    ParameterUtil.SetParameterValue(command, "TotalPrice", userOrderItem.TotalPrice);

                    command.ExecuteNonQuery();
                }
            }
        }

        public bool Delete(int userOrderId, int productId)
        {
            string query = "DELETE FROM UsersOrderItems WHERE user_order_id = :UserOrderId AND product_id = :ProductId";

            using (IDbConnection connection = DatabaseConncectionPooling.GetConnection())
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    ParameterUtil.AddParameter(command, "UserOrderId", DbType.Int32);
                    ParameterUtil.AddParameter(command, "ProductId", DbType.Int32);

                    command.Prepare();

                    ParameterUtil.SetParameterValue(command, "UserOrderId", userOrderId);
                    ParameterUtil.SetParameterValue(command, "ProductId", productId);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<UserOrderItem> GetAll()
        {
            string query = "SELECT * FROM UsersOrderItems";
            List<UserOrderItem> items = new List<UserOrderItem>();

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
                            UserOrderItem item = new UserOrderItem(
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

        public UserOrderItem? GetById(int userOrderId, int productId)
        {
            string query = "SELECT * FROM UsersOrderItems WHERE user_order_id = :UserOrderId AND product_id = :ProductId";

            using (IDbConnection connection = DatabaseConncectionPooling.GetConnection())
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    ParameterUtil.AddParameter(command, "UserOrderId", DbType.Int32);
                    ParameterUtil.AddParameter(command, "ProductId", DbType.Int32);

                    command.Prepare();

                    ParameterUtil.SetParameterValue(command, "UserOrderId", userOrderId);
                    ParameterUtil.SetParameterValue(command, "ProductId", productId);

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new UserOrderItem(
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
        public List<UserOrderItem>? GetByUserOrderId(int userOrderId)
        {
            string query = "SELECT * FROM UsersOrderItems WHERE user_order_id = :UserOrderId";
            List<UserOrderItem> items = new List<UserOrderItem>();

            using (IDbConnection connection = DatabaseConncectionPooling.GetConnection())
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    ParameterUtil.AddParameter(command, "UserOrderId", DbType.Int32);
                    command.Prepare();
                    ParameterUtil.SetParameterValue(command, "UserOrderId", userOrderId);

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new UserOrderItem(
                                reader.GetInt32(0),
                                reader.GetInt32(1),
                                reader.GetInt32(2),
                                reader.GetDecimal(3)
                            ));
                        }
                    }
                }
            }

            return items.Count > 0 ? items : null; 
        }
    }
}
