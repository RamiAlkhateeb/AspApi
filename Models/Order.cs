using System;
using System.Threading.Tasks;
using MySqlConnector;
using System.Data;

namespace ProductsAPI
{
    public class Order
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        internal AppDb Db { get; set; }

        public Order()
        {
        }

        internal Order(AppDb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `orders` (`UserId`, `ProductId` , `Date`) VALUES (@UserId, @ProductId, @Date);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            ID = (int) cmd.LastInsertedId;
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `users` SET `Name` = @Name, `Address` = @Address , `Age` = @Age WHERE `Id` = @id;";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `users` WHERE `Id` = @id;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Id",
                DbType = DbType.Int32,
                Value = ID,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@UserId",
                DbType = DbType.Int32,
                Value = UserId,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ProductId",
                DbType = DbType.Int32,
                Value = ProductId,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Date",
                DbType = DbType.DateTime,
                Value =  this.Date,
            });
        }

    }
}
