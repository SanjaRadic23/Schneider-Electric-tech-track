using Microsoft.Extensions.DependencyInjection;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
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
    public class ReportingRepository : IReportingRepository
    {
        public static ReportingRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<ReportingRepository>();
        }

        public ReportingRepository()
        {
        }
        public List<OrderReport> GetMonthlyOrderStatistics()
        {
            List<OrderReport> reports = new List<OrderReport>();

            using (IDbConnection connection = DatabaseConncectionPooling.GetConnection())
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = "BEGIN :result_cursor := GetMonthlyOrderStatistics; END;";
                    command.CommandType = CommandType.Text; 

                    OracleParameter cursorParam = new OracleParameter
                    {
                        ParameterName = "result_cursor",
                        OracleDbType = OracleDbType.RefCursor,
                        Direction = ParameterDirection.Output
                    };

                    command.Parameters.Add(cursorParam);

                    command.ExecuteNonQuery();

                    using (var reader = (OracleRefCursor)cursorParam.Value)
                    {
                        using (OracleDataReader dataReader = reader.GetDataReader())
                        {
                            while (dataReader.Read())
                            {
                                reports.Add(new OrderReport
                                {
                                    Period = dataReader.GetString(0),
                                    TotalOrders = dataReader.GetInt32(1),
                                    TotalRevenue = dataReader.GetDecimal(2),
                                    AverageOrderValue = dataReader.GetDecimal(3),
                                });
                            }
                        }
                    }
                }
            }

            return reports;
        }

        public List<OrderReport> GetQuarterlyOrderStatistics()
        {
            List<OrderReport> reports = new List<OrderReport>();

            using (IDbConnection connection = DatabaseConncectionPooling.GetConnection())
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = "BEGIN :result_cursor := GetQuarterlyOrderStatistics; END;";
                    command.CommandType = CommandType.Text;

                    OracleParameter cursorParam = new OracleParameter
                    {
                        ParameterName = "result_cursor",
                        OracleDbType = OracleDbType.RefCursor,
                        Direction = ParameterDirection.Output
                    };

                    command.Parameters.Add(cursorParam);
                    command.ExecuteNonQuery();

                    using (var reader = (OracleRefCursor)cursorParam.Value)
                    {
                        using (OracleDataReader dataReader = reader.GetDataReader())
                        {
                            while (dataReader.Read())
                            {
                                reports.Add(new OrderReport
                                {
                                    Period = dataReader.GetString(0),
                                    TotalOrders = dataReader.GetInt32(1),
                                    TotalRevenue = dataReader.GetDecimal(2),
                                    AverageOrderValue = dataReader.GetDecimal(3),
                                });
                            }
                        }
                    }
                }
            }

            return reports;
        }

        public List<InventoryReport> GetStockAndTotalValue()
        {
            List<InventoryReport> reports = new List<InventoryReport>();

            using (IDbConnection connection = DatabaseConncectionPooling.GetConnection())
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = "BEGIN :result_cursor := GetStockAndTotalValue; END;";
                    command.CommandType = CommandType.Text;

                    OracleParameter cursorParam = new OracleParameter
                    {
                        ParameterName = "result_cursor",
                        OracleDbType = OracleDbType.RefCursor,
                        Direction = ParameterDirection.Output
                    };

                    command.Parameters.Add(cursorParam);
                    command.ExecuteNonQuery();

                    using (var reader = (OracleRefCursor)cursorParam.Value)
                    {
                        using (OracleDataReader dataReader = reader.GetDataReader())
                        {
                            while (dataReader.Read())
                            {
                                reports.Add(new InventoryReport
                                {
                                    ProductName = dataReader.GetString(0),
                                    Quantity = dataReader.GetInt32(1),
                                    TotalValue = dataReader.GetDecimal(2),
                                });
                            }
                        }
                    }
                }
            }

            return reports;
        }

        public List<BestSellingProduct> GetTopSoldProductsLast7Days()
        {
            List<BestSellingProduct> reports = new List<BestSellingProduct>();

            using (IDbConnection connection = DatabaseConncectionPooling.GetConnection())
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = "BEGIN :result_cursor := GetTopSoldProductsLast7Days; END;";
                    command.CommandType = CommandType.Text;

                    OracleParameter cursorParam = new OracleParameter
                    {
                        ParameterName = "result_cursor",
                        OracleDbType = OracleDbType.RefCursor,
                        Direction = ParameterDirection.Output
                    };

                    command.Parameters.Add(cursorParam);
                    command.ExecuteNonQuery();

                    using (var reader = (OracleRefCursor)cursorParam.Value)
                    {
                        using (OracleDataReader dataReader = reader.GetDataReader())
                        {
                            while (dataReader.Read())
                            {
                                reports.Add(new BestSellingProduct
                                {
                                    ProductName = dataReader.GetString(0),
                                    TotalQuantity = dataReader.GetInt32(1),
                                });
                            }
                        }
                    }
                }
            }

            return reports;
        }

        public List<OrderReport> GetYearlyOrderStatistics()
        {
            List<OrderReport> reports = new List<OrderReport>();

            using (IDbConnection connection = DatabaseConncectionPooling.GetConnection())
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = "BEGIN :result_cursor := GetYearlyOrderStatistics; END;";
                    command.CommandType = CommandType.Text;

                    OracleParameter cursorParam = new OracleParameter
                    {
                        ParameterName = "result_cursor",
                        OracleDbType = OracleDbType.RefCursor,
                        Direction = ParameterDirection.Output
                    };

                    command.Parameters.Add(cursorParam);
                    command.ExecuteNonQuery();

                    using (var reader = (OracleRefCursor)cursorParam.Value)
                    {
                        using (OracleDataReader dataReader = reader.GetDataReader())
                        {
                            while (dataReader.Read())
                            {
                                reports.Add(new OrderReport
                                {
                                    Period = dataReader.GetString(0),
                                    TotalOrders = dataReader.GetInt32(1),
                                    TotalRevenue = dataReader.GetDecimal(2),
                                    AverageOrderValue = dataReader.GetDecimal(3),
                                });
                            }
                        }
                    }
                }
            }

            return reports;
        }
    }
}
