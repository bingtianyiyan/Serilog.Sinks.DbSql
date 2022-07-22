using Serilog.Formatting;
using Serilog.Sinks.DbSql.SqlSink;
using Serilog.Sinks.PeriodicBatching;
using System;
using System.Data.Common;

namespace Serilog.Sinks.DbSql.Configuration.Factories
{
    internal class DbSqlSinkFactory : IDbSqlSinkFactory
    {
        public IBatchedLogEventSink Create(
            string connectionString,
            DbProviderFactory factory,
            DbSqlSinkOptions sinkOptions,
            IFormatProvider formatProvider,
            ColumnOptions columnOptions,
            ITextFormatter logEventFormatter) =>
            new DbSqlSink(
                factory,
                connectionString,
                sinkOptions,
                formatProvider,
                columnOptions,
                logEventFormatter);
    }
}