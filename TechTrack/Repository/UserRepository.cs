using Microsoft.Extensions.DependencyInjection;
using Oracle.ManagedDataAccess.Client;
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
    public class UserRepository : IUserRepository
    {
        public static UserRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<UserRepository>();
        }
        public UserRepository() { 

           
        }
        public int NextId()
        {
            using (IDbConnection connection = DatabaseConncectionPooling.GetConnection())
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT MAX(id_user) FROM Users"; // Dobijanje najveseg Id-a

                    return Convert.ToInt32(command.ExecuteScalar()) + 1; // Vraca sledeci ID
                }
            }
        }

        public void Add(User user)
        {
            
            int nextId = NextId();
            user.IdUser = nextId; 

            string query = "INSERT INTO Users (id_user, first_name, last_name, phone_number, email, role) " +
                           "VALUES (:IdUser, :FirstName, :LastName, :PhoneNumber, :Email, :Role)";

            using (IDbConnection connection = DatabaseConncectionPooling.GetConnection())
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    
                    ParameterUtil.AddParameter(command, "IdUser", DbType.Int32);
                    ParameterUtil.AddParameter(command, "FirstName", DbType.String);
                    ParameterUtil.AddParameter(command, "LastName", DbType.String);
                    ParameterUtil.AddParameter(command, "PhoneNumber", DbType.String);
                    ParameterUtil.AddParameter(command, "Email", DbType.String);
                    ParameterUtil.AddParameter(command, "Role", DbType.String);

                    command.Prepare();

                    
                    ParameterUtil.SetParameterValue(command, "IdUser", user.IdUser);
                    ParameterUtil.SetParameterValue(command, "FirstName", user.FirstName);
                    ParameterUtil.SetParameterValue(command, "LastName", user.LastName);
                    ParameterUtil.SetParameterValue(command, "PhoneNumber", user.PhoneNumber);
                    ParameterUtil.SetParameterValue(command, "Email", user.Email);
                    ParameterUtil.SetParameterValue(command, "Role", user.Role);

                    command.ExecuteNonQuery();
                }
            }
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAll()
        {
            string query = "SELECT * " +
                            "FROM Users";

            List<User> list = new List<User>();

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
                            User u = new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5));

                            list.Add(u);
                        }
                    }
                }
            }

            return list;
        }

        public User? GetById(int Id)
        {
            throw new NotImplementedException();
        }

    }
}
