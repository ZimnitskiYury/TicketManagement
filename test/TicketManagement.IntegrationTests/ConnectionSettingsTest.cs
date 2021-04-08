using System.Data.Common;
using System.Data.SqlClient;

namespace TicketManagement.IntegrationTests
{
    internal class ConnectionSettingsTest
    {
        private readonly string _connection = @"Data Source =.\SQLEXPRESS;Initial Catalog = TicketManagement.DatabaseTest; Integrated Security = True";

        public DbConnection Connection
        {
            get
            {
                return new SqlConnection(_connection);
            }
        }
    }
}
