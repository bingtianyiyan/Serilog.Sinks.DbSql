using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace Serilog.Sinks.DbSql.Platform
{
    internal class ProviderFactory : IProviderFactory
    {
        private static Dictionary<SqlProviderType, string> providerInvariantNames = new Dictionary<SqlProviderType, string>();
        private static Dictionary<SqlProviderType, DbProviderFactory> providerFactoies = new Dictionary<SqlProviderType, DbProviderFactory>(20);

        static ProviderFactory()
        {
            providerInvariantNames.Add(SqlProviderType.SqlServer, "System.Data.SqlClient");
            providerInvariantNames.Add(SqlProviderType.MySql, "MySql.Data.MySqlClient");
            providerInvariantNames.Add(SqlProviderType.SQLite, "System.Data.SQLite");
            // providerInvariantNames.Add(SqlProviderType.Oracle, "Oracle.DataAccess.Client");
            // do not test
            //providerInvariantNames.Add(SqlProviderType.ODBC, "System.Data.ODBC");
            //providerInvariantNames.Add(SqlProviderType.OleDb, "System.Data.OleDb");
            //providerInvariantNames.Add(SqlProviderType.Firebird, "FirebirdSql.Data.Firebird");
            providerInvariantNames.Add(SqlProviderType.PostgreSql, "Npgsql");
            //providerInvariantNames.Add(SqlProviderType.DB2, "IBM.Data.DB2.iSeries");
            //providerInvariantNames.Add(SqlProviderType.Informix, "IBM.Data.Informix");
            //providerInvariantNames.Add(SqlProviderType.SqlServerCe, "System.Data.SqlServerCe");
        }

        /// <summary>
        /// get provider
        /// </summary>
        /// <param name="providerType"></param>
        /// <returns></returns>
        public string GetProviderInvariantName(SqlProviderType providerType)
        {
            return providerInvariantNames[providerType];
        }

        /// <summary>
        /// get database with DbProviderFactory
        /// </summary>
        /// <param name="providerType"></param>
        /// <returns></returns>
        public DbProviderFactory GetDbProviderFactory(SqlProviderType providerType)
        {
            if (!providerFactoies.ContainsKey(providerType))
            {
                providerFactoies.Add(providerType, ImportDbProviderFactory(providerType));
            }
            return providerFactoies[providerType];
        }

        /// <summary>
        /// load database DbProviderFactory
        /// </summary>
        /// <param name="providerType"></param>
        /// <returns></returns>
        private static DbProviderFactory ImportDbProviderFactory(SqlProviderType providerType)
        {
            string providerName = providerInvariantNames[providerType];
            DbProviderFactory factory = null;
            try
            {
                factory = DbProviderFactories.GetFactory(providerName);
            }
            catch (Exception ex)
            {
                factory = null;
            }
            return factory;
        }

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="providerInvariantName"></param>
        /// <param name="factory"></param>
        public void RegisterFactory(string providerInvariantName, SqlProviderType providerType)
        {
            var factory = GetDbProviderFactoryWithName(providerType);
            DbProviderFactories.RegisterFactory(providerInvariantName, factory);
        }

        private DbProviderFactory GetDbProviderFactoryWithName(SqlProviderType providerType)
        {
            if (providerType == SqlProviderType.MySql)
            {
                return MySqlConnector.MySqlConnectorFactory.Instance;
            }
            else if (providerType == SqlProviderType.PostgreSql)
            {
                return NpgsqlFactory.Instance;
            }
            else if (providerType == SqlProviderType.SqlServer)
            {
                return SqlClientFactory.Instance;
            }
            else if (providerType == SqlProviderType.SQLite)
            {
                return SQLiteFactory.Instance;
            }else if(providerType == SqlProviderType.Oracle)
            {
                return OracleClientFactory.Instance;
            }
            throw new Exception("Factoryies is not defined in project");
        }
    }
}