using System;
using System.Data;
using System.Data.Common;

namespace Serilog.Sinks.DbSql.SqlClient
{
    internal interface ISqlCommandWrapper : IDisposable
    {
        CommandType CommandType { get; set; }
        string CommandText { get; set; }

        void AddParameter(string parameterName, object value);

        void AddParameterName(string parameterName);

        DbParameterCollection GetParameters();

        DbTransaction GetTransaction();

        int ExecuteNonQuery();
    }
}