using System;
using System.Threading.Tasks;
using MySqlConnector;
using System.Data;

namespace ProductsAPI
{
    public class Product
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public String Category { get; set; }
        public double Price { get; set; }

        internal AppDb Db { get; set; }

        public Product()
        {
        }

        internal Product(AppDb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `products` (`Name`, `Category` , `Price`) VALUES (@Name, @Category, @Price);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            ID = (int) cmd.LastInsertedId;
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `products` SET `Name` = @Name, 
            `Category` = @Category, `Price` = @Price WHERE `Id` = @id;";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `BlogPost` WHERE `Id` = @id;";
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
                ParameterName = "@Name",
                DbType = DbType.String,
                Value = Name,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Category",
                DbType = DbType.String,
                Value = Category,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Price",
                DbType = DbType.Int32,
                Value = Price,
            });
        }

    }
}
