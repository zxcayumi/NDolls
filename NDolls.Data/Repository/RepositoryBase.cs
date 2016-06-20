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
        /// <param name="items">查询（排序）项集合</param>
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
        /// <param name="item">查询（排序）项</param>
        /// <returns>查询结果集合</returns>
        public List<T> FindByCondition(Item item)
        {
            return FindByCondition(0, new List<Item> { item });
        }

        /// <summary>
        /// 根据查询条件获取对象集合
        /// </summary>
        /// <param name="items">查询（排序）项集合</param>
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
            List<Item> conditions = Util.EntityUtil.ModelToCondition<T>(model);

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
        /// <param name="keyValues">主键对应的值</param>
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
            items = items.FindAll(p => p.ItemType != ItemType.OrderItem);
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
                    type.GetMethod("AddOrUpdate",new Type[]{m.GetType()}).Invoke(obj, new object[] { m });
                }

                tran.TransactionCommit();
                ClearCache(tran, entities);//事务结束清理Repository缓存
                return true;
            }
            catch(Exception ex)
            {
                tran.TransactionRollback();
                ClearCache(tran, entities);//事务结束清理Repository缓存
                throw ex;
                //return false;
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
        /// <param name="type">更新策略</param>
        /// <returns>修改是否成功</returns>
        public bool Update(T model, OptType type)
        {
            Fields fields = EntityUtil.GetFieldsByType(model.GetType());
            List<AssociationAttribute> assocations = fields.AssociationFields;

            return Persist(new OptEntity(model, type), assocations);
        }

        /// <summary>
        /// 按条件批量更新（将model不为空的数据更新至数据库）
        /// </summary>
        public bool UpdateByCondition(T model, List<Item> items)
        {
            List<DbParameter> pars = new List<DbParameter>();

            String sets = getSetValueSql(model, pars);//Update赋值相关部分
            String condition = getConditionSQL(items, pars);//Update条件相关部分
            String sql = String.Format(updateSQL, tableName, sets, condition);

            try
            {
                if (DBTran == null)
                {
                    return DBHelper.ExecuteNonQuery(System.Data.CommandType.Text, sql, pars) > 0;
                }
                else
                {
                    return DBHelper.ExecuteNonQuery(DBTran.Transaction, System.Data.CommandType.Text, sql, pars) > 0;
                }
            }
            catch
            { }

            return false;
        }

        /// <summary>
        /// 按条件批量更新（将model不为空的数据更新至数据库）
        /// </summary>
        public bool UpdateByCondition(T model, Item item)
        {
            return UpdateByCondition(model, new List<Item>() { item });
        }

        /// <summary>
        /// 按条件批量更新（将model不为空的数据更新至数据库）
        /// </summary>
        public bool UpdateByCondition(T model, String sqlCondition)
        {
            List<DbParameter> pars = new List<DbParameter>();

            String sets = getSetValueSql(model, pars);//Update赋值相关部分
            String sql = String.Format(updateSQL, tableName, sets, sqlCondition);

            try
            {
                if (DBTran == null)
                {
                    return DBHelper.ExecuteNonQuery(System.Data.CommandType.Text, sql, pars) > 0;
                }
                else
                {
                    return DBHelper.ExecuteNonQuery(DBTran.Transaction, System.Data.CommandType.Text, sql, pars) > 0;
                }
            }
            catch
            { }

            return false;
        }

        /// <summary>
        /// 按条件批量更新
        /// </summary>
        public bool UpdateByCondition(Dictionary<String, Object> model, List<Item> items)
        {
            List<DbParameter> pars = new List<DbParameter>();

            String sets = getSetValueSql(model, pars);//Update赋值相关部分
            String condition = getConditionSQL(items, pars);//Update条件相关部分
            String sql = String.Format(updateSQL, tableName, sets, condition);

            try
            {
                if (DBTran == null)
                {
                    return DBHelper.ExecuteNonQuery(System.Data.CommandType.Text, sql, pars) > 0;
                }
                else
                {
                    return DBHelper.ExecuteNonQuery(DBTran.Transaction, System.Data.CommandType.Text, sql, pars) > 0;
                }
            }
            catch
            { }

            return false;
        }

        /// <summary>
        /// 按条件批量更新
        /// </summary>
        public bool UpdateByCondition(Dictionary<String, Object> model, Item item)
        {
            return UpdateByCondition(model, new List<Item>() { item });
        }

        /// <summary>
        /// 按条件批量更新
        /// </summary>
        public bool UpdateByCondition(Dictionary<String, Object> model, String sqlCondition)
        {
            List<DbParameter> pars = new List<DbParameter>();

            String sets = getSetValueSql(model, pars);//Update赋值相关部分
            String sql = String.Format(updateSQL, tableName, sets, sqlCondition);

            try
            {
                if (DBTran == null)
                {
                    return DBHelper.ExecuteNonQuery(System.Data.CommandType.Text, sql, pars) > 0;
                }
                else
                {
                    return DBHelper.ExecuteNonQuery(DBTran.Transaction, System.Data.CommandType.Text, sql, pars) > 0;
                }
            }
            catch
            { }

            return false;
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
        /// 添加或修改（自动判断对象是否存在）
        /// </summary>
        /// <param name="model">实体对象</param>
        /// <param name="type">更新策略</param>
        /// <returns>执行是否成功</returns>
        public bool AddOrUpdate(T model, OptType type)
        {
            if (!Exist(model))
            {
                return Add(model);
            }
            else
            {
                return Update(model, type);
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
                if (DBTran == null)
                {
                    return DBHelper.ExecuteNonQuery(System.Data.CommandType.Text, sql, pars) > 0;
                }
                else
                {
                    return DBHelper.ExecuteNonQuery(DBTran.Transaction, System.Data.CommandType.Text, sql, pars) > 0;
                }
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
                if (DBTran == null)
                {
                    return DBHelper.ExecuteNonQuery(System.Data.CommandType.Text, sql, pars) > 0;
                }
                else
                {
                    return DBHelper.ExecuteNonQuery(DBTran.Transaction, System.Data.CommandType.Text, sql, pars) > 0;
                }
            }
            catch
            {}
            return false;
        }

        /// <summary>
        /// 用户自定义删除
        /// </summary>
        /// <param name="sqlCondition">自定义sql条件</param>
        /// <returns>删除是否成功</returns>
        public bool DeleteByCondition(String sqlCondition)
        {
            //禁止无条件删除
            if (String.IsNullOrEmpty(sqlCondition)) return false;

            string sql = String.Format(deleteSQL, tableName, sqlCondition);

            try
            {
                if (DBTran == null)
                {
                    return DBHelper.ExecuteNonQuery(System.Data.CommandType.Text, sql, null) > 0;
                }
                else
                {
                    return DBHelper.ExecuteNonQuery(DBTran.Transaction, System.Data.CommandType.Text, sql, null) > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 用户自定义删除
        /// </summary>
        /// <param name="condition">查询条件项</param>
        /// <returns>删除是否成功</returns>
        public bool DeleteByCondition(Item condition)
        {
            return DeleteByCondition(new List<Item> { condition });
        }

        /// <summary>
        /// 用户自定义删除
        /// </summary>
        /// <param name="conditions">查询条件项集合</param>
        /// <returns>删除是否成功</returns>
        public bool DeleteByCondition(List<Item> conditions)
        {
            if (conditions.Count <= 0) return false;
            if (!conditions.Exists(p => p.ItemType == ItemType.ConditionItem)) return false;

            List<DbParameter> pars = new List<DbParameter>();
            string conSql = getConditionSQL(conditions, pars);

            if (conSql == "1=1")//若无条件项，不允许删除
            {
                return false;
            }

            string sql = String.Format(deleteSQL, tableName, conSql);

            try
            {
                if (DBTran == null)
                {
                    return DBHelper.ExecuteNonQuery(System.Data.CommandType.Text, sql, pars) > 0;
                }
                else
                {
                    return DBHelper.ExecuteNonQuery(DBTran.Transaction, System.Data.CommandType.Text, sql, pars) > 0;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 辅助对象及方法
        
        /// <summary>
        /// 获取sql条件字符串
        /// </summary>
        /// <param name="items">sql语句项集合</param>
        /// <param name="pars">添加参数集合</param>
        /// <returns>sql字符串条件及排序部分</returns>
        protected String getConditionSQL(List<Item> items, List<DbParameter> pars)
        {
            object[] infos = typeof(T).GetCustomAttributes(typeof(ConditionAttribute),false);
            ConditionAttribute atr;
            foreach (object obj in infos)
            {
                atr = obj as ConditionAttribute;
                items.Add(new ConditionItem(atr.FieldName,atr.FieldValue,atr.SearchType));
            }

            String searchSql = getSearchSQL(items, pars);
            String ordeSql = getOrderSQL(items);

            return searchSql + " " + ordeSql;
        }

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
        /// 根据Dictionary获取Update的SQL语句赋值部分（取mode中不为null的属性）
        /// </summary>
        protected String getSetValueSql(Dictionary<String,Object> dic, List<DbParameter> pars)
        {
            StringBuilder fs = new StringBuilder();//Update赋值相关部分
            foreach (String key in dic.Keys)//构造SQL参数集合
            {
                if (dic[key] != null && dic[key].ToString() != "")
                {
                    fs.Append(key + "=@" + key + ",");
                    pars.Add(SQLFactory.CreateParameter(key, dic[key].ToString()));
                }
            }

            return fs.ToString().TrimEnd(',');
        }

        /// <summary>
        /// 根据Model获取Update的SQL语句赋值部分（取mode中不为null的属性）
        /// </summary>
        protected String getSetValueSql(T model, List<DbParameter> pars)
        {
            StringBuilder fs = new StringBuilder();//Update赋值相关部分
            foreach (DataField field in EntityUtil.GetDataFields(model))//构造SQL参数集合
            {
                if (!field.IsIdentity)
                {
                    if (field.FieldValue == null || (field.FieldType == "datetime" && field.FieldValue.ToString().Contains("0001")))
                    {
                        continue;
                    }
                    else
                    {
                        fs.Append(field.FieldName + "=@" + field.FieldName + ",");
                        pars.Add(SQLFactory.CreateParameter(field.FieldName, field.FieldValue));
                    }
                }
            }

            return fs.ToString().TrimEnd(',');
        }

        /// <summary>
        /// 添加model中配置的排序项
        /// </summary>
        /// <param name="items"></param>
        private void addOrderItems(List<Item> items)
        {
            List<Item> orderItems = items != null ? items.FindAll(p => p.ItemType == ItemType.OrderItem) : null;//条件项集合

            Fields fields = EntityUtil.GetFieldsByType(typeof(T));
            Item item = null;
            foreach (OrderAttribute oa in fields.OrderFields)
            {
                item = orderItems.Find(p => ((OrderItem)p).FieldName == oa.FieldName);
                if (item == null)
                {
                    items.Add(new OrderItem(oa.FieldName, oa.OrderType));
                }
            }
        }

        /// <summary>
        /// 获取查询语句
        /// </summary>
        /// <param name="items">查询条件项集合</param>
        /// <param name="pars">参数化查询参数集合</param>
        /// <returns>查询SQL语句</returns>
        protected String getSearchSQL(List<Item> items, List<DbParameter> pars)
        {
            StringBuilder sb = new StringBuilder();
            List<Item> conditions = items != null ? items.FindAll(p => p.ItemType == ItemType.ConditionItem) : null;//条件项集合
            List<Item> groups = items != null ? items.FindAll(p => p.ItemType == ItemType.ConditionGroup) : null;//条件组集合

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

            return sb.ToString();
        }

        /// <summary>
        /// 获取sql条件字符串
        /// </summary>
        /// <param name="items">sql语句项集合</param>
        /// <returns>sql字符串条件及排序部分</returns>
        protected String getOrderSQL(List<Item> items)
        {
            addOrderItems(items);

            List<Item> orders = items != null ? items.FindAll(p => p.ItemType == ItemType.OrderItem) : null;//排序项集合

            //排序项集合
            StringBuilder osb = new StringBuilder();
            if (orders != null && orders.Count > 0)
            {
                osb.Append("ORDER BY ");
                foreach (OrderItem item in orders)
                {
                    osb.Append(item.FieldName + " " + item.OrderType.ToString() + ",");
                }
            }

            return osb.ToString().TrimEnd(',');
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
        /// <param name="associations">关联对象集合</param>
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
