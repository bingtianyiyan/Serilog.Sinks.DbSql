﻿using System.Data;
using System.Globalization;
using System.Text;

namespace Serilog.Sinks.DbSql
{
    internal class SqliteTableCreator : ISqlCreateTableWriter
    {
        public string GetSqlFromDataTable(
            string schemaName,
            string tableName,
            DataTable dataTable,
            ColumnOptions columnOptions)
        {
            var sql = new StringBuilder();
            var ix = new StringBuilder();
            sql.AppendLine($"CREATE TABLE IF NOT EXISTS {schemaName}.{tableName} ( ");

            // build column list
            var i = 1;
            foreach (DataColumn column in dataTable.Columns)
            {
                var common = (SqlColumn)column.ExtendedProperties["SqlColumn"];

                sql.Append(GetColumnDDL(common));
                if (dataTable.Columns.Count > i++) sql.Append(",");
                sql.AppendLine();
            }

            // end of CREATE TABLE
            sql.AppendLine(");");

            // output any extra non-clustered indexes
            sql.Append(ix);
            return sql.ToString();
        }

        /// <summary>
        /// get column all is to lower
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private static string GetColumnDDL(SqlColumn column)
        {
            var sb = new StringBuilder();

            sb.Append($"{column.ColumnName} ");

            var datType = column.DataType.ToString() == column.RealDataType ? column.DataType.ToString().ToLowerInvariant() : column.RealDataType;
            //if not set,get the specail hander
            if (column.DataType.ToString() == column.RealDataType)
            {
                if (datType == "int")
                {
                    sb.Append("integer");
                }
                else
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
                sb.Append(" PRIMARY KEY AUTOINCREMENT");

            sb.Append(column.AllowNull ? " NULL" : " NOT NULL");

            return sb.ToString();
        }
    }
}
