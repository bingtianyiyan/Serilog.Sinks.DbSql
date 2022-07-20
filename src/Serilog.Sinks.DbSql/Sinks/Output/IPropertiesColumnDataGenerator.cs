using Serilog.Events;
using System.Collections.Generic;

namespace Serilog.Sinks.DbSql.Output
{
    internal interface IPropertiesColumnDataGenerator
    {
        IEnumerable<KeyValuePair<string, object>> ConvertPropertiesToColumn(IReadOnlyDictionary<string, LogEventPropertyValue> properties);
    }
}