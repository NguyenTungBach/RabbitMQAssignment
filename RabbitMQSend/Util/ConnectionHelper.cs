using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RabbitMQSend.Util
{
    class ConnectionHelper
    {
        private static SqlConnection _connection;
        //private const string ConnectionString = @"Server=tcp:testwebserverclientdbserver.database.windows.net,1433;Initial Catalog=RabbitMQAssignment_db;Persist Security Info=False;User ID=sqladmin;Password=Bach@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private const string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=RabbitMQDbLocal;Integrated Security=True";
        public static SqlConnection GetConnectSql()
        {
            if (_connection == null || _connection.State == System.Data.ConnectionState.Closed)
            {
                _connection = new SqlConnection(
                    string.Format(ConnectionString));
            }
            return _connection;
        }
    }
}
