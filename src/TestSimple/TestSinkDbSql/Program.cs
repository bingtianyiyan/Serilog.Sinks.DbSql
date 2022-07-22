using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.DbSql;
using Serilog.Sinks.DbSql.SqlSink;
using System.Collections.ObjectModel;
using System.Data;

namespace TestSinkDbSql
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                })
                .ConfigureAppConfiguration(x =>
                {
                    x.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    //loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration)
                    //.Enrich.FromLogContext()
                    // New SinkOptions based interface

                    #region Mysql

                    //loggerConfiguration
                    //              .WriteTo.DbSql(
                    //                          "Server=localhost; Port=3306; Database=CoreShop; Uid=root; Pwd=root;",
                    //                         new DbSqlSinkOptions
                    //                         {
                    //                             SchemaName = "coreshop",
                    //                             TableName = "logs7",
                    //                             AutoCreateSqlTable = true,
                    //                             SqlDatabaseType = SqlProviderType.MySql
                    //                         },
                    //                           restrictedToMinimumLevel: LevelAlias.Minimum,
                    //                           formatProvider: null,
                    //                           columnOptions: BuildColumnOptions(),
                    //                           logEventFormatter: null
                    //                );

                    #endregion Mysql

                    #region Postgres problem

                    //loggerConfiguration
                    //  .WriteTo.DbSql(
                    //              "Server=127.0.0.1; Port=5432; Database=coreshop; Uid=postgres; Pwd=postgres;",
                    //             new DbSqlSinkOptions
                    //             {
                    //                 SchemaName = "public",
                    //                 TableName = "logs6",
                    //                 AutoCreateSqlTable = true,
                    //                 SqlDatabaseType = SqlProviderType.PostgreSql
                    //             },
                    //               restrictedToMinimumLevel: LevelAlias.Minimum,
                    //               formatProvider: null,
                    //               columnOptions: BuildColumnOptions(),
                    //               logEventFormatter: null
                    //    );

                    #endregion Postgres problem

                    #region Sqlite
                    loggerConfiguration
                     .WriteTo.DbSql(
                                 $@"DataSource = E:\工具包\sqlite-tools-win32-x86-3390200\sqlite-tools-win32-x86-3390200\mysqdb.db;",
                                new DbSqlSinkOptions
                                {
                                    SchemaName = "main",
                                    TableName = "logs6",
                                    AutoCreateSqlTable = true,
                                    SqlDatabaseType = SqlProviderType.SQLite
                                },
                                  restrictedToMinimumLevel: LevelAlias.Minimum,
                                  formatProvider: null,
                                  columnOptions: BuildColumnOptions(),
                                  logEventFormatter: null
                       );
                    #endregion
                });

        private static ColumnOptions BuildColumnOptions()
        {
            var columnOptions = new ColumnOptions
            {
                //TimeStamp =
                //{
                //    ColumnName = "TimeStampUTC",
                //    ConvertToUtc = true,
                //},

                AdditionalColumns = new Collection<SqlColumn>
                {
                    new SqlColumn { DataType =SqlDbType.VarChar, ColumnName = "MachineName" },
                   // new SqlColumn {DataType = SqlDbType.Int,ColumnName ="MM"}
                }
            };

            columnOptions.Store.Remove(StandardColumn.Properties);

            return columnOptions;
        }
    }
}