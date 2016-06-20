using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace NDolls.Data
{
    /// <summary>
    /// 本类为SQLite数据库帮助静态类,使用时只需直接调用即可,无需实例化
    /// </summary>
    public class SQLiteHelper : HelperBase, IDBHelper
    {

        /// <summary>
        /// 执行数据库操作(新增、更新或删除)
        /// </summary>
        /// <param name="commandType">执行类型</param>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="cmdParms">SQL参数对象</param>
        /// <returns>所受影响的行数</returns>
        public int ExecuteNonQuery(CommandType cmdType, string cmdText, List<DbParameter> cmdParms)
        {
            int result = 0;
            if (ConnectionString == null || ConnectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (cmdText == null || cmdText.Length == 0)
                throw new ArgumentNullException("commandText");

            SQLiteCommand cmd = new SQLiteCommand();
            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                result = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
            return result;
        }

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) using an existing SQL Transaction 
        /// using the provided parameters.
        /// </summary>
        /// <param name="trans">an existing sql transaction</param>
        /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or T-SQL command</param>
        /// <param name="cmdParms">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public int ExecuteNonQuery(DbTransaction trans, CommandType cmdType, string cmdText, List<DbParameter> cmdParms)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// 执行数据库操作(新增、更新或删除)同时返回执行后查询所得的第1行第1列数据
        /// </summary>
        /// <param name="cmdText">执行语句或存储过程名</param>
        /// <param name="cmdType">执行类型</param>
        /// <param name="cmdParms">SQL参数对象</param>
        /// <returns>查询所得的第1行第1列数据</returns>
        public object ExecuteScalar(CommandType cmdType, string cmdText, List<DbParameter> cmdParms)
        {
            if (ConnectionString == null || ConnectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (cmdText == null || cmdText.Length == 0)
                throw new ArgumentNullException("commandText");

            SQLiteCommand cmd = new SQLiteCommand();
            using (SQLiteConnection con = new SQLiteConnection(ConnectionString))
            {
                PrepareCommand(cmd, con, null, cmdType, cmdText, cmdParms);
                object result = cmd.ExecuteScalar();
                cmdParms.Clear();
                return result;
            }            
        }

        /// <summary>
        /// 执行数据库查询，返回SqlDataReader对象
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型</param>
        /// <param name="cmdParms">SQL参数对象</param>
        /// <returns>SqlDataReader对象</returns>
        public DbDataReader ExecuteReader(CommandType cmdType, string cmdText, List<DbParameter> cmdParms)
        {
            DbDataReader reader = null;
            if (ConnectionString == null || ConnectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (cmdText == null || cmdText.Length == 0)
                throw new ArgumentNullException("commandText");

            SQLiteConnection conn = new SQLiteConnection(ConnectionString);
            SQLiteCommand cmd = new SQLiteCommand();
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            try
            {
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return reader;
        }

        /// <summary>
        /// 执行数据库查询，返回DataSet对象
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="cmdText">执行语句或存储过程名</param>
        /// <param name="cmdType">执行类型</param>
        /// <param name="cmdParms">SQL参数对象</param>
        /// <returns>DataSet对象</returns>
        public DataTable Query(string cmdText, List<DbParameter> cmdParms)
        {
            if (ConnectionString == null || ConnectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (cmdText == null || cmdText.Length == 0)
                throw new ArgumentNullException("commandText");

            SQLiteConnection thisconnection = new SQLiteConnection(ConnectionString);
            thisconnection.Open();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter();
            SQLiteCommand cmd = new SQLiteCommand(cmdText, thisconnection);
            adapter.SelectCommand = cmd;
            if (cmdParms != null)
                foreach (SQLiteParameter param in cmdParms)
                    cmd.Parameters.Add(param);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            thisconnection.Close();
            return ds.Tables[0];
        }

        /// <summary>
        /// 通用分页查询方法
        /// </summary>
        /// <param name="connString">连接字符串</param>
        /// <param name="tableName">表名</param>
        /// <param name="strColumns">查询字段名</param>
        /// <param name="strWhere">where条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <param name="pageSize">每页数据数量</param>
        /// <param name="currentIndex">当前页数</param>
        /// <param name="recordOut">数据总量</param>
        /// <returns>DataTable数据表</returns>
        //public static DataTable SelectPaging(string connString, string tableName, string strColumns, string strWhere, string strOrder, int pageSize, int currentIndex, out int recordOut)
        //{
        //    DataTable dt = new DataTable();
        //    recordOut = Convert.ToInt32(ExecuteScalar(connString, "select count(*) from " + tableName, CommandType.Text));
        //    string pagingTemplate = "select {0} from {1} where {2} order by {3} limit {4} offset {5} ";
        //    int offsetCount = (currentIndex - 1) * pageSize;
        //    string commandText = String.Format(pagingTemplate, strColumns, tableName, strWhere, strOrder, pageSize.ToString(), offsetCount.ToString());
        //    using (DbDataReader reader = ExecuteReader(connString, commandText, CommandType.Text))
        //    {
        //        if (reader != null)
        //        {
        //            dt.Load(reader);
        //        }
        //    }
        //    return dt;
        //}

        #region 命令预处理
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
                cmd.Transaction = trans as SQLiteTransaction;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SQLiteParameter parm in cmdParms)
                {
                    if (parm.Value == null || ((parm.DbType == DbType.DateTime || parm.DbType == DbType.Date) && (parm.Value.ToString().Contains("0001") )))
                        parm.Value = DBNull.Value;
                    cmd.Parameters.Add(parm);
                }
            }
        }
        #endregion
    }
}
