using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using Serilog.Sinks.DbSql;
using Serilog.Sinks.DbSql.SqlSink;
using Serilog.Events;
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
                    loggerConfiguration
                        .WriteTo.DbSql(
                                   MySqlConnector.MySqlConnectorFactory.Instance,
                                    "Server=localhost; Port=3306; Database=CoreShop; Uid=root; Pwd=root;",
                                   new DbSqlSinkOptions
                                   {
                                       SchemaName = "coreshop",
                                       TableName = "logs5",
                                       AutoCreateSqlTable = false,
                                       SqlDatabaseType = SqlProviderType.MySql
                                   },
                                     restrictedToMinimumLevel: LevelAlias.Minimum,
                                     formatProvider: null,
                                     columnOptions: BuildColumnOptions(),
                                     logEventFormatter: null
                          );

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
                    new SqlColumn { DataType = SqlDbType.NVarChar, ColumnName = "MachineName" }
                }
            };

            //columnOptions.Store.Remove(StandardColumn.Properties);

            return columnOptions;
        }


    }
}
