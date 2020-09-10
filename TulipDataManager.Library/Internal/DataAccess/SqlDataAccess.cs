using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TulipDataManager.Library.Models;

namespace TulipDataManager.Library.Internal.DataAccess
{
    internal class SqlDataAccess
    {
        public string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(storedProcedure, 
                    parameters, commandType: CommandType.StoredProcedure).ToList();

                return rows;
            }
        }


        public void SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                
                connection.Execute(storedProcedure,
                        parameters, commandType: CommandType.StoredProcedure);
     
            }
        }

        public int CreateProduct(string storedProcedure, ProductModel product, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var p = new DynamicParameters();
                p.Add("@ProductName", product.ProductName);
                p.Add("@Description", product.Description);
                p.Add("@ProductImage", product.ProductImage);
                p.Add("@RetailPrice", product.RetailPrice);
                p.Add("@QuantityInStock", product.QuantityInStock);
                p.Add("@IsTaxable", product.IsTaxable);
                p.Add("@Sex", product.Sex);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute(storedProcedure,
                        p, commandType: CommandType.StoredProcedure);

                int newId = p.Get<int>("@id");
                return newId;
            }
        }

        public int CreateOrder(string storedProcedure, OrderModel order, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var p = new DynamicParameters();
                p.Add("@UserId", order.UserId);
                p.Add("@SubTotal", order.SubTotal);
                p.Add("@Tax", order.Tax);
                p.Add("@Total", order.Total);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute(storedProcedure,
                        p, commandType: CommandType.StoredProcedure);

                int newId = p.Get<int>("@id");
                return newId;
            }
        }
    }
}
