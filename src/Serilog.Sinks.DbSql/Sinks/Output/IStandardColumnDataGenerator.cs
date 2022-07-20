using Serilog.Events;
using System.Collections.Generic;

namespace Serilog.Sinks.DbSql.Output
{
    internal interface IStandardColumnDataGenerator
    {
        KeyValuePair<string, object> GetStandardColumnNameAndValue(StandardColumn column, LogEvent logEvent);
    }
}