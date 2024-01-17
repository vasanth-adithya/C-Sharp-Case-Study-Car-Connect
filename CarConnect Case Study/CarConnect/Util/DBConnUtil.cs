using System.Configuration;
using System.Data.SqlClient;

namespace CarConnect.Util
{
    static class DBConnUtil
    {
        static SqlConnection connection = null;

        public static SqlConnection GetConnection()
        {
            connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["CarConnectDB"].ConnectionString;
            return connection;
        }
    }
}
