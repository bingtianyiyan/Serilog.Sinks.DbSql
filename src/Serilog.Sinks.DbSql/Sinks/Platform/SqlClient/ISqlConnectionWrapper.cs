using System;
using System.Data;

namespace Serilog.Sinks.DbSql.SqlClient
{
    internal interface ISqlConnectionWrapper : IDisposable
    {

        void Open();

        ISqlCommandWrapper CreateCommand(IDbTransaction trans);

        ISqlCommandWrapper CreateCommand(string cmdText, IDbTransaction trans);

        IDbTransaction BeginTran();

        void CommitTran();

        void RollbackTran();
    }
}