using Serilog.Formatting;
using Serilog.Sinks.DbSql.Output;
using Serilog.Sinks.DbSql.SqlSink;
using System;

namespace Serilog.Sinks.DbSql.Dependencies
{
    internal static class SinkDependenciesFactory
    {
        internal static SinkDependencies Create(
            string providerName,
            string connectionString,
            DbSqlSinkOptions sinkOptions,
            IFormatProvider formatProvider,
            ColumnOptions columnOptions,
            ITextFormatter logEventFormatter)
        {
            columnOptions = columnOptions ?? new ColumnOptions();
            columnOptions.FinalizeConfigurationForSinkConstructor();
            var sqlConnectionFactory =
                new SqlConnectionFactory(providerName, connectionString);
            var logEventDataGenerator =
                new LogEventDataGenerator(columnOptions,
                    new StandardColumnDataGenerator(columnOptions, formatProvider,
                        new XmlPropertyFormatter(),
                        logEventFormatter),
                    new PropertiesColumnDataGenerator(columnOptions));

            var sinkDependencies = new SinkDependencies
            {
                SqlTableCreator = new SqlTableCreator(
                    sinkOptions.TableName, sinkOptions.SchemaName, sinkOptions.SqlDatabaseType, columnOptions,
                    new SqlCreateTableWriter(), sqlConnectionFactory),
                DataTableCreator = new DataTableCreator(sinkOptions.TableName, columnOptions),
                SqlBulkBatchWriter = new SqlBulkBatchWriter(
                    sinkOptions.TableName, sinkOptions.SchemaName,
                     sqlConnectionFactory, logEventDataGenerator),
            };

            return sinkDependencies;
        }
    }
}