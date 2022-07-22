using Serilog.Formatting;
using Serilog.Sinks.DbSql.SqlSink;
using Serilog.Sinks.PeriodicBatching;
using System;
using System.Data.Common;

namespace Serilog.Sinks.DbSql.Configuration.Factories
{
    internal interface IDbSqlSinkFactory
    {
        IBatchedLogEventSink Create(
            string connectionString,
            DbProviderFactory factory,
            DbSqlSinkOptions sinkOptions,
            IFormatProvider formatProvider,
            ColumnOptions columnOptions,
            ITextFormatter logEventFormatter);
    }
}