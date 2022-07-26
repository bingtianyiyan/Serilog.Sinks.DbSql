﻿using Serilog.Formatting;
using Serilog.Sinks.DbSql.Output;
using Serilog.Sinks.DbSql.Platform;
using Serilog.Sinks.DbSql.SqlSink;
using System;

namespace Serilog.Sinks.DbSql.Dependencies
{
    internal static class SinkDependenciesFactory
    {
        internal static SinkDependencies Create(
            string connectionString,
            DbSqlSinkOptions sinkOptions,
            IFormatProvider formatProvider,
            ColumnOptions columnOptions,
            ITextFormatter logEventFormatter)
        {
            columnOptions = columnOptions ?? new ColumnOptions();
            columnOptions.FinalizeConfigurationForSinkConstructor();
            //register
            IProviderFactory _providerFactory = new ProviderFactory();
            string providerName = _providerFactory.GetProviderInvariantName(sinkOptions.SqlDatabaseType);
            _providerFactory.RegisterFactory(providerName, sinkOptions.SqlDatabaseType);

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
                    sinkOptions.TableName, sinkOptions.SchemaName, columnOptions,
                    SqlCreateTableWriter.GetRealDbTableWriter(sinkOptions.SqlDatabaseType), sqlConnectionFactory),
                DataTableCreator = new DataTableCreator(sinkOptions.TableName, columnOptions),
                SqlBulkBatchWriter = new SqlBulkBatchWriter(
                    sinkOptions.TableName, sinkOptions.SchemaName,
                     sqlConnectionFactory, logEventDataGenerator),
            };

            return sinkDependencies;
        }
    }
}