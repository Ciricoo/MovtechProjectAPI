using System.Data.SqlClient;

namespace MovtechProject.DataAcess.Data
{
    public class Database
    {

        private readonly string _connectionString;

        public Database(string connectionString)
        {
            _connectionString = connectionString;
        }
        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
