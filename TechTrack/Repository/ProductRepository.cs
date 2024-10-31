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
    public class ProductRepository : IProductRepository
    {
        public static ProductRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<ProductRepository>();
        }

        public ProductRepository()
        {

        }
        public void Add(Product product)
        {
            int nextId = NextId();

            string query = "INSERT INTO TechProducts (id_product, name, description, quantity, price, supplier_id) " +
                           "VALUES (:Id, :Name, :Description, :Quantity, :Price, :SupplierId)";

            using (IDbConnection conn = DatabaseConncectionPooling.GetConnection())
            {
                conn.Open();

                using (IDbCommand command = conn.CreateCommand())
                {
                    command.CommandText = query;

                    ParameterUtil.AddParameter(command, "Id", DbType.Int32);
                    ParameterUtil.AddParameter(command, "Name", DbType.String);
                    ParameterUtil.AddParameter(command, "Description", DbType.String);
                    ParameterUtil.AddParameter(command, "Quantity", DbType.Int32);
                    ParameterUtil.AddParameter(command, "Price", DbType.Decimal);
                    ParameterUtil.AddParameter(command, "SupplierId", DbType.Int32);

                    command.Prepare();

                    ParameterUtil.SetParameterValue(command, "Id", nextId);
                    ParameterUtil.SetParameterValue(command, "Name", product.Name);
                    ParameterUtil.SetParameterValue(command, "Description", product.Description);
                    ParameterUtil.SetParameterValue(command, "Quantity", product.Quantity);
                    ParameterUtil.SetParameterValue(command, "Price", product.Price);
                    ParameterUtil.SetParameterValue(command, "SupplierId", product.SupplierId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public bool Delete(int id)
        {
            string query = @"
                            DECLARE
                                v_product_id INTEGER := :Id;
                                v_user_order_ids SYS.ODCINumberList;
                                v_purchase_order_ids SYS.ODCINumberList;
                            BEGIN
                                SELECT user_order_id BULK COLLECT INTO v_user_order_ids
                                FROM UsersOrderItems
                                WHERE product_id = v_product_id;

                                SELECT purchase_order_id BULK COLLECT INTO v_purchase_order_ids
                                FROM PurchasesOrderItems
                                WHERE product_id = v_product_id;

                                DELETE FROM UsersOrderItems 
                                WHERE product_id = v_product_id;

                                DELETE FROM UsersOrders 
                                WHERE id_order IN (SELECT * FROM TABLE(v_user_order_ids));

                                DELETE FROM PurchasesOrderItems 
                                WHERE product_id = v_product_id;

                                DELETE FROM PurchasesOrders 
                                WHERE id_order IN (SELECT * FROM TABLE(v_purchase_order_ids));

                                DELETE FROM TechProducts WHERE id_product = v_product_id;

                                COMMIT;
                            EXCEPTION
                                WHEN OTHERS THEN
                                    ROLLBACK;
                                    RAISE;
                            END;";

            using (IDbConnection conn = DatabaseConncectionPooling.GetConnection())
            {
                conn.Open();

                using (IDbCommand command = conn.CreateCommand())
                {
                    command.CommandText = query;

                    ParameterUtil.AddParameter(command, "Id", DbType.Int32);
                    ParameterUtil.SetParameterValue(command, "Id", id);

                    try
                    {
                        return command.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {

                        return false;
                    }
                }
            }
        }

        public List<Product> GetAll()
        {
            List<Product> products = new List<Product>();

            string query = "SELECT * FROM TechProducts";

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
                            products.Add(new Product(
                                reader.GetInt32(0), 
                                reader.GetString(1), 
                                reader.GetString(2),
                                reader.GetInt32(3), 
                                reader.GetDecimal(4), 
                                reader.GetInt32(5) 
                            ));
                        }
                    }
                }
            }

            return products;
        }

        public Product? GetBySupplierId(int Id)
        {
            Product? product = null;

            string query = "SELECT * FROM TechProducts WHERE supplier_id = :Id";

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
                            product = new Product(
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetInt32(3),
                                reader.GetDecimal(4),
                                reader.GetInt32(5)
                            );
                        }
                    }
                }
            }

            return product;
        }
        public Product? GetById(int Id)
        {
            Product? product = null;

            string query = "SELECT * FROM TechProducts WHERE id_product = :Id";

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
                            product = new Product(
                                reader.GetInt32(0), 
                                reader.GetString(1), 
                                reader.GetString(2),
                                reader.GetInt32(3),
                                reader.GetDecimal(4),
                                reader.GetInt32(5)
                            );
                        }
                    }
                }
            }

            return product;
        }

        public int NextId()
        {
            using (IDbConnection connection = DatabaseConncectionPooling.GetConnection())
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT MAX(id_product) FROM TechProducts"; 

                    return Convert.ToInt32(command.ExecuteScalar()) + 1; 
                }
            }
        }

        public Product? GetByNameAndSupplierId(string name, int id)
        {
            Product? product = null;

            string query = "SELECT * FROM TechProducts WHERE name = :Name AND supplier_id = :SupplierId";

            using (IDbConnection conn = DatabaseConncectionPooling.GetConnection())
            {
                conn.Open();

                using (IDbCommand command = conn.CreateCommand())
                {
                    command.CommandText = query;

                    ParameterUtil.AddParameter(command, "Name", DbType.String);
                    ParameterUtil.SetParameterValue(command, "Name", name);

                    ParameterUtil.AddParameter(command, "SupplierId", DbType.Int32);
                    ParameterUtil.SetParameterValue(command, "SupplierId", id);

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            product = new Product(
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetInt32(3),
                                reader.GetDecimal(4),
                                reader.GetInt32(5)
                            );
                        }
                    }
                }
            }

            return product;
        }
        public List<Product> Search(string searchTerm)
        {
            List<Product> products = new List<Product>();

            string query = @"SELECT * FROM TechProducts 
                                WHERE LOWER(name) LIKE '%' || :SearchTerm || '%' 
                                OR LOWER(description) LIKE '%' || :SearchTerm || '%' 
                                OR LOWER(CAST(quantity AS VARCHAR2(50))) LIKE '%' || :SearchTerm || '%' 
                                OR LOWER(CAST(price AS VARCHAR2(50))) LIKE '%' || :SearchTerm || '%'";

            using (IDbConnection conn = DatabaseConncectionPooling.GetConnection())
            {
                conn.Open();

                using (IDbCommand command = conn.CreateCommand())
                {
                    command.CommandText = query;

                    ParameterUtil.AddParameter(command, "SearchTerm", DbType.String);
                    ParameterUtil.SetParameterValue(command, "SearchTerm", searchTerm.ToLower());

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product(
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetInt32(3),
                                reader.GetDecimal(4),
                                reader.GetInt32(5)
                            ));
                        }
                    }
                }
            }

            return products;
        }
        public void Update(Product product)
        {
            string query = @"
                            BEGIN
                                UPDATE TechProducts 
                                SET 
                                    name = COALESCE(NULLIF(:Name, ''), name),
                                    description = COALESCE(NULLIF(:Description, ''), description),
                                    quantity = COALESCE(NULLIF(:Quantity, NULL), quantity),  
                                    price = COALESCE(NULLIF(:Price, NULL), price),          
                                    supplier_id = COALESCE(NULLIF(:SupplierId, NULL), supplier_id) 
                                WHERE 
                                    id_product = :Id;

                                COMMIT;
                            EXCEPTION
                                WHEN OTHERS THEN
                                    ROLLBACK;
                                    RAISE;
                            END;";

            using (IDbConnection conn = DatabaseConncectionPooling.GetConnection())
            {
                conn.Open();

                using (IDbCommand command = conn.CreateCommand())
                {
                    command.CommandText = query;

                    ParameterUtil.AddParameter(command, "Name", DbType.String);
                    ParameterUtil.AddParameter(command, "Description", DbType.String);
                    ParameterUtil.AddParameter(command, "Quantity", DbType.Int32);
                    ParameterUtil.AddParameter(command, "Price", DbType.Decimal);
                    ParameterUtil.AddParameter(command, "SupplierId", DbType.Int32);
                    ParameterUtil.AddParameter(command, "Id", DbType.Int32);

                    ParameterUtil.SetParameterValue(command, "Name", product.Name);
                    ParameterUtil.SetParameterValue(command, "Description", product.Description);
                    ParameterUtil.SetParameterValue(command, "Quantity", product.Quantity);
                    ParameterUtil.SetParameterValue(command, "Price", product.Price);
                    ParameterUtil.SetParameterValue(command, "SupplierId", product.SupplierId);
                    ParameterUtil.SetParameterValue(command, "Id", product.IdProduct);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
