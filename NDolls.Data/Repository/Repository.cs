using System;
using System.Collections.Generic;
using System.Text;
using NDolls.Core;
using System.Data.SqlClient;
using System.Transactions;
using NDolls.Data.Entity;
using NDolls.Data.Util;
using NDolls.Data.Attribute;
using System.Data.Common;

namespace NDolls.Data
{
    /// <summary>
    /// ORM容器处理类（SQLServer）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> : RepositoryBase<T>, IRepository<T> where T : EntityBase
    {
        public Repository()
            : base()
        { }

        /// <summary>
        /// 构造函数，根据实体类型初始化实体信息(对应表名、表主键)
        /// </summary>
        /// <param name="type">实体类型</param>
        public Repository(Type type)
            : base(type)
        {
            tableName = EntityUtil.GetTableName(type);
            primaryKey = EntityUtil.GetPrimaryKey(tableName);
        }

        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns>查询结果集合</returns>
        public List<T> FindAll()
        {
            return FindByCondition(new ConditionItem("1", "1", SearchType.Accurate));
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
        /// 根据查询条件获取对象集合
        /// </summary>
        /// <param name="top">查询数量(0:查询所有)</param>
        /// <param name="conditions">查询（排序）项集合</param>
        /// <returns>查询结果集合</returns>
        public List<T> FindByCondition(int top, List<Item> items)
        {
            //构造查询条件
            List<DbParameter> pars = new List<DbParameter>();

            //生成查询语句
            string conSql = getConditionSQL(items, pars);//sql条件部分
            string sql;
            string fields;
            if (top > 0)
            {
                fields = "TOP " + top + " *";
            }
            else
            {
                fields = "*";
            }

            sql = String.Format(selectSQL, fields, tableName, conSql);

            List<T> list = new List<T>();
            list = DataConvert<T>.ToEntities(DBHelper.Query(sql, pars));

            return list;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageCount">每页大小</param>
        /// <param name="index">当前页索引</param>
        /// <param name="items">查询（排序）项集合</param>
        /// <returns>查询结果集合</returns>
        public List<T> FindByPage(int pageCount, int index, List<Item> items)
        {
            String sql = "SELECT TOP " + pageCount + " * FROM(SELECT row_number() OVER(ORDER BY " + primaryKey + ") row,* FROM " + tableName + " ) tt WHERE row > " + ((index - 1) * 10); ;

            List<T> list = new List<T>();
            list = DataConvert<T>.ToEntities(DBHelper.Query(sql, null));

            return list;
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
                if (field.FieldValue == null || field.FieldType.ToLower().Contains("datetime"))
                    continue;

                if (field.FieldType.ToLower().Contains("varchar"))
                {
                    conditions.Add(new ConditionItem(field.FieldName, field.FieldValue, SearchType.Fuzzy));
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
            pars.Add(new SqlParameter(primaryKey, keyValue));

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
                pars.Add(new SqlParameter(primaryKeys[i], keyValues[i]));
            }
            condition = condition.Substring(0, condition.LastIndexOf("AND"));
            string sql = String.Format(selectSQL, "*", tableName, condition);

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
        /// 用户自定义查询
        /// </summary>
        /// <param name="customCondition">用户自己拼写的查询条件语句</param>
        /// <returns>查询的结果集</returns>
        public List<T> Find(String customCondition)
        {
            string sql = String.Format(selectSQL, "*", tableName, customCondition);
            return DataConvert<T>.ToEntities(DBHelper.Query(sql, null));
        }

        /// <summary>
        /// 执行非查询sql语句
        /// </summary>
        /// <param name="sql">非查询sql语句</param>
        /// <returns>执行成功的数据行数</returns>
        public int Excute(String sql)
        {
            return DBHelper.ExecuteNonQuery(System.Data.CommandType.Text, sql, null);
        }

        /// <summary>
        /// 根据对象主键查询是否存在该对象
        /// </summary>
        /// <param name="keyValue">主键对应的值</param>
        /// <returns>是否存在</returns>
        public bool Exist(T model)
        {
            string sql = String.Format(selectSQL, "COUNT(*)", tableName, primaryKey + "=@" + primaryKey);
            List<DbParameter> pars = new List<DbParameter>();
            pars.Add(new SqlParameter(primaryKey, EntityUtil.GetValueByField(model, primaryKey)));

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

        public bool Update(T model)
        {
            Fields fields = EntityUtil.GetFieldsByType(model.GetType());
            List<AssociationAttribute> assocations = fields.AssociationFields;

            return Persist(new OptEntity(model, OptType.Update), assocations);
        }

        public bool Delete(string keyValue)
        {
            string sql = String.Format(deleteSQL, tableName, primaryKey + "=@" + primaryKey);
            List<DbParameter> pars = new List<DbParameter>();
            pars.Add(new SqlParameter(primaryKey, keyValue));

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

        public bool Delete(string[] keyValues)
        {
            if (primaryKeys.Length != keyValues.Length)
                return false;

            string condition = "";
            List<DbParameter> pars = new List<DbParameter>();
            for (int i = 0; i < keyValues.Length; i++)
            {
                condition += primaryKeys[i] + "=@" + primaryKeys[i] + " AND ";
                pars.Add(new SqlParameter(primaryKeys[i], keyValues[i]));
            }

            string sql = String.Format(deleteSQL, tableName, condition.Substring(0, condition.LastIndexOf("AND")));

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

        /// <summary>
        /// 获取sql条件字符串
        /// </summary>
        /// <param name="items">sql语句项集合</param>
        /// <param name="pars">添加参数集合</param>
        /// <returns>sql字符串条件及排序部分</returns>
        private string getConditionSQL(List<Item> items, List<DbParameter> pars)
        {
            StringBuilder sb = new StringBuilder();
            List<Item> conditions = items.FindAll(p => p.ItemType == ItemType.ConditionItem);//条件项集合
            List<Item> orders = items.FindAll(p => p.ItemType == ItemType.OrderItem);//排序项集合

            if (conditions == null || conditions.Count == 0)
            {
                sb.Append("1=1 AND ");
            }
            else
            {
                String fieldName, parameterName;
                foreach (ConditionItem item in conditions)
                {
                    fieldName = item.FieldName;
                    parameterName = item.FieldName;
                    if (pars.Exists(p => p.ParameterName == parameterName))
                    {
                        parameterName += item.GetHashCode();
                    }

                    switch (item.ConditionType)
                    {
                        case SearchType.Accurate:
                            sb.Append(fieldName + "=@" + parameterName + " AND ");
                            pars.Add(new SqlParameter(parameterName, item.FieldValue));
                            break;
                        case SearchType.Fuzzy:
                            sb.Append(fieldName + " LIKE @" + parameterName + " AND ");
                            pars.Add(new SqlParameter(parameterName, "%" + item.FieldValue + "%"));
                            break;
                        case SearchType.Unequal:
                            sb.Append(fieldName + " <> @" + parameterName + " AND ");
                            pars.Add(new SqlParameter(parameterName, item.FieldValue));
                            break;
                        case SearchType.ValuesIn:
                            sb.Append("(");
                            String[] fiels = item.FieldValue.ToString().Split(',');
                            for (int i = 0; i < fiels.Length; i++)
                            {
                                if (i == (fiels.Length - 1))
                                {
                                    sb.Append(fieldName + " = @" + (parameterName + i));
                                }
                                else
                                {
                                    sb.Append(fieldName + " = @" + (parameterName + i) + " OR ");
                                }
                                pars.Add(new SqlParameter((parameterName + i), fiels[i]));
                            }
                            sb.Append(") AND ");
                            break;
                        case SearchType.ValuesNotIn:
                            sb.Append("(");
                            fiels = item.FieldValue.ToString().Split(',');
                            for (int i = 0; i < fiels.Length; i++)
                            {
                                if (i == (fiels.Length - 1))
                                {
                                    sb.Append(item.FieldName + " <> @" + (item.FieldName + i));
                                }
                                else
                                {
                                    sb.Append(item.FieldName + " <> @" + (item.FieldName + i) + " AND ");
                                }
                                pars.Add(new SqlParameter((item.FieldName + i), fiels[i]));
                            }
                            sb.Append(") AND ");
                            break;
                        case SearchType.Greater:
                            sb.Append(fieldName + " > @" + parameterName + " AND ");
                            pars.Add(new SqlParameter(parameterName, item.FieldValue));
                            break;
                        case SearchType.Lower:
                            sb.Append(fieldName + " < @" + parameterName + " AND ");
                            pars.Add(new SqlParameter(parameterName, item.FieldValue));
                            break;
                        default:
                            break;
                    }
                }
            }

            StringBuilder osb = new StringBuilder();
            if (orders != null && orders.Count > 0)
            {
                osb.Append(" ORDER BY ");
                foreach (OrderItem item in orders)
                {
                    osb.Append(item.FieldName + " " + item.OrderType.ToString() + ",");
                }
            }

            return sb.ToString().Substring(0, sb.ToString().LastIndexOf("AND ")) + osb.ToString().TrimEnd(',');
        }

    }
}
