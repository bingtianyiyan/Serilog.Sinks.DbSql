﻿using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.DbSql.Dependencies;
using Serilog.Sinks.PeriodicBatching;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Serilog.Sinks.DbSql.SqlSink
{
    internal class DbSqlSink : IBatchedLogEventSink, IDisposable
    {
        private readonly ISqlBulkBatchWriter _sqlBulkBatchWriter;
        private readonly DataTable _eventTable;

        /// <summary>
        /// The default database schema name.
        /// </summary>
        public const string DefaultSchemaName = "";

        /// <summary>
        /// A reasonable default for the number of events posted in each batch.
        /// </summary>
        public const int DefaultBatchPostingLimit = 50;

        /// <summary>
        /// A reasonable default time to wait between checking for event batches.
        /// </summary>
        public static readonly TimeSpan DefaultPeriod = TimeSpan.FromSeconds(5);

        private bool _disposedValue;

        public DbSqlSink(
            string providerName,
            string connectionString,
            DbSqlSinkOptions sinkOptions,
            IFormatProvider formatProvider = null,
            ColumnOptions columnOptions = null,
            ITextFormatter logEventFormatter = null)
            : this(sinkOptions, SinkDependenciesFactory.Create(providerName, connectionString, sinkOptions, formatProvider, columnOptions, logEventFormatter))
        {
        }

        // Internal constructor with injectable dependencies for better testability
        internal DbSqlSink(
            DbSqlSinkOptions sinkOptions,
            SinkDependencies sinkDependencies)
        {
            ValidateParameters(sinkOptions);
            CheckSinkDependencies(sinkDependencies);

            _sqlBulkBatchWriter = sinkDependencies.SqlBulkBatchWriter;
            _eventTable = sinkDependencies.DataTableCreator.CreateDataTable();

            CreateTable(sinkOptions, sinkDependencies);
        }

        /// <summary>
        /// Emit a batch of log events, running asynchronously.
        /// </summary>
        /// <param name="events">The events to emit.</param>
        /// <remarks>
        /// Override either <see cref="PeriodicBatchingSink.EmitBatch" /> or <see cref="PeriodicBatchingSink.EmitBatchAsync" />, not both.
        /// </remarks>
        public Task EmitBatchAsync(IEnumerable<LogEvent> events) =>
            _sqlBulkBatchWriter.WriteBatch(events, _eventTable);

        /// <summary>
        /// Called upon batchperiod if no data is in batch. Not used by this sink.
        /// </summary>
        /// <returns>A completed task</returns>
        public Task OnEmptyBatchAsync() =>
#if NET452
            Task.FromResult(false);

#else
            Task.CompletedTask;

#endif

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the Serilog.Sinks.DbSql.MySqlServerAuditSink and optionally
        /// releases the managed resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _eventTable.Dispose();
                }

                _disposedValue = true;
            }
        }

        private static void ValidateParameters(DbSqlSinkOptions sinkOptions)
        {
            if (sinkOptions?.TableName == null)
            {
                throw new InvalidOperationException("Table name must be specified!");
            }
        }

        private static void CheckSinkDependencies(SinkDependencies sinkDependencies)
        {
            if (sinkDependencies == null)
            {
                throw new ArgumentNullException(nameof(sinkDependencies));
            }

            if (sinkDependencies.DataTableCreator == null)
            {
                throw new InvalidOperationException($"DataTableCreator is not initialized!");
            }

            if (sinkDependencies.SqlTableCreator == null)
            {
                throw new InvalidOperationException($"SqlTableCreator is not initialized!");
            }

            if (sinkDependencies.SqlBulkBatchWriter == null)
            {
                throw new InvalidOperationException($"SqlBulkBatchWriter is not initialized!");
            }
        }

        private void CreateTable(DbSqlSinkOptions sinkOptions, SinkDependencies sinkDependencies)
        {
            if (sinkOptions.AutoCreateSqlTable)
            {
                sinkDependencies.SqlTableCreator.CreateTable(_eventTable);
            }
        }
    }
}