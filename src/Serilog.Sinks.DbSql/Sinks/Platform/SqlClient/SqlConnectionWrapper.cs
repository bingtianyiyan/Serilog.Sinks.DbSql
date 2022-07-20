using System;
using System.Data;
using System.Data.Common;

namespace Serilog.Sinks.DbSql.SqlClient
{
    internal class SqlConnectionWrapper : ISqlConnectionWrapper
    {
        private readonly IDbConnection _sqlConnection;
        private bool _disposedValue;
        private readonly object _o_lock = new object();

        public SqlConnectionWrapper(string providerName, string connectionString)
        {
            if (_sqlConnection == null)
            {
                lock (_o_lock)
                {
                    if (_sqlConnection == null)
                    {
                        var factory = DbProviderFactories.GetFactory(providerName);
                        _sqlConnection = factory.CreateConnection();
                        _sqlConnection.ConnectionString = connectionString;
                    }
                }
            }
        }

        public IDbTransaction Transaction { get; private set; }

        public bool IsClosed
        {
            get
            {
                if (_sqlConnection == null)
                {
                    return true;
                }
                return (_sqlConnection.State == ConnectionState.Closed || _sqlConnection.State == ConnectionState.Broken);
            }
        }

        public virtual void Open()
        {
            if (IsClosed)
            {
                _sqlConnection.Open();
            }
        }

        public ISqlCommandWrapper CreateCommand(IDbTransaction trans)
        {
            var sqlCommand = _sqlConnection.CreateCommand();
            if (trans != null)
            {
                sqlCommand.Transaction = trans;
            }
            return new SqlCommandWrapper(sqlCommand);
        }

        public ISqlCommandWrapper CreateCommand(string cmdText, IDbTransaction trans)
        {
            var sqlCommand = _sqlConnection.CreateCommand();
            sqlCommand.CommandText = cmdText;
            sqlCommand.Connection = _sqlConnection;
            if (trans != null)
            {
                sqlCommand.Transaction = trans;
            }
            return new SqlCommandWrapper(sqlCommand);
        }

        public IDbTransaction BeginTran()
        {
            Open();
            if (Transaction == null)
            {
                Transaction = _sqlConnection.BeginTransaction();
            }
            return Transaction;
        }

        public IDbTransaction BeginTran(IsolationLevel il)
        {
            Open();
            if (Transaction == null)
            {
                Transaction = _sqlConnection.BeginTransaction(il);
            }
            return Transaction;
        }

        public void CommitTran()
        {
            if (!IsClosed && Transaction != null)
            {
                try
                {
                    Transaction.Commit();
                    Close();
                }
                catch
                {
                    RollbackTran();
                    throw;
                }
            }
        }

        public void RollbackTran()
        {
            if (!IsClosed && Transaction != null)
            {
                try
                {
                    Transaction.Rollback();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    Close();
                }
            }
        }

        public void DisposeTran()
        {
            if (Transaction != null)
            {
                Transaction?.Dispose();
                Transaction = null;
            }
        }

        public void Close()
        {
            DisposeTran();
            CloseConnection();
        }

        private void CloseConnection()
        {
            if (!IsClosed)
            {
                _sqlConnection.Close();
            }
            _sqlConnection?.Dispose();
            _disposedValue = true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                _sqlConnection.Dispose();
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}