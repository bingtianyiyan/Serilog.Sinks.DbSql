using Serilog.Events;
using System.Collections.Generic;

namespace Serilog.Sinks.DbSql.Output
{
    internal interface ILogEventDataGenerator
    {
        IEnumerable<KeyValuePair<string, object>> GetColumnsAndValues(LogEvent logEvent);
    }
}