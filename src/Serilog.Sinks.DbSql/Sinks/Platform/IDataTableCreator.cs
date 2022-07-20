using System.Data;

namespace Serilog.Sinks.DbSql
{
    internal interface IDataTableCreator
    {
        DataTable CreateDataTable();
    }
}