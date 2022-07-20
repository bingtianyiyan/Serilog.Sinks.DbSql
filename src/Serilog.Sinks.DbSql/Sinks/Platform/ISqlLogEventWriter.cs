using Serilog.Events;

namespace Serilog.Sinks.DbSql
{
    internal interface ISqlLogEventWriter
    {
        void WriteEvent(LogEvent logEvent);
    }
}