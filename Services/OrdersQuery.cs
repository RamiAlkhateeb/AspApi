using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;
using System;

namespace ProductsAPI
{
    public class OrdersQuery
    {
        public AppDb Db { get; }

        public OrdersQuery(AppDb db)
        {
            Db = db;
        }


        public async Task<Order> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM `orders` WHERE `Id` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<OrderInfo> FindOrderInfoAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT u.Name , p.Name , o.Date FROM `orders` o 
            , `users` u , `products` p WHERE o.Id = @id and o.UserId = u.Id 
            and o.ProductId = p.Id ";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadOneAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Order>> LatestUsersAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM `orders` ORDER BY `Id` DESC LIMIT 10;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `orders`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<Order>> ReadAllAsync(DbDataReader reader)
        {
            var orders = new List<Order>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var order = new Order(Db)
                    {
                        ID = reader.GetInt32(0),
                        UserId = reader.GetInt32(1),
                        ProductId = reader.GetInt32(2),
                        Date = reader.GetDateTime(3)
                    };
                    orders.Add(order);
                }
            }
            return orders;
        }

        private async Task<List<OrderInfo>> ReadOneAsync(DbDataReader reader)
        {
            var orders = new List<OrderInfo>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var order = new OrderInfo(Db)
                    {
                        UserName = reader.GetString(0),
                        ProductName = reader.GetString(1),
                        Date = reader.GetDateTime(2)
                    };
                    orders.Add(order);
                }
            }
            return orders;
        }
    }

    public class OrderInfo {

        internal OrderInfo(AppDb db)
        {
            Db = db;
        }
        public String UserName { get; set; }
        public String ProductName { get; set; }
        public DateTime Date { get; set; }
        internal AppDb Db { get; set; }
    }
}