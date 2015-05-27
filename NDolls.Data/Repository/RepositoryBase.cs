using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDolls.Data.Util;
using NDolls.Data.Entity;
using NDolls.Data.Attribute;
using System.Reflection;
using System.Data.Common;
using System.Data;

namespace NDolls.Data
{
    public class RepositoryBase<T> where T : EntityBase
    {
        protected static readonly string selectSQL = "SELECT {0} FROM {1} WHERE {2}";
        protected static readonly string insertSQL = "INSERT INTO {0}({1}) VALUES({2})";
        protected static readonly string updateSQL = "UPDATE {0} SET {1} WHERE {2}";
        protected static readonly string deleteSQL = "DELETE FROM {0} WHERE {1}";

        protected string tableName = "";//数据库表名
        protected string primaryKey = "";//数据库主键字段名
        protected string[] primaryKeys; //数据库主键字段名集合

        public DBTransaction DBTran { get; set; }

        #region 构造函数

        /// <summary>
        /// 构造函数，初始化实体信息(对应表名、表主键)
        /// </summary>
        public RepositoryBase()
        {
            tableName = EntityUtil.GetTableName(typeof(T));
            primaryKey = EntityUtil.GetPrimaryKey(tableName);
            primaryKeys = primaryKey.Split(',');
        }

        /// <summary>
        /// 构造函数，初始化实体信息(对应表名、表主键)
        /// </summary>
        public RepositoryBase(Type type)
        {
            tableName = EntityUtil.GetTableName(type);
            primaryKey = EntityUtil.GetPrimaryKey(tableName);
            primaryKeys = primaryKey.Split(',');
        }
        #endregion

        #region 公共方法

        /// <summary>
        /// 根据查询条件获取对象集合（由于不同数据库的分页方式不同，将实现放到子类）
        /// </summary>
        /// <param name="top">查询数量(0:查询所有)</param>
        /// <param name="conditions">查询（排序）项集合</param>
        /// <returns>查询结果集合</returns>
        public virtual List<T> FindByCondition(int top, List<Item> items)
        {
            return null;
        }

        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns>查询结果集合</returns>
        public List<T> FindAll()
        {
            return Find("1=1");
        }

        /// <summary>
        /// 根据查询条件获取对象集合
        /// </summary>
        /// <param name="condition">查询（排序）项</param>
        /// <returns>查询结果集合</returns>
        public List<T> FindByCondition(Item item)
        {
            return FindByCondition(0, new List<Item> { item });
        }

        /// <summary>
        /// 根据查询条件获取对象集合
        /// </summary>
        /// <param name="conditions">查询（排序）项集合</param>
        /// <returns>查询结果集合</returns>
        public List<T> FindByCondition(List<Item> items)
        {
            return FindByCondition(0, items);
        }

        /// <summary>
        /// 按实体对象信息查询
        /// </summary>
        /// <param name="model">实体对象</param>
        /// <returns>查询结果集合</returns>
        public List<T> Find(T model)
        {
            return Find(0, model);
        }

