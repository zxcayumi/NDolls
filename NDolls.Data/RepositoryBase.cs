using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDolls.Data.Util;
using NDolls.Data.Entity;

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

        /// <summary>
        /// 用户自定义特性信息
        /// </summary>
        public List<Attribute.CustomAttribute> CustomFields
        {
            get
            {
                return EntityUtil.GetFieldsByType(typeof(T)).CustomFields;
            }
        }

        /// <summary>
        /// 验证实体对象，并返回错误信息
        /// </summary>
        public String Validate(T entity)
        {
            return EntityUtil.ValidateEntity(entity);
        }
    }
}
