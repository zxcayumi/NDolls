using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDolls.Data.Entity;
using System.Data.SqlClient;
using NDolls.Data.Util;
using System.Data.Common;

namespace NDolls.Data
{
    /// <summary>
    /// 数据库事务处理
    /// </summary>
    public class DBTransaction
    {
        private static readonly string insertSQL = "INSERT INTO {0}({1}) VALUES({2})";
        private static readonly string updateSQL = "UPDATE {0} SET {1} WHERE {2}";
        private static readonly string deleteSQL = "DELETE FROM {0} WHERE {1}";

        private List<DbParameter> pars = null;//sql命令参数集合
        private StringBuilder fs = null;//字段sql
        private StringBuilder vs = null;//值sql
        private string sql = null;
        private string condition = null;
        private string tableName = null;

        private IDBHelper dbHelper;
        public List<OptEntity> entities = new List<OptEntity>();

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public DBTransaction()
        {
            this.dbHelper = SQLFactory.CreateDBHelper();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="entities">操作对象集合</param>
        public DBTransaction(IDBHelper dbHelper, List<OptEntity> entities)
        {
            this.dbHelper = dbHelper;
            this.entities = entities;
        }
        #endregion

        #region 事务处理相关
        private static Dictionary<Guid, TranSession> tranConnectionDic = new Dictionary<Guid, TranSession>();//事务连接存储字典
        private TranSession ts = new TranSession();

        /// <summary>
        /// 事务处理Transaction
        /// </summary>
        public DbTransaction Transaction
        {
            get { return ts.STransaction; }
        }

        /// <summary>
        /// 事务SessionID
        /// </summary>
        public Guid SessionID
        {
            get
            { return ts.SID; }
        }

        /// <summary>
        /// 开启事务处理
        /// </summary>
        public void TransactionOpen()
        {
            Guid sid = Guid.NewGuid();
            ts.SID = sid;

            if (ts.SConnection == null)
            {
                ts.SConnection = SQLFactory.CreateDBConnection();
            }

            if (ts.SConnection.State != System.Data.ConnectionState.Open)
            {
                ts.SConnection.Open();
            }

            ts.STransaction = ts.SConnection.BeginTransaction();

            tranConnectionDic.Add(sid, ts);//开启批量事务操作，将TranSession存入字典
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void TransactionCommit()
        {
            if (ts.STransaction != null)
                ts.STransaction.Commit();
            TranConnClose();

            //事务完毕后清理缓存
            ClearCache();
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void TransactionRollback()
        {
            if (ts.STransaction != null)
                ts.STransaction.Rollback();
            TranConnClose();

            //事务完毕后清理缓存
            ClearCache();
        }

        /// <summary>
        /// 清理Repository缓存
        /// </summary>
        private void ClearCache()
        {
            foreach (OptEntity item in entities)
            {
                RepositoryFactory<EntityBase>.RemoveRepository(SessionID.ToString() + "_" + item.Entity.GetType().ToString());
            }
        }

        /// <summary>
        /// 关闭事务连接
        /// </summary>
        private void TranConnClose()
        {
            if (ts.SConnection.State == System.Data.ConnectionState.Open)
            {
                ts.SConnection.Close();
                ts.SConnection = null;
                tranConnectionDic.Remove(ts.SID);//关闭批量事务操作，将TranSession从字典移除
            }
        }

        /// <summary>
        /// 事务执行
        /// </summary>
        /// <returns>事务执行结果</returns>
        public bool Excute()
        {
            DbConnection conn = null;
            DbTransaction tran = null;
            if (!tranConnectionDic.ContainsKey(ts.SID))//若未开启全局事务处理
            {
                conn = SQLFactory.CreateDBConnection();
                conn.Open();
                tran = conn.BeginTransaction();
            }
            else//若开启全局事务处理
            {
                conn = ts.SConnection;
                tran = ts.STransaction;
            }

            //数据处理
            try
            {
                foreach (OptEntity item in entities)
                {
                    tableName = EntityUtil.GetTableName(item.Entity.GetType());
                    resetVars();//重置参数(新对象重新赋值)

                    switch (item.Type)
                    {
                        case OptType.Create://新增                            
                            sql = getCreateSQL(item.Entity);
                            break;
                        case OptType.UpdateAllowedNull://修改
                            sql = getUpdateSQL(item.Entity, OptType.UpdateAllowedNull);
                            break;
                        case OptType.UpdateIgnoreNull://修改
                            sql = getUpdateSQL(item.Entity, OptType.UpdateIgnoreNull);
                            break;
                        case OptType.Save:
                            break;
                        case OptType.Delete://删除
                            sql = getDeleteSQL(item.Entity);
                            break;
                    }

                    if (!String.IsNullOrEmpty(sql))
                        dbHelper.ExecuteNonQuery(tran, System.Data.CommandType.Text, sql, pars);
                }

                //字典中存在SID：事务操作（多个对象批量操作）；字典中不存在SID：非事务操作（单个对象操作）
                if (!tranConnectionDic.ContainsKey(ts.SID))
                {
                    tran.Commit();
                    conn.Close();
                }
                return true;
            }
            catch(Exception ex)
            {
                if (!tranConnectionDic.ContainsKey(ts.SID))//若未开启全局事务处理
                {
                    tran.Rollback();
                    conn.Close();
                }
                throw (ex);
            }
            
        }
        #endregion

        #region 辅助方法

        private void resetVars()
        {
            sql = "";
            pars = new List<DbParameter>();
            fs = new StringBuilder();//字段sql
            vs = new StringBuilder();//值sql
        }

        private string getCreateSQL(EntityBase entity)
        {
            string identitySql="";
            foreach (DataField field in EntityUtil.GetDataFields(entity))//构造SQL参数集合
            {
                if (field.FieldValue != null && !field.IsIdentity)//字段值不为空 且 不是标识
                {
                    pars.Add(SQLFactory.CreateParameter(field.FieldName, field.FieldValue));
                    fs.Append(field.FieldName + ",");
                    vs.Append("@" + field.FieldName + ",");
                }
                else if (field.FieldValue != null && !field.IsIdentity)//如果有标识字段，返回最新标识值
                {
                    identitySql = ";";
                    identitySql += SQL.ResourceManager.GetString(DataConfig.DatabaseType+"Identity").Replace("{0}",EntityUtil.GetTableName(entity.GetType()));
                }
            }

            return String.Format(insertSQL, tableName, fs.ToString().TrimEnd(','), vs.ToString().TrimEnd(',')) + identitySql ;
        }

        /// <summary>
        /// 获取UpdateSQL语句
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="allowNull">是否允许值为null的属性(true:为null的属性赋空值；false:忽略为null的属性不做处理，保持数据库原样)</param>
        private string getUpdateSQL(EntityBase entity,OptType allowNull)
        {
            condition = "";

            foreach (DataField field in EntityUtil.GetDataFields(entity))//构造SQL参数集合
            {
                pars.Add(SQLFactory.CreateParameter(field.FieldName, field.FieldValue));
                //pars.Add(new SqlParameter(field.FieldName, field.FieldValue));

                if (EntityUtil.GetPrimaryKey(tableName).Contains(field.FieldName))
                {
                    condition += field.FieldName + "=@" + field.FieldName + " AND ";
                }
                else
                {
                    if (!field.IsIdentity)
                    {
                        if (field.FieldValue == null && allowNull != OptType.UpdateAllowedNull)
                        {
                            continue;
                        }
                        else
                        {
                            fs.Append(field.FieldName + "=@" + field.FieldName + ",");
                        }
                    }
                }
            }

            return String.Format(updateSQL, tableName, fs.ToString().Trim(','), condition.Substring(0, condition.LastIndexOf("AND")));//生成UPDATE语句
        }

        private string getDeleteSQL(EntityBase entity)
        {
            string pk = EntityUtil.GetPrimaryKey(tableName);
            string[] pks = pk.Split(',');
            pars = new List<DbParameter>();
            condition = "";

            for (int i = 0; i < pks.Length; i++)
            {
                condition += pks[i] + "=@" + pks[i] + " AND ";
                pars.Add(SQLFactory.CreateParameter(pks[i], EntityUtil.GetValueByField(entity, pks[i])));
                //pars.Add(new SqlParameter(pks[i], EntityUtil.GetValueByField(entity, pks[i])));
            }

            return String.Format(deleteSQL, tableName, condition.Substring(0, condition.LastIndexOf(',')));
        }

        #endregion

    }
}
