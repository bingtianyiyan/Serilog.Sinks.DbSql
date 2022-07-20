﻿using Serilog.Formatting;
using Serilog.Sinks.DbSql.SqlSink;
using Serilog.Sinks.PeriodicBatching;
using System;

namespace Serilog.Sinks.DbSql.Configuration.Factories
{
    internal class DbSqlSinkFactory : IDbSqlSinkFactory
    {
        public IBatchedLogEventSink Create(
            string providerName,
            string connectionString,
            DbSqlSinkOptions sinkOptions,
            IFormatProvider formatProvider,
            ColumnOptions columnOptions,
            ITextFormatter logEventFormatter) =>
            new DbSqlSink(
                providerName,
                connectionString,
                sinkOptions,
                formatProvider,
                columnOptions,
                logEventFormatter);
    }
}