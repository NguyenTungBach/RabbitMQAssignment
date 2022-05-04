using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace RabbitMQReceived.Util
{
    class ConnectionHelper
    {
        private static SqlConnection _connection;
        private const string ConnectionString = @"Server=tcp:testwebserverclientdbserver.database.windows.net,1433;Initial Catalog=RabbitMQAssignment_db;Persist Security Info=False;User ID=sqladmin;Password=Bach@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
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
