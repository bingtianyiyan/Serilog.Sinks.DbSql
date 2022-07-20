using Serilog.Sinks.DbSql.SqlClient;

namespace Serilog.Sinks.DbSql
{
    internal interface ISqlConnectionFactory
    {
        ISqlConnectionWrapper Create();
    }
}