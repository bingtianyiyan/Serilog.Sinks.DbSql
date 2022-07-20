using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Serilog.Sinks.DbSql.Platform
{
    public class ProviderFactory:IProviderFactory   
    {
		private static Dictionary<SqlProviderType, string> providerInvariantNames = new Dictionary<SqlProviderType, string>();
		private static Dictionary<SqlProviderType, DbProviderFactory> providerFactoies = new Dictionary<SqlProviderType, DbProviderFactory>(20);
		static ProviderFactory()
		{
			providerInvariantNames.Add(SqlProviderType.SqlServer, "System.Data.SqlClient");
			providerInvariantNames.Add(SqlProviderType.MySql, "MySql.Data.MySqlClient");
			providerInvariantNames.Add(SqlProviderType.SQLite, "System.Data.SQLite");
			providerInvariantNames.Add(SqlProviderType.Oracle, "Oracle.DataAccess.Client");
			providerInvariantNames.Add(SqlProviderType.ODBC, "System.Data.ODBC");
			providerInvariantNames.Add(SqlProviderType.OleDb, "System.Data.OleDb");
			providerInvariantNames.Add(SqlProviderType.Firebird, "FirebirdSql.Data.Firebird");
			providerInvariantNames.Add(SqlProviderType.PostgreSql, "Npgsql");
			providerInvariantNames.Add(SqlProviderType.DB2, "IBM.Data.DB2.iSeries");
			providerInvariantNames.Add(SqlProviderType.Informix, "IBM.Data.Informix");
			providerInvariantNames.Add(SqlProviderType.SqlServerCe, "System.Data.SqlServerCe");
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
		public void RegisterFactory(string providerInvariantName, DbProviderFactory factory)
        {
			DbProviderFactories.RegisterFactory(providerInvariantName, factory);
		}


	}
}
