using MySql.Data.MySqlClient;
using System.Configuration;

namespace BookStoreDAL
{
    public class DatabaseConnection
    {
        private static readonly string connectionString = "Server=localhost;Database=quanlycuahangbansach;Uid=root;Pwd=123456;CharSet=utf8mb4;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public static bool TestConnection()
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}