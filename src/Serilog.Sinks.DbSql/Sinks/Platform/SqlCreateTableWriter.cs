
using System;

namespace Serilog.Sinks.DbSql
{
    internal class SqlCreateTableWriter
    {
       public static ISqlCreateTableWriter GetRealDbTableWriter(SqlProviderType sqlProviderType)
        {
            switch (sqlProviderType)
            {
                case SqlProviderType.SqlServer:
                    return new SqlServerTableCreator();
                case SqlProviderType.MySql:
                    return new MySqlTableCreator();
                case SqlProviderType.SQLite:
                    return new SqliteTableCreator();
                case SqlProviderType.Oracle:
                    return new OracleTableCreator();
                case SqlProviderType.PostgreSql:
                    return new PostgresTableCreator();
            }
            throw new Exception("UnDefined SqlCreateTableWriter Object");
        }
    }
}