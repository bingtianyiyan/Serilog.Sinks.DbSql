using System;

namespace Serilog.Sinks.DbSql.SqlSink
{
    /// <summary>
    /// provider Common DbSql Configuration Options
    /// </summary>
   public  class DbSqlSinkOptions
    {

        public DbSqlSinkOptions()
        {
            BatchPostingLimit = 50;
            BatchPeriod = TimeSpan.FromSeconds(5);
            EagerlyEmitFirstEvent = true;
        }

        internal DbSqlSinkOptions(
            string tableName,
            int? batchPostingLimit,
            TimeSpan? batchPeriod,
            bool autoCreateSqlTable,
            string schemaName,
            SqlProviderType sqlDatabaseType) : this()
        {
            TableName = tableName;
            BatchPostingLimit = batchPostingLimit ?? BatchPostingLimit;
            BatchPeriod = batchPeriod ?? BatchPeriod;
            AutoCreateSqlTable = autoCreateSqlTable;
            SchemaName = schemaName ?? SchemaName;
            SqlDatabaseType = sqlDatabaseType;
        }

        /// <summary>
        /// Name of the database table for writing the log events
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Name of the database schema
        /// </summary>
        public string SchemaName { get; set; }

        /// <summary>
        /// Flag to automatically create the log events table if it does not exist (default: false)
        /// </summary>
        public bool AutoCreateSqlTable { get; set; }

        /// <summary>
        /// Flag to make logging SQL commands take part in ambient transactions (default: false)
        /// </summary>
        public bool EnlistInTransaction { get; set; }

        /// <summary>
        /// Limits how many log events are written to the database per batch (default: 50)
        /// </summary>
        public int BatchPostingLimit { get; set; }

        /// <summary>
        /// Time span until a batch of log events is written to the database (default: 5 seconds)
        /// </summary>
        public TimeSpan BatchPeriod { get; set; }

        /// <summary>
        /// Flag to eagerly emit a batch containing the first received event (default: true)
        /// </summary>
        public bool EagerlyEmitFirstEvent { get; set; }


        /// <summary>
        /// Db Type 
        /// </summary>
        public SqlProviderType SqlDatabaseType { get; set; }
    }
}
