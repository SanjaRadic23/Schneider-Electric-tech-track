using Microsoft.Extensions.DependencyInjection;
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
    public class SupplierRepository : ISupplierRepository
    {
        public static SupplierRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<SupplierRepository>();
        }

        public SupplierRepository() { }

        public void Add(Supplier supplier)
        {
            int nextId = NextId();

            string query = "INSERT INTO Suppliers (id_supplier, name, phone_number, email, address) " +
                           "VALUES (:Id, :Name, :PhoneNumber, :Email, :Address)";

            using (IDbConnection conn = DatabaseConncectionPooling.GetConnection())
            {
                conn.Open();

                using (IDbCommand command = conn.CreateCommand())
                {
                    command.CommandText = query;

                    ParameterUtil.AddParameter(command, "Id", DbType.Int32);
                    ParameterUtil.AddParameter(command, "Name", DbType.String);
                    ParameterUtil.AddParameter(command, "PhoneNumber", DbType.String);
                    ParameterUtil.AddParameter(command, "Email", DbType.String);
                    ParameterUtil.AddParameter(command, "Address", DbType.String);

                    command.Prepare();

                    ParameterUtil.SetParameterValue(command, "Id", nextId);
                    ParameterUtil.SetParameterValue(command, "Name", supplier.Name);
                    ParameterUtil.SetParameterValue(command, "PhoneNumber", supplier.PhoneNumber);
                    ParameterUtil.SetParameterValue(command, "Email", supplier.Email);
                    ParameterUtil.SetParameterValue(command, "Address", supplier.Address);

                    command.ExecuteNonQuery();
                }
            }
        }

        public bool Delete(int id)
        {
            string query = "DELETE FROM Suppliers WHERE id_supplier = :Id";

            using (IDbConnection conn = DatabaseConncectionPooling.GetConnection())
            {
                conn.Open();

                using (IDbCommand command = conn.CreateCommand())
                {
                    command.CommandText = query;

                    ParameterUtil.AddParameter(command, "Id", DbType.Int32);
                    ParameterUtil.SetParameterValue(command, "Id", id);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<Supplier> GetAll()
        {
            List<Supplier> suppliers = new List<Supplier>();

            string query = "SELECT * FROM Suppliers";

            using (IDbConnection conn = DatabaseConncectionPooling.GetConnection())
            {
                conn.Open();

                using (IDbCommand command = conn.CreateCommand())
                {
                    command.CommandText = query;

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            suppliers.Add(new Supplier(
                                reader.GetInt32(0),
                                reader.GetString(1),  
                                reader.GetString(2),  
                                reader.GetString(3),  
                                reader.GetString(4)   
                            ));
                        }
                    }
                }
            }

            return suppliers;
        }

        public Supplier? GetById(int Id)
        {
            Supplier? supplier = null;

            string query = "SELECT * FROM Suppliers WHERE id_supplier = :Id";

            using (IDbConnection conn = DatabaseConncectionPooling.GetConnection())
            {
                conn.Open();

                using (IDbCommand command = conn.CreateCommand())
                {
                    command.CommandText = query;

                    ParameterUtil.AddParameter(command, "Id", DbType.Int32);
                    ParameterUtil.SetParameterValue(command, "Id", Id);

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            supplier = new Supplier(
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetString(3),
                                reader.GetString(4)
                            );
                        }
                    }
                }
            }

            return supplier;
        }

        public int NextId()
        {
            using (IDbConnection connection = DatabaseConncectionPooling.GetConnection())
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT MAX(id_supplier) FROM Suppliers";

                    return Convert.ToInt32(command.ExecuteScalar()) + 1;
                }
            }
        }
    }
}
