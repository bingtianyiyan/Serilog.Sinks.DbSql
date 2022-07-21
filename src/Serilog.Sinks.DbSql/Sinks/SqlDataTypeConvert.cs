//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Text;

//namespace Serilog.Sinks.DbSql.Sinks
//{
//    /// <summary>
//    /// Convert Value Data Type To Real Database Value Data Type 
//    /// </summary>
//    public class SqlDataTypeConvert
//    {
//        //SqlServer,
//        //MySql,
//        //SQLite,
//        //Oracle,
//        //ODBC,
//        //OleDb,
//        //Firebird,
//        //PostgreSql,
//        //DB2,
//        //Informix,
//        //SqlServerCe

//        /// <summary>
//        /// PostgreSql
//        /// </summary>
//        /// <param name="dbType"></param>
//        /// <param name="columnLength"></param>
//        /// <returns></returns>
//        /// <exception cref="ArgumentOutOfRangeException"></exception>
//        private static string GetPostgreSqlSqlTypeStr(SqlDbType dbType, int? columnLength = null)
//        {
//            //switch (dbType)
//            //{
//            //    case SqlDbType.BigInt:
//            //        return "bigint";
//            //    case SqlDbType.Decimal:
//            //        return "double precision";
//            //    case SqlDbType.Int:
//            //        return "integer";
//            //    case SqlDbType.Decimal:
//            //        return "numeric";
//            //    case SqlDbType.Real:
//            //        return "real";
//            //    case SqlDbType.SmallInt:
//            //        return "smallint";
//            //    case SqlDbType.Bit:
//            //        return "boolean";
//            //    case SqlDbType.Money:
//            //        return "money";
//            //    case SqlDbType.Char:
//            //        return $"character({columnLength ?? DefaultCharColumnsLength})";
//            //    case SqlDbType.Text:
//            //        return "text";
//            //    case SqlDbType.VarChar:
//            //        return $"character varying({columnLength ?? DefaultVarcharColumnsLength})";
//            //    case SqlDbType.Binary:
//            //        return "bytea";
//            //    case SqlDbType.Date:
//            //        return "date";
//            //    case SqlDbType.Time:
//            //        return "time";
//            //    case SqlDbType.Timestamp:
//            //        return "timestamp";
//            //    case SqlDbType.TimestampTz:
//            //        return "timestamp with time zone";
//            //    case SqlDbType.TimeSpan:
//            //        return "interval";
//            //    case SqlDbType.DateTimeOffset:
//            //        return "time with time zone";
//            //    case NpgsqlDbType.Inet:
//            //        return "inet";
//            //    case NpgsqlDbType.Cidr:
//            //        return "cidr";
//            //    case NpgsqlDbType.MacAddr:
//            //        return "macaddr";
//            //    case SqlDbType.Bit:
//            //        return $"bit({columnLength ?? DefaultBitColumnsLength})";
//            //    case NpgsqlDbType.Varbit:
//            //        return $"bit varying({columnLength ?? DefaultBitColumnsLength})";
//            //    case SqlDbType.Guid:
//            //        return "uuid";
//            //    case SqlDbType.Xml:
//            //        return "xml";
//            //    case NpgsqlDbType.Json:
//            //        return "json";
//            //    case NpgsqlDbType.Jsonb:
//            //        return "jsonb";
//            //    default:
//            //        throw new ArgumentOutOfRangeException(nameof(dbType), dbType, "Cannot atomatically create column of type " + dbType);
//            //}
//            return null;
//        }


//        #region SqlServer
//        public static string GetSqlServerSqlTypeStr(SqlDbType dbType, int? columnLength = null, string realDbType = null)
//        {
//            if (realDbType != null)
//            {
//                return realDbType;
//            }
//            switch (dbType)
//            {
//                case SqlDbType.Int:
//                    return "int";
//                case SqlDbType.VarChar:
//                    return "varchar";
//                case SqlDbType.Bit:
//                    return "bit";
//                case SqlDbType.DateTime:
//                    return "datetime";
//                case SqlDbType.Decimal:
//                    //case SqlDbType.Decimal:
//                   // return "numeric";
//                    return "decimal";
//                case SqlDbType.Float:
//                    return "float";
//                case SqlDbType.Image:
//                    return "image";
//                case SqlDbType.Money:
//                    return "money";
//                case SqlDbType.NText:
//                    return "ntext";
//                case SqlDbType.NVarChar:
//                    return "nvarchar";
//                case SqlDbType.SmallDateTime:
//                    return "smalldatetime";
//                case SqlDbType.SmallInt:
//                    return "smallint";
//                case SqlDbType.Text:
//                    return "text";
//                case SqlDbType.BigInt:
//                    return "bigint";
//                case SqlDbType.Binary:
//                    return "binary";
//                case SqlDbType.Char:
//                    return "char";
//                case SqlDbType.NChar:
//                    return "nchar";
//                case SqlDbType.Real:
//                    return "real";
//                case SqlDbType.SmallMoney:
//                    return "smallmoney";
//                case SqlDbType.Variant:
//                    return "sql_variant";
//                case SqlDbType.Timestamp:
//                    return "timestamp";
//                case SqlDbType.TinyInt:
//                    return "tinyint";
//                case SqlDbType.UniqueIdentifier:
//                    return "uniqueidentifier";
//                case SqlDbType.VarBinary:
//                    return "varbinary";
//                case SqlDbType.Xml:
//                    return "xml";
//                default:
//                    return null;
//            }

//        }
//        #endregion

//    }

//}