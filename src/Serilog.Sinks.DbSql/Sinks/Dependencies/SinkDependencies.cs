﻿namespace Serilog.Sinks.DbSql.Dependencies
{
    internal class SinkDependencies
    {
        public IDataTableCreator DataTableCreator { get; set; }
        public ISqlTableCreator SqlTableCreator { get; set; }
        public ISqlBulkBatchWriter SqlBulkBatchWriter { get; set; }
        public ISqlLogEventWriter SqlLogEventWriter { get; set; }
    }
}