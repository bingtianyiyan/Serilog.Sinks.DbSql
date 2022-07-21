using System.Data;

namespace Serilog.Sinks.DbSql
{
    internal interface ISqlTableCreator
    {
        void CreateTable(DataTable dataTable);

        void CreateTable(string sql);
    }
}