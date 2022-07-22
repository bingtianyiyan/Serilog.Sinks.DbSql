using System.Data;
using System.Globalization;
using System.Text;

namespace Serilog.Sinks.DbSql
{
    internal class SqlCreateTableWriter : ISqlCreateTableWriter
    {
        public string GetSqlFromDataTable(
            string schemaName,
            string tableName,
            SqlProviderType sqlDatabaseType,
            DataTable dataTable,
            ColumnOptions columnOptions)
        {
            var sql = new StringBuilder();
            var ix = new StringBuilder();
            var indexCount = 1;
            if (sqlDatabaseType == SqlProviderType.MySql)
            {
                sql.AppendLine($"CREATE TABLE IF NOT EXISTS {schemaName}.{tableName} ( ");

                // build column list
                var i = 1;
                foreach (DataColumn column in dataTable.Columns)
                {
                    var common = (SqlColumn)column.ExtendedProperties["SqlColumn"];

                    sql.Append(GetColumnDDL(common, sqlDatabaseType));
                    if (dataTable.Columns.Count > i++) sql.Append(",");
                    sql.AppendLine();
                }

                // end of CREATE TABLE
                sql.AppendLine(");");

                // output any extra non-clustered indexes
                sql.Append(ix);
            }
            else if (sqlDatabaseType == SqlProviderType.SqlServer)
            {
                // start schema check and DDL (wrap in EXEC to make a separate batch)
                sql.AppendLine($"IF(NOT EXISTS(SELECT * FROM sys.schemas WHERE name = '{schemaName}'))");
                sql.AppendLine("BEGIN");
                sql.AppendLine($"EXEC('CREATE SCHEMA [{schemaName}] AUTHORIZATION [dbo]')");
                sql.AppendLine("END");

                // start table-creatin batch and DDL
                sql.AppendLine($"IF NOT EXISTS (SELECT s.name, t.name FROM sys.tables t JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE s.name = '{schemaName}' AND t.name = '{tableName}')");
                sql.AppendLine("BEGIN");
                sql.AppendLine($"CREATE TABLE [{schemaName}].[{tableName}] ( ");

                // build column list
                var i = 1;
                foreach (DataColumn column in dataTable.Columns)
                {
                    var common = (SqlColumn)column.ExtendedProperties["SqlColumn"];

                    sql.Append(GetColumnDDL(common, sqlDatabaseType));
                    if (dataTable.Columns.Count > i++) sql.Append(",");
                    sql.AppendLine();

                    // collect non-PK indexes for separate output after the table DDL
                    if (common != null && common.NonClusteredIndex && common != columnOptions.PrimaryKey)
                        ix.AppendLine($"CREATE NONCLUSTERED INDEX [IX{indexCount++}_{tableName}] ON [{schemaName}].[{tableName}] ([{common.ColumnName}]);");
                }

                // primary key constraint at the end of the table DDL
                if (columnOptions.PrimaryKey != null)
                {
                    var clustering = (columnOptions.PrimaryKey.NonClusteredIndex ? "NON" : string.Empty);
                    sql.AppendLine($" CONSTRAINT [PK_{tableName}] PRIMARY KEY {clustering}CLUSTERED ([{columnOptions.PrimaryKey.ColumnName}])");
                }

                // end of CREATE TABLE
                sql.AppendLine(");");

                // CCI is output separately after table DDL
                if (columnOptions.ClusteredColumnstoreIndex)
                    sql.AppendLine($"CREATE CLUSTERED COLUMNSTORE INDEX [CCI_{tableName}] ON [{schemaName}].[{tableName}]");

                // output any extra non-clustered indexes
                sql.Append(ix);

                // end of batch
                sql.AppendLine("END");
            }
            else if (sqlDatabaseType == SqlProviderType.PostgreSql)
            {
                sql.AppendLine($"CREATE TABLE IF NOT EXISTS {tableName} ( ");

                // build column list
                var i = 1;
                foreach (DataColumn column in dataTable.Columns)
                {
                    var common = (SqlColumn)column.ExtendedProperties["SqlColumn"];

                    sql.Append(GetColumnDDL(common, sqlDatabaseType));
                    if (dataTable.Columns.Count > i++) sql.Append(",");
                    sql.AppendLine();
                }
                // end of CREATE TABLE
                sql.AppendLine(");");

                // output any extra non-clustered indexes
                sql.Append(ix);
            }

            return sql.ToString();
        }

        /// <summary>
        /// get column all is to lower
        /// </summary>
        /// <param name="column"></param>
        /// <param name="sqlDatabaseType"></param>
        /// <returns></returns>
        private static string GetColumnDDL(SqlColumn column, SqlProviderType sqlDatabaseType)
        {
            var sb = new StringBuilder();

            sb.Append($"{column.ColumnName} ");

            var datType = column.DataType.ToString().ToLowerInvariant();
            //时间类型postgres特殊处理timestamp
            if (sqlDatabaseType == SqlProviderType.PostgreSql)
            {
                if (datType == "datetime")
                {
                    sb.Append("timestamp");
                }
                if (datType == "nvarchar")
                {
                    sb.Append("varchar");
                }
                if (column.StandardColumnIdentifier != StandardColumn.Id && datType != "datetime" && datType != "nvarchar")
                {
                    sb.Append(datType);
                }
            }
            else
            {
                sb.Append(datType);
            }

            if (SqlDataTypes.DataLengthRequired.Contains(column.DataType))
                sb.Append("(").Append(column.DataLength == -1 ? "1024" : column.DataLength.ToString(CultureInfo.InvariantCulture)).Append(")");

            if (column.StandardColumnIdentifier == StandardColumn.Id)
                if (sqlDatabaseType == SqlProviderType.SqlServer)
                {
                    sb.Append(" IDENTITY(1,1)");
                }
                else if (sqlDatabaseType == SqlProviderType.MySql)
                {
                    sb.Append(" auto_increment primary key");
                }
                //else if(sqlDatabaseType == SqlProviderType.Oracle)
                //{
                //}
                else if (sqlDatabaseType == SqlProviderType.PostgreSql)
                {
                    sb.Append(" serial primary key");
                }

            sb.Append(column.AllowNull ? " NULL" : " NOT NULL");

            return sb.ToString();
        }
    }
}