        /// <summary>
        /// 按实体对象信息查询
        /// </summary>
        /// <param name="top">查询数量</param>
        /// <param name="model">实体对象</param>
        /// <returns>查询结果集合</returns>
        public List<T> Find(int top, T model)
        {
            List<Item> conditions = new List<Item>();

            List<DataField> fields = EntityUtil.GetDataFields(model);
            foreach (DataField field in fields)
            {
                if (field.FieldValue == null || field.FieldValue.ToString() == "" || field.FieldType.ToLower().Contains("date"))
                    continue;

                if (field.FieldType.ToLower().Contains("varchar"))
                {
                    conditions.Add(new ConditionItem(field.FieldName, field.FieldValue, SearchType.Fuzzy));
                }
                else if ("int,float,decimal,double,number".Contains(field.FieldType.ToLower()))
                {
                    if ((int)field.FieldValue > 0)
                        conditions.Add(new ConditionItem(field.FieldName, field.FieldValue, SearchType.Lower));
                }
                else
                {
                    conditions.Add(new ConditionItem(field.FieldName, field.FieldValue, SearchType.Accurate));
                }
            }

            if (top < 1)
                return FindByCondition(conditions);
            else
                return FindByCondition(top, conditions);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="keyValue">主键对应的值</param>
        /// <returns>实体对象</returns>
        public T FindByPK(string keyValue)
        {
            string sql = String.Format(selectSQL, "*", tableName, primaryKey + "=@" + primaryKey);
            List<DbParameter> pars = new List<DbParameter>();
            pars.Add(SQLFactory.CreateParameter(primaryKey, keyValue));

            T model = default(T);
            try
            {
                model = DataConvert<T>.ToEntity(DBHelper.Query(sql, pars).Rows[0]);

                #region 级联对象加载
                //关联对象加载
                EntityUtil.SetAssociation(model);
                #endregion
            }
            catch { }

            return model;
        }

        /// <summary>
        /// 根据主键查询(联合主键情况)
        /// </summary>
        /// <param name="keyValue">主键对应的值</param>
        /// <returns>实体对象</returns>
        public T FindByPK(string[] keyValues)
        {
            if (primaryKeys.Length != keyValues.Length)
                return null;

            string condition = "";
            List<DbParameter> pars = new List<DbParameter>();
            for (int i = 0; i < primaryKeys.Length; i++)
            {
                condition += primaryKeys[i] + "=@" + primaryKeys[i] + " AND ";
                pars.Add(SQLFactory.CreateParameter(primaryKeys[i], keyValues[i]));
            }
            condition = condition.Substring(0, condition.LastIndexOf("AND"));
            string sql = String.Format(selectSQL, "*", tableName, condition);

            T model = default(T);
            try
            {
                model = DataConvert<T>.ToEntity(DBHelper.Query(sql, pars).Rows[0]);

                //关联对象加载
                EntityUtil.SetAssociation(model);
            }
            catch { }

            return model;
        }

        /// <summary>
        /// 用户自定义查询
        /// </summary>
        /// <param name="customCondition">用户自定义条件</param>
        /// <returns>查询的结果集</returns>
        public List<T> Find(String customCondition)
        {
            if (String.IsNullOrEmpty(customCondition))
            {
                customCondition = "1 = 1";
            }
            string sql = String.Format(selectSQL, "*", tableName, customCondition);
            return DataConvert<T>.ToEntities(DBHelper.Query(sql, null));
        }

        /// <summary>
        /// 自定义查询
        /// </summary>
        /// <param name="fields">要查询的字段</param>
        /// <param name="items">查询项集合</param>
        /// <returns>查询结果集</returns>
        public DataTable FindByCustom(String fields, List<Item> items)
        {
            //构造查询条件
            List<DbParameter> pars = new List<DbParameter>();

            //生成查询语句
            string conSql = getConditionSQL(items, pars);//sql条件部分
            string sql = String.Format(selectSQL, fields, tableName, conSql);

            return DBHelper.Query(sql, pars);
        }

        /// <summary>
        /// 自定义查询
        /// </summary>
        /// <param name="fields">要查询的字段</param>
        /// <param name="customCondition">自定义条件语句</param>
        /// <returns>查询结果集</returns>
        public DataTable FindByCustom(String fields, String customCondition)
        {
            if (String.IsNullOrEmpty(customCondition))
            {
                customCondition = "1 = 1";
            }
            string sql = String.Format(selectSQL, fields, tableName, customCondition);

            return DBHelper.Query(sql, null);
        }

        /// <summary>
        /// 获取符合条件的数据个数
        /// </summary>
        /// <param name="items">查询项集合</param>
        /// <returns>符合条件的数据个数</returns>
        public int GetCount(List<Item> items)
        {
            List<DbParameter> pars = new List<DbParameter>();//构造查询条件
            string conSql = getConditionSQL(items, pars);////生成查询语句,sql条件部分
            string sql = String.Format(selectSQL, "COUNT(*)", tableName, conSql);

            return Convert.ToInt32(DBHelper.ExecuteScalar(System.Data.CommandType.Text, sql, pars));
        }

        /// <summary>
        /// 获取符合条件的数据个数
        /// </summary>
        /// <param name="customCondition">用户自定义条件</param>
        /// <returns>符合条件的数据个数</returns>
        public int GetCount(String customCondition)
        {
            if (String.IsNullOrEmpty(customCondition))
            {
                customCondition = "1 = 1";
            }
            string sql = String.Format(selectSQL, "COUNT(*)", tableName, customCondition);
            return int.Parse(DBHelper.Query(sql, null).Rows[0][0].ToString());
        }

        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>是否操作成功</returns>
        public static bool BatchSave(List<EntityBase> entities)
        {
            DBTransaction tran = new DBTransaction();
            tran.TransactionOpen();
            try
            {
                object obj ;
                Type type ;
                foreach (EntityBase m in entities)
                {
                    obj = RepositoryFactory<EntityBase>.CreateRepository(m, tran);
                    type = obj.GetType();
                    type.GetMethod("AddOrUpdate").Invoke(obj, new object[] { m });
                }

                tran.TransactionCommit();
                ClearCache(tran, entities);//事务结束清理Repository缓存
                return true;
            }
            catch
            {
                tran.TransactionRollback();
                ClearCache(tran, entities);//事务结束清理Repository缓存
                return false;
            }
        }

        private static void ClearCache(DBTransaction tran, List<EntityBase> entities)
        {
            foreach (EntityBase m in entities)
            {
                RepositoryFactory<EntityBase>.RemoveRepository(tran.SessionID.ToString() + "_" + m.GetType().ToString());
            }
        }

        /// <summary>
        /// 根据对象主键查询是否存在该对象
        /// </summary>
        /// <param name="keyValue">主键对应的值</param>
        /// <returns>是否存在</returns>
        public bool Exist(T model)
        {
            String[] pks = primaryKey.Split(',');
            String conditions = "";
            List<DbParameter> pars = new List<DbParameter>();
            foreach(String pk in pks)
            {
                if(conditions != "")
                    conditions += (" AND " + pk + "=@" + pk);
                else
                    conditions += (pk + "=@" + pk);
                pars.Add(SQLFactory.CreateParameter(pk, EntityUtil.GetValueByField(model, pk)));
            }

            string sql = String.Format(selectSQL, "COUNT(*)", tableName, conditions);            

            return "0" != DBHelper.ExecuteScalar(System.Data.CommandType.Text, sql, pars).ToString();
        }

        /// <summary>
        /// 根据条件查询是否存在该对象
        /// </summary>
        /// <param name="conditions">条件项集合</param>
        /// <returns>是否存在</returns>
        public bool Exist(List<Item> items)
        {
            List<DbParameter> pars = new List<DbParameter>();//构造查询条件
            string conSql = getConditionSQL(items, pars);////生成查询语句,sql条件部分
            string sql = String.Format(selectSQL, "COUNT(*)", tableName, conSql);

            return "0" != DBHelper.ExecuteScalar(System.Data.CommandType.Text, sql, pars).ToString();
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="model">实体对象</param>
        /// <returns>添加是否成功</returns>
        public bool Add(T model)
        {
            Fields fields = EntityUtil.GetFieldsByType(model.GetType());
            List<AssociationAttribute> assocations = fields.AssociationFields;

            return Persist(new OptEntity(model, OptType.Create), assocations);
        }

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="model">实体对象</param>
        /// <returns>修改是否成功</returns>
        public bool Update(T model)
        {
            return Update(model, OptType.UpdateAllowedNull);
        }

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="model">实体对象</param>
        /// <returns>修改是否成功</returns>
        public bool Update(T model, OptType type)
        {
            Fields fields = EntityUtil.GetFieldsByType(model.GetType());
            List<AssociationAttribute> assocations = fields.AssociationFields;

            return Persist(new OptEntity(model, type), assocations);
        }

        /// <summary>
        /// 添加或修改（自动判断对象是否存在）
        /// </summary>
        /// <param name="model">实体对象</param>
        /// <returns>执行是否成功</returns>
        public bool AddOrUpdate(T model)
        {
            if (!Exist(model))
            {
                return Add(model);
            }
            else
            {
                return Update(model);
            }
        }

        /// <summary>
        /// 按主键删除
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>删除是否成功</returns>
        public bool Delete(string keyValue)
        {
            string sql = String.Format(deleteSQL, tableName, primaryKey + "=@" + primaryKey);
            List<DbParameter> pars = new List<DbParameter>();
            pars.Add(SQLFactory.CreateParameter(primaryKey, keyValue));

            try
            {
                if(DBHelper.ExecuteNonQuery(System.Data.CommandType.Text, sql, pars)>0)
                    return true;
            }
            catch
            {}
            return false;
        }

        /// <summary>
        /// 按主键删除
        /// </summary>
        /// <param name="keyValues">主键值</param>
        /// <returns>删除是否成功</returns>
        public bool Delete(string[] keyValues)
        {
            if (primaryKeys.Length != keyValues.Length)
                return false;

            string condition = "";
            List<DbParameter> pars = new List<DbParameter>();
            for (int i = 0; i < keyValues.Length; i++)
            {
                condition += primaryKeys[i] + "=@" + primaryKeys[i] + " AND ";
                pars.Add(SQLFactory.CreateParameter(primaryKeys[i], keyValues[i]));
            }

            string sql = String.Format(deleteSQL, tableName, condition.Substring(0, condition.LastIndexOf("AND")));

            try
            {
                if(DBHelper.ExecuteNonQuery(System.Data.CommandType.Text, sql, pars)>0)
                    return true;
            }
            catch
            {}
            return false;
        }

        public bool DeleteByCondition(Item condition)
        {
            return DeleteByCondition(new List<Item> { condition });
        }

        public bool DeleteByCondition(List<Item> conditions)
        {
            if (conditions.Count <= 0) return false;
            if (!conditions.Exists(p => p.ItemType != ItemType.ConditionItem)) return false;

            List<DbParameter> pars = new List<DbParameter>();
            string conSql = getConditionSQL(conditions, pars);

            if (conSql == "1=1")//若无条件项，不允许删除
            {
                return false;
            }

            string sql = String.Format(deleteSQL, tableName, conSql);

            try
            {
                DBHelper.ExecuteNonQuery(System.Data.CommandType.Text, sql, pars);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 辅助对象及方法

        /// <summary>
        /// 数据库处理对象
        /// </summary>
        protected IDBHelper DBHelper
        {
            get
            {
                return SQLFactory.CreateDBHelper();
            }
        }

        /// <summary>
        /// 用户自定义特性信息
        /// </summary>
        protected List<Attribute.CustomAttribute> CustomFields
        {
            get
            {
                return EntityUtil.GetFieldsByType(typeof(T)).CustomFields;
            }
        }

        /// <summary>
        /// 按用户自定义分类查询自定义特性集合
        /// </summary>
        /// <param name="type">用户自定义分类</param>
        public List<Attribute.CustomAttribute> GetCustomFieldsByType(String type)
        {
            return CustomFields.FindAll(f => f.CusType == type);
        }

        /// <summary>
        /// 获取sql条件字符串
        /// </summary>
        /// <param name="items">sql语句项集合</param>
        /// <param name="pars">添加参数集合</param>
        /// <returns>sql字符串条件及排序部分</returns>
        protected string getConditionSQL(List<Item> items, List<DbParameter> pars)
        {
            StringBuilder sb = new StringBuilder();
            List<Item> conditions = items != null ? items.FindAll(p => p.ItemType == ItemType.ConditionItem) : null;//条件项集合
            List<Item> groups = items != null ? items.FindAll(p => p.ItemType == ItemType.ConditionGroup) : null;//条件组集合
            List<Item> orders = items != null ? items.FindAll(p => p.ItemType == ItemType.OrderItem) : null;//排序项集合

            //条件项集合
            if (conditions != null)
            {
                foreach (ConditionItem item in conditions)
                {
                    item.LoadParameters(sb, pars, JoinType.AND);
                }
            }

            //条件组集合
            if (groups != null)
            {
                foreach (Item group in groups)
                {
                    group.LoadParameters(sb, pars, JoinType.AND);
                }
            }

            if (sb.Length == 0)
            {
                sb.Append("1=1");
            }

            //排序项集合
            StringBuilder osb = new StringBuilder();
            if (orders != null && orders.Count > 0)
            {
                osb.Append(" ORDER BY ");
                foreach (OrderItem item in orders)
                {
                    osb.Append(item.FieldName + " " + item.OrderType.ToString() + ",");
                }
            }

            return sb.ToString() + osb.ToString().TrimEnd(',');
        }

        /// <summary>
        /// 验证实体对象，并返回错误信息
        /// </summary>
        public String Validate(T entity)
        {
            return EntityUtil.ValidateEntity(entity);
        }

        #endregion

        #region 自定义数据库操作
        /// <summary>
        /// 用户自定义sql语句执行（非查询语句）
        /// </summary>
        /// <param name="sql">非查询sql语句</param>
        /// <returns>执行成功的数据行数</returns>
        public static int Excute(String sql)
        {
            return SQLFactory.CreateDBHelper().ExecuteNonQuery(System.Data.CommandType.Text, sql, null);
        }

        /// <summary>
        /// 用户自定义sql语句批量执行（非查询语句）
        /// </summary>
        /// <param name="sql">非查询sql语句集合</param>
        /// <returns>是否成功</returns>
        public static bool Excute(List<String> sqls)
        {
            DbConnection conn = SQLFactory.CreateDBConnection();
            if (conn.State == ConnectionState.Closed) conn.Open();
            DbTransaction tran = conn.BeginTransaction();
            try
            {
                foreach (String sql in sqls)
                {
                    SQLFactory.CreateDBHelper().ExecuteNonQuery(tran,System.Data.CommandType.Text, sql, null);
                }

                tran.Commit();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 用户自定义sql语句查询
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns>查询结果集</returns>
        public static DataTable Query(String sql)
        {
            return SQLFactory.CreateDBHelper().Query(sql, null);
        }
        #endregion

        #region 事务处理

        /// <summary>
        /// 持久化主对象及其的关联对象信息
        /// </summary>
        /// <param name="model">操作主对象</param>
        /// <param name="filedName">关联对象集合</param>
        public bool Persist(OptEntity model, List<AssociationAttribute> associations)
        {
            OptType optType = OptType.Save;

            List<OptEntity> entities = new List<OptEntity>();//实体对象集合
            entities.Add(model);//加入主对象

            if (associations != null && associations.Count > 0)
            {
                Type type = model.Entity.GetType();
                PropertyInfo info;
                dynamic obj;
                dynamic repository;
                foreach (AssociationAttribute item in associations)
                {
                    info = type.GetProperty(item.FieldName);
                    repository = null;
                    if(DBTran == null)
                        repository = RepositoryFactory<EntityBase>.CreateRepository(info.PropertyType.GetGenericArguments()[0]);//此处泛型T无实际作用
                    else
                        repository = RepositoryFactory<EntityBase>.CreateRepository(info.PropertyType.GetGenericArguments()[0], DBTran);//此处泛型T无实际作用

                    obj = info.GetValue(model.Entity, null);
                    if (obj == null)
                        continue;

                    switch (item.AssType)
                    {
                        case AssociationType.Association://关联关系
                            //控制级联类别
                            if (item.CasType == CascadeType.SAVE || item.CasType == CascadeType.UNDELETE || item.CasType == CascadeType.ALL)
                            {
                                if (repository.Exist(obj))
                                    optType = OptType.UpdateAllowedNull;
                                else
                                    optType = OptType.Create;
                            }
                            else if (item.CasType == CascadeType.UPDATE)
                                optType = OptType.UpdateAllowedNull;

                            entities.Add(new OptEntity(obj, optType));
                            break;
                        case AssociationType.Aggregation://聚合关系
                        case AssociationType.Composition://组合关系
                            foreach (dynamic entity in obj)
                            {
                                //控制级联类别
                                if (item.CasType == CascadeType.SAVE || item.CasType == CascadeType.UNDELETE || item.CasType == CascadeType.ALL)
                                {
                                    if (repository.Exist(entity))
                                        optType = OptType.UpdateAllowedNull;
                                    else
                                        optType = OptType.Create;
                                }
                                else if (item.CasType == CascadeType.UPDATE)
                                    optType = OptType.UpdateAllowedNull;

                                entities.Add(new OptEntity(entity, optType));
                            }
                            break;
                    }//end switch
                }//end foreach
            }//end if

            DBTransaction tran = null;
            if (DBTran == null)
            {
                tran = new DBTransaction(DBHelper, entities);
            }
            else
            {
                tran = DBTran;
                tran.entities = entities;
            }
            return tran.Excute();
        }

        #endregion

    }
}
