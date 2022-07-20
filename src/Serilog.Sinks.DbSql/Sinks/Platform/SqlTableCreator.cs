using Serilog.Debugging;
using System;
using System.Data;

namespace Serilog.Sinks.DbSql
{
    internal class SqlTableCreator : ISqlTableCreator
    {
        private readonly string _tableName;
        private readonly string _schemaName;
        private readonly SqlProviderType _sqlDatabaseType;
        private readonly ColumnOptions _columnOptions;
        private readonly ISqlCreateTableWriter _sqlCreateTableWriter;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public SqlTableCreator(
            string tableName,
            string schemaName,
            SqlProviderType sqlDatabaseType,
            ColumnOptions columnOptions,
            ISqlCreateTableWriter sqlCreateTableWriter,
            ISqlConnectionFactory sqlConnectionFactory)
        {
            _tableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            _schemaName = schemaName ?? throw new ArgumentNullException(nameof(schemaName));
            _sqlDatabaseType = sqlDatabaseType;
            _columnOptions = columnOptions ?? throw new ArgumentNullException(nameof(columnOptions));
            _sqlCreateTableWriter = sqlCreateTableWriter ?? throw new ArgumentNullException(nameof(sqlCreateTableWriter));
            _sqlConnectionFactory = sqlConnectionFactory ?? throw new ArgumentNullException(nameof(sqlConnectionFactory));
        }

        public void CreateTable(DataTable dataTable)
        {
            try
            {
                using (var conn = _sqlConnectionFactory.Create())
                {
                    var sql = _sqlCreateTableWriter.GetSqlFromDataTable(_schemaName, _tableName,_sqlDatabaseType, dataTable, _columnOptions);
                    using (var cmd = conn.CreateCommand(sql,null))
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                SelfLog.WriteLine("Exception creating table {0}:\n{1}", _tableName, ex.ToString());
            }
        }
    }
}