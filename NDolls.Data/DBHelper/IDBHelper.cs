using System;
using System.Data.Common;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NDolls.Data
{
    public interface IDBHelper
    {
        DbConnection Connection { get; }
        //DbTransaction Transaction { get; set; }

        int ExecuteNonQuery(CommandType cmdType, string cmdText, List<DbParameter> commandParameters);
        int ExecuteNonQuery(DbTransaction trans, CommandType cmdType, string cmdText, List<DbParameter> commandParameters);
        DbDataReader ExecuteReader(CommandType cmdType, string cmdText, List<DbParameter> commandParameters);
        object ExecuteScalar(CommandType cmdType, string cmdText, List<DbParameter> commandParameters);
        DataTable Query(string sqlString, List<DbParameter> parameters);
    }
}
