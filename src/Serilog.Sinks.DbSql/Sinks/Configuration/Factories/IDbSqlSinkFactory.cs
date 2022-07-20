using System;
using Serilog.Formatting;
using Serilog.Sinks.DbSql.SqlSink;
using Serilog.Sinks.PeriodicBatching;

namespace Serilog.Sinks.DbSql.Configuration.Factories
{
    internal interface IDbSqlSinkFactory
    {
        IBatchedLogEventSink Create(
            string providerName,
            string connectionString,
            DbSqlSinkOptions sinkOptions,
            IFormatProvider formatProvider,
            ColumnOptions columnOptions,
            ITextFormatter logEventFormatter);
    }
}
