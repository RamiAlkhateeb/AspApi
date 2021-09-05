using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

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
            cmd.CommandText = @"SELECT `Id`, `Name`, `Address`,`Age` FROM `users` WHERE `Id` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
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
            cmd.CommandText = @"DELETE FROM `BlogPost`";
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
    }
}