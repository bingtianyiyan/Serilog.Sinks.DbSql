namespace Serilog.Sinks.DbSql
{
    /// <summary>
    /// Database Provider
    /// </summary>
    public enum SqlProviderType : byte
    {
        SqlServer,
        MySql,
        SQLite,
        Oracle,
        ODBC,
        OleDb,
        Firebird,
        PostgreSql,
        DB2,
        Informix,
        SqlServerCe
    }
}