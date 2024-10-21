using Microsoft.Extensions.DependencyInjection;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using TechTrack.Domain.IRepository;
using TechTrack.Domain.Model;
using TechTrack.Helpers;

namespace TechTrack.Repository
{
    public class UserAccountRepository : IUserAccountRepository
    {
       

        public static UserAccountRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<UserAccountRepository>();
        }

        public UserAccountRepository()
        {
            
        }

        public int NextId()
        {
            using (IDbConnection connection = DatabaseConncectionPooling.GetConnection())
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT MAX(id_account) FROM UserAccounts"; // Dobijanje najveseg Id-a

                    return Convert.ToInt32(command.ExecuteScalar()) + 1; // Vraca sledeci ID
                }
            }
        }

        public void Add(UserAccount userAccount)
        {
            int nextId = NextId();

            string query = "INSERT INTO UserAccounts (id_account, username, password, user_id) " +
                           "VALUES (:Id, :Username, :Password, :UserId)";

            using (IDbConnection conn = DatabaseConncectionPooling.GetConnection())
            {
                conn.Open();

                using (IDbCommand command = conn.CreateCommand())
                {
                    command.CommandText = query;

                    
                    ParameterUtil.AddParameter(command, "Id", DbType.Int32);
                    ParameterUtil.AddParameter(command, "Username", DbType.String);
                    ParameterUtil.AddParameter(command, "Password", DbType.String);
                    ParameterUtil.AddParameter(command, "UserId", DbType.Int32);

                    command.Prepare();

                    
                    ParameterUtil.SetParameterValue(command, "Id", nextId);
                    ParameterUtil.SetParameterValue(command, "Username", userAccount.Username);
                    ParameterUtil.SetParameterValue(command, "Password", PasswordHasher.HashPassword(userAccount.Password));
                    ParameterUtil.SetParameterValue(command, "UserId", userAccount.UserId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<UserAccount> GetAll()
        {
            List<UserAccount> list = new List<UserAccount>();

            using (IDbConnection conn = DatabaseConncectionPooling.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM UserAccounts";

                using (IDbCommand command = conn.CreateCommand())
                {
                    command.CommandText = query;
                    command.Prepare();

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new UserAccount(
                                reader.IsDBNull(0) ? 0 : reader.GetInt32(0), 
                                reader.IsDBNull(1) ? string.Empty : reader.GetString(1), 
                                reader.IsDBNull(2) ? string.Empty : reader.GetString(2), 
                                reader.IsDBNull(3) ? 0 : reader.GetInt32(3) 
                            ));
                        }
                    }
                }
            }

            return list;
        }

        public UserAccount? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public UserAccount GetByUsername(string username)
        {
            UserAccount userAccount = null;
            
            using (IDbConnection conn = DatabaseConncectionPooling.GetConnection())
            {
                try
                {
                    conn.Open();
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }

                string query = "SELECT * FROM UserAccounts WHERE username = :Username";

                using (IDbCommand command = conn.CreateCommand())
                {
                    command.CommandText = query;
                    ParameterUtil.AddParameter(command, "Username", DbType.String);
                    ParameterUtil.SetParameterValue(command, "Username", username);

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userAccount = new UserAccount(
                                reader.GetInt32(0), 
                                reader.GetString(1), 
                                reader.GetString(2), 
                                reader.GetInt32(3) 
                            );
                        }
                    }
                }
            }

            return userAccount;
        }
    }
}
