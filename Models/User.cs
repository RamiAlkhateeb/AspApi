using System;
using System.Threading.Tasks;
using MySqlConnector;
using System.Data;

namespace ProductsAPI
{
    public class User
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public String Address { get; set; }
        public int Age { get; set; }

        internal AppDb Db { get; set; }

        public User()
        {
        }

        internal User(AppDb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `users` (`Name`, `Address` , `Age`) VALUES (@Name, @Address, @Age);";
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
                ParameterName = "@Name",
                DbType = DbType.String,
                Value = Name,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Address",
                DbType = DbType.String,
                Value = Address,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Age",
                DbType = DbType.Int32,
                Value = Age,
            });
        }

    }
}
