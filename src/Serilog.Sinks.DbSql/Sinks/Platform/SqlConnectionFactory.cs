using Serilog.Sinks.DbSql.SqlClient;
using System;

namespace Serilog.Sinks.DbSql
{
    internal class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly string _connectionString;
        private readonly string _providerName;
        public SqlConnectionFactory(
            string providerName,
            string connectionString)
        {
            if (string.IsNullOrWhiteSpace(providerName))
            {
                throw new ArgumentNullException(nameof(providerName));
            }
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }
            _connectionString = connectionString;
            _providerName = providerName;
        }

        public ISqlConnectionWrapper Create()
        {
            return new SqlConnectionWrapper(_providerName,_connectionString);
        }
    }
}