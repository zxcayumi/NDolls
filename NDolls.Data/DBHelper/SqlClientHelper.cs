using System.IO;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Net;
using System.Collections.Generic;
using System.Data.Common;

namespace NDolls.Data
{
    class SqlClientHelper : HelperBase, IDBHelper
    {
        // Hashtable to store cached parameters
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public int ExecuteNonQuery(CommandType cmdType, string cmdText, List<DbParameter> commandParameters)
        {

            if (ConnectionString == null || ConnectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (cmdText == null || cmdText.Length == 0)
                throw new ArgumentNullException("commandText");

            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(ConnectionString)) 
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) using an existing SQL Transaction 
        /// using the provided parameters.
        /// </summary>
        /// <param name="trans">an existing sql transaction</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public int ExecuteNonQuery(DbTransaction trans, CommandType cmdType, string cmdText, List<DbParameter> commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// Execute a SqlCommand that returns a resultset against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>A SqlDataReader containing the results</returns>
        public DbDataReader ExecuteReader(CommandType cmdType, string cmdText, List<DbParameter> commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(ConnectionString);

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch {
                conn.Close();
                throw;
            }
        }

        /// <summary>
        /// Execute a SqlCommand that returns the first column of the first record against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public object ExecuteScalar(CommandType cmdType, string cmdText, List<DbParameter> commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection connection = new SqlConnection(ConnectionString)) {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        #region 不常用的方法
        /// <summary>
        /// add parameter array to the cache
        /// </summary>
        /// <param name="cacheKey">Key to the parameter cache</param>
        /// <param name="cmdParms">an array of SqlParamters to be cached</param>
        public static void CacheParameters(string cacheKey, List<DbParameter> commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }

        /// <summary>
        /// Retrieve cached parameters
        /// </summary>
        /// <param name="cacheKey">key used to lookup parameters</param>
        /// <returns>Cached SqlParamters array</returns>
        public static SqlParameter[] GetCachedParameters(string cacheKey) {
            SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];

            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }
        #endregion

        /// <summary>
        /// Prepare a command for execution
        /// </summary>
        /// <param name="cmd">SqlCommand object</param>
        /// <param name="conn">SqlConnection object</param>
        /// <param name="trans">SqlTransaction object</param>
        /// <param name="cmdType">Cmd type e.g. stored procedure or text</param>
        /// <param name="cmdText">Command text, e.g. Select * from Products</param>
        /// <param name="cmdParms">SqlParameters to use in the command</param>
        private static void PrepareCommand(DbCommand cmd, DbConnection conn, DbTransaction trans, CommandType cmdType, string cmdText, List<DbParameter> cmdParms)
        {
            CommonVar.WriteLog(cmdText);
            
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans as SqlTransaction;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                {
                    if (parm.Value == null || ((parm.DbType == DbType.DateTime || parm.DbType == DbType.Date) && (parm.Value.ToString().Contains("0001"))))
                        parm.Value = DBNull.Value;
                    cmd.Parameters.Add(parm);
                }
            }
        }

        /// <summary>
        /// DataSet批量回写数据库
        /// </summary>
        /// <param name="thisDataset">要加入数据库的Dataset信息</param>
        /// <param name="sql">查询语句</param>
        /// <param name="TableName">表名称,只能对单个表进行操作,多表相联系的无法实现</param>
        //public void DataSetUpdate(DataSet thisDataset, String sql,String TableName,DataColumn[] primaryKeys) {
        //    SqlDataAdapter thisAdapter = new SqlDataAdapter(sql, connectionString);
        //    thisDataset.Tables[0].PrimaryKey = primaryKeys;
        //    SqlCommandBuilder thisBuilder = new SqlCommandBuilder(thisAdapter);
        //    thisAdapter.Update(thisDataset, TableName);
        //    thisDataset.AcceptChanges();
        //}

        /// <summary>
        /// 查询返回数据集
        /// </summary>
        /// <param name="sqlString">查询语句</param>
        /// <param name="parameters">数据参数</param>
        /// <returns>符合条件的数据集</returns>
        public DataTable Query(string sqlString, List<DbParameter> parameters)
        {
            CommonVar.WriteLog(sqlString);

            SqlConnection thisconnection = new SqlConnection(ConnectionString);
            thisconnection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand(sqlString, thisconnection);
            adapter.SelectCommand = cmd;
            if (parameters != null)
                foreach (SqlParameter param in parameters)
                    cmd.Parameters.Add(param);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            thisconnection.Close();
            return ds.Tables[0];
        }
    }
}