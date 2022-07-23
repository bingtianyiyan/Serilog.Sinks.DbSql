using System.Data;

namespace Serilog.Sinks.DbSql
{
    internal interface ISqlCreateTableWriter
    {
        string GetSqlFromDataTable(string schemaName, string tableName,DataTable dataTable, ColumnOptions columnOptions);
    }
}