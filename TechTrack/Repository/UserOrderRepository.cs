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
    public class UserOrderRepository : IUserOrderRepository
    {
        public int NextId()
        {
            using (IDbConnection connection = DatabaseConncectionPooling.GetConnection())
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT MAX(id_order) FROM UsersOrders";
                    return Convert.ToInt32(command.ExecuteScalar()) + 1;
                }
            }
        }

        public void Add(UserOrder userOrder)
        {
            int nextId = NextId();
            userOrder.IdOrder = nextId;

            string query = "INSERT INTO UsersOrders (id_order, creation_date, status, user_id) " +
                           "VALUES (:IdOrder, :CreationDate, :Status, :UserId)";

            using (IDbConnection connection = DatabaseConncectionPooling.GetConnection())
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    ParameterUtil.AddParameter(command, "IdOrder", DbType.Int32);
                    ParameterUtil.AddParameter(command, "CreationDate", DbType.Date);
                    ParameterUtil.AddParameter(command, "Status", DbType.String);
                    ParameterUtil.AddParameter(command, "UserId", DbType.Int32);

                    command.Prepare();

                    ParameterUtil.SetParameterValue(command, "IdOrder", userOrder.IdOrder);
                    ParameterUtil.SetParameterValue(command, "CreationDate", userOrder.CreationDate);
                    ParameterUtil.SetParameterValue(command, "Status", userOrder.Status);
                    ParameterUtil.SetParameterValue(command, "UserId", userOrder.UserId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public bool Delete(int id)
        {
            string query = "DELETE FROM UsersOrders WHERE id_order = :IdOrder";

            using (IDbConnection connection = DatabaseConncectionPooling.GetConnection())
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    ParameterUtil.AddParameter(command, "IdOrder", DbType.Int32);

                    command.Prepare();

                    ParameterUtil.SetParameterValue(command, "IdOrder", id);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<UserOrder> GetAll()
        {
            string query = "SELECT * FROM UsersOrders";
            List<UserOrder> orders = new List<UserOrder>();

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
                            UserOrder order = new UserOrder(
                                reader.GetInt32(0),
                                reader.GetDateTime(1),
                                reader.GetString(2),
                                reader.GetInt32(3)
                            );
                            orders.Add(order);
                        }
                    }
                }
            }
            return orders;
        }

        public UserOrder? GetById(int id)
        {
            string query = "SELECT * FROM UsersOrders WHERE id_order = :IdOrder";

            using (IDbConnection connection = DatabaseConncectionPooling.GetConnection())
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    ParameterUtil.AddParameter(command, "IdOrder", DbType.Int32);
                    command.Prepare();
                    ParameterUtil.SetParameterValue(command, "IdOrder", id);

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new UserOrder(
                                reader.GetInt32(0),
                                reader.GetDateTime(1),
                                reader.GetString(2),
                                reader.GetInt32(3)
                            );
                        }
                    }
                }
            }
            return null;
        }

        public void Update(UserOrder userOrder)
        {
            string query = @"
                            BEGIN
                                UPDATE UsersOrders
                                SET 
                                    creation_date = COALESCE(NULLIF(:CreationDate, TO_DATE('', 'YYYY-MM-DD')), creation_date),
                                    status = COALESCE(NULLIF(:Status, ''), status),
                                    user_id = COALESCE(NULLIF(:UserId, -1), user_id)
                                WHERE 
                                    id_order = :IdOrder;

                                COMMIT;
                            EXCEPTION
                                WHEN OTHERS THEN
                                    ROLLBACK;
                                    RAISE;
                            END;";

            using (IDbConnection connection = DatabaseConncectionPooling.GetConnection())
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    ParameterUtil.AddParameter(command, "CreationDate", DbType.Date);
                    ParameterUtil.AddParameter(command, "Status", DbType.String);
                    ParameterUtil.AddParameter(command, "UserId", DbType.Int32);
                    ParameterUtil.AddParameter(command, "IdOrder", DbType.Int32);

                    command.Prepare();

                    ParameterUtil.SetParameterValue(command, "CreationDate", userOrder.CreationDate);
                    ParameterUtil.SetParameterValue(command, "Status", userOrder.Status);
                    ParameterUtil.SetParameterValue(command, "UserId", userOrder.UserId);
                    ParameterUtil.SetParameterValue(command, "IdOrder", userOrder.IdOrder);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
