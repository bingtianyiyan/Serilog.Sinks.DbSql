using Serilog.Core;
using Serilog.Sinks.DbSql.SqlSink;
using Serilog.Sinks.PeriodicBatching;

namespace Serilog.Sinks.DbSql.Configuration.Factories
{
    internal interface IPeriodicBatchingSinkFactory
    {
        ILogEventSink Create(IBatchedLogEventSink sink, DbSqlSinkOptions sinkOptions);
    }
}