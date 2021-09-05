using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

namespace ProductsAPI
{
    public class ProductsQuery
    {
        public AppDb Db { get; }

        public ProductsQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<Product> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Id`, `Name`, `Category`,`Price` FROM `products` WHERE `Id` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Product>> LatestPostsAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Id` , `Name` , `Category` ,`Price` FROM `products` ORDER BY `Id` DESC LIMIT 10;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `BlogPost`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<Product>> ReadAllAsync(DbDataReader reader)
        {
            var products = new List<Product>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var product = new Product(Db)
                    {
                        ID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Category = reader.GetString(2) ,
                        Price = reader.GetInt32(3)
                    };
                    products.Add(product);
                }
            }
            return products;
        }
    }
}