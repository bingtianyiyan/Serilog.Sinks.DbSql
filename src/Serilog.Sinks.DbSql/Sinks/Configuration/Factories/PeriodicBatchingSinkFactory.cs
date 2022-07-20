using Serilog.Core;
using Serilog.Sinks.DbSql.SqlSink;
using Serilog.Sinks.PeriodicBatching;

namespace Serilog.Sinks.DbSql.Configuration.Factories
{
    internal class PeriodicBatchingSinkFactory : IPeriodicBatchingSinkFactory
    {
        public ILogEventSink Create(IBatchedLogEventSink sink, DbSqlSinkOptions sinkOptions)
        {
            var periodicBatchingSinkOptions = new PeriodicBatchingSinkOptions
            {
                BatchSizeLimit = sinkOptions.BatchPostingLimit,
                Period = sinkOptions.BatchPeriod,
                EagerlyEmitFirstEvent = sinkOptions.EagerlyEmitFirstEvent
            };

            return new PeriodicBatchingSink(sink, periodicBatchingSinkOptions);
        }
    }
}