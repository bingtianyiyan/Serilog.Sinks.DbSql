using System.Data.Common;

namespace Serilog.Sinks.DbSql.Platform
{
    public interface IProviderFactory
    {
        void RegisterFactory(string providerInvariantName, DbProviderFactory factory);

        string GetProviderInvariantName(SqlProviderType providerType);

        DbProviderFactory GetDbProviderFactory(SqlProviderType providerType);
    }
}