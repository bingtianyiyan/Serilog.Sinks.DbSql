using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.DbSql.Output;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Serilog.Sinks.DbSql
{
    internal class SqlBulkBatchWriter : ISqlBulkBatchWriter
    {
        private readonly string _tableName;
        private readonly string _schemaName;
        private readonly bool _disableTriggers;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly ILogEventDataGenerator _logEventDataGenerator;

        public SqlBulkBatchWriter(
            string tableName,
            string schemaName,
            bool disableTriggers,
            ISqlConnectionFactory sqlConnectionFactory,
            ILogEventDataGenerator logEventDataGenerator)
        {
            _tableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            _schemaName = !String.IsNullOrEmpty(schemaName) ? $"{schemaName}." : "";
            _disableTriggers = disableTriggers;
            _sqlConnectionFactory = sqlConnectionFactory ?? throw new ArgumentNullException(nameof(sqlConnectionFactory));
            _logEventDataGenerator = logEventDataGenerator ?? throw new ArgumentNullException(nameof(logEventDataGenerator));
        }

        public async Task WriteBatch(IEnumerable<LogEvent> events, DataTable dataTable)
        {
            try
            {
                // FillDataTable(events, dataTable);

                using (var connection = _sqlConnectionFactory.Create())
                {
                    var columns = dataTable.Columns.Cast<DataColumn>()
                                                   .Where(x => x.ColumnName != nameof(StandardColumn.Id))
                                                   .Select(m => m.ColumnName);
                    try
                    {
                        using (var insertCommand = connection.CreateCommand(connection.BeginTran()))
                        {
                            insertCommand.CommandText = $"INSERT INTO {_schemaName}{_tableName}({string.Join(",", columns)}) VALUES(?{string.Join(",?", columns)})";
                            for (int i = 0; i < dataTable.Columns.Count; i++)
                            {
                                if (dataTable.Columns[i].ColumnName != nameof(StandardColumn.Id))
                                {
                                    insertCommand.AddParameterName("?" + dataTable.Columns[i].ColumnName);
                                }
                            }
                            var commandP = insertCommand.GetParameters();

                            foreach (var logEvent in events)
                            {
                                var index = 0;
                                foreach (var field in _logEventDataGenerator.GetColumnsAndValues(logEvent))
                                {
                                    commandP[index].Value = field.Value;
                                    index++;
                                }
                                insertCommand.ExecuteNonQuery();
                            }

                            connection.CommitTran();
                        }
                    }
                    catch (Exception ex)
                    {
                        connection.RollbackTran();
                        SelfLog.WriteLine("Unable to write {0} log events to the database due to following error: {1}", events.Count(), ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                SelfLog.WriteLine("create Factory Unable to write {0} log events to the database due to following error: {1}", events.Count(), ex.Message);
            }
#if NET452
           await Task.FromResult(false);

#else
            await Task.CompletedTask;
#endif
        }

        private void FillDataTable(IEnumerable<LogEvent> events, DataTable dataTable)
        {
            // Add the new rows to the collection.
            foreach (var logEvent in events)
            {
                var row = dataTable.NewRow();

                foreach (var field in _logEventDataGenerator.GetColumnsAndValues(logEvent))
                {
                    row[field.Key] = field.Value;
                }

                dataTable.Rows.Add(row);
            }

            dataTable.AcceptChanges();
        }
    }
}