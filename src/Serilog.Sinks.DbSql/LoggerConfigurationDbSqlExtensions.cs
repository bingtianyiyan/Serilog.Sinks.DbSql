using Serilog.Configuration;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.DbSql.Configuration.Factories;
using Serilog.Sinks.DbSql.Platform;
using Serilog.Sinks.DbSql.SqlSink;
using System;
using System.Data.Common;

namespace Serilog.Sinks.DbSql
{
    /// <summary>
    ///     Adds the WriteTo. extension method to <see cref="LoggerConfiguration" />.
    /// </summary>
    public static class LoggerConfigurationDbSqlExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="loggerConfiguration"></param>
        /// <param name="factory"></param>
        /// <param name="connectionString"></param>
        /// <param name="sinkOptions"></param>
        /// <param name="restrictedToMinimumLevel"></param>
        /// <param name="formatProvider"></param>
        /// <param name="columnOptions"></param>
        /// <param name="logEventFormatter"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static LoggerConfiguration DbSql(
        this LoggerSinkConfiguration loggerConfiguration,
        DbProviderFactory factory,
        string connectionString,
        DbSqlSinkOptions sinkOptions,
        LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
        IFormatProvider formatProvider = null,
        ColumnOptions columnOptions = null,
        ITextFormatter logEventFormatter = null)
        {
            if (loggerConfiguration == null)
                throw new ArgumentNullException(nameof(loggerConfiguration));

            IDbSqlSinkFactory sinkFactory = new DbSqlSinkFactory();
            var sink = sinkFactory.Create(connectionString,factory, sinkOptions, formatProvider, columnOptions, logEventFormatter);

            IPeriodicBatchingSinkFactory periodicBatchingSinkFactory = new PeriodicBatchingSinkFactory();
            var periodicBatchingSink = periodicBatchingSinkFactory.Create(sink, sinkOptions);

            return loggerConfiguration.Sink(periodicBatchingSink, restrictedToMinimumLevel);
        }

        public static LoggerConfiguration DbSql(
            this LoggerSinkConfiguration loggerConfiguration,
            string connectionString,
             DbProviderFactory factory,
            string configSectionName,
            DbSqlSinkOptions sinkOptions,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            IFormatProvider formatProvider = null,
            ColumnOptions columnOptions = null,
            ITextFormatter logEventFormatter = null) =>
            loggerConfiguration.DbSqlInternal(
                configSectionName: configSectionName,
                factory: factory,
                connectionString: connectionString,
                sinkOptions: sinkOptions,
                restrictedToMinimumLevel: restrictedToMinimumLevel,
                formatProvider: formatProvider,
                columnOptions: columnOptions,
                logEventFormatter: logEventFormatter,
                sinkFactory: new DbSqlSinkFactory(),
                batchingSinkFactory: new PeriodicBatchingSinkFactory());

        // Internal overload with parameters used by tests to override the config section and inject mocks
        internal static LoggerConfiguration DbSqlInternal(
            this LoggerSinkConfiguration loggerConfiguration,
            string configSectionName,
            DbProviderFactory factory,
            string connectionString,
            DbSqlSinkOptions sinkOptions,
            LogEventLevel restrictedToMinimumLevel,
            IFormatProvider formatProvider,
            ColumnOptions columnOptions,
            ITextFormatter logEventFormatter,
            IDbSqlSinkFactory sinkFactory,
            IPeriodicBatchingSinkFactory batchingSinkFactory)
        {
            if (loggerConfiguration == null)
                throw new ArgumentNullException(nameof(loggerConfiguration));

            //ReadConfiguration(configSectionName, ref connectionString, ref sinkOptions, ref columnOptions);


            var sink = sinkFactory.Create( connectionString,factory, sinkOptions, formatProvider, columnOptions, logEventFormatter);

            var periodicBatchingSink = batchingSinkFactory.Create(sink, sinkOptions);

            return loggerConfiguration.Sink(periodicBatchingSink, restrictedToMinimumLevel);
        }

        private static void ReadConfiguration(
            string configSectionName,
            ref string connectionString,
            ref DbSqlSinkOptions sinkOptions,
            ref ColumnOptions columnOptions)
        {
            sinkOptions = sinkOptions ?? new DbSqlSinkOptions();
            columnOptions = columnOptions ?? new ColumnOptions();

            //var serviceConfigSection = applySystemConfiguration.GetSinkConfigurationSection(configSectionName);
            //if (serviceConfigSection != null)
            //{
            //    columnOptions = applySystemConfiguration.ConfigureColumnOptions(serviceConfigSection, columnOptions);
            //    sinkOptions = applySystemConfiguration.ConfigureSinkOptions(serviceConfigSection, sinkOptions);
            //}

            ////netcore

            //connectionString = applySystemConfiguration.GetConnectionString(connectionString);
        }
    }
}