using System;
using System.Data;
using System.Data.Common;

namespace Serilog.Sinks.DbSql.SqlClient
{
    internal class SqlCommandWrapper : ISqlCommandWrapper
    {
        private readonly IDbCommand _sqlCommand;
        private bool _disposedValue;

        public SqlCommandWrapper(IDbCommand sqlCommand)
        {
            _sqlCommand = sqlCommand ?? throw new ArgumentNullException(nameof(sqlCommand));
        }

        public CommandType CommandType
        {
            get => _sqlCommand.CommandType;
            set => _sqlCommand.CommandType = value;
        }

        public string CommandText
        {
            get => _sqlCommand.CommandText;
            set => _sqlCommand.CommandText = value;
        }

        public void AddParameter(string parameterName, object value)
        {
            var parameter = (DbParameter)_sqlCommand.CreateParameter();//(parameterName, value ?? DBNull.Value)
            parameter.ParameterName = parameterName;
            parameter.Value = value;

            _sqlCommand.Parameters.Add(parameter);
        }

        public void AddParameterName(string parameterName)
        {
            var parameter = (DbParameter)_sqlCommand.CreateParameter();//(parameterName, value ?? DBNull.Value)
            parameter.ParameterName = parameterName;
            _sqlCommand.Parameters.Add(parameter);
        }

        public DbParameterCollection GetParameters()
        {
            return (DbParameterCollection)_sqlCommand.Parameters;
        }

        public DbTransaction GetTransaction()
        {
            return (DbTransaction)_sqlCommand.Transaction;
        }

        public int ExecuteNonQuery() =>
            _sqlCommand.ExecuteNonQuery();

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                _sqlCommand.Dispose();
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