using Serilog.Events;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Serilog.Sinks.DbSql
{
    internal interface ISqlBulkBatchWriter
    {
        Task WriteBatch(IEnumerable<LogEvent> events, DataTable dataTable);
    }
}