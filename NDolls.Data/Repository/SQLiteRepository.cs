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
    class SQLiteRepository<T> : RepositoryBase<T>, IRepository<T> where T : EntityBase
    {
        public SQLiteRepository()
            : base()
        { }

        /// <summary>
        /// 构造函数，根据实体类型初始化实体信息(对应表名、表主键)
        /// </summary>
        /// <param name="type">实体类型</param>
        public SQLiteRepository(Type type)
            : base(type)
        { }

        /// <summary>
        /// 构造函数，根据实体类型初始化实体信息(对应表名、表主键)
        /// </summary>
        /// <param name="type">实体类型</param>
        public SQLiteRepository(DBTransaction tran)
            : base()
        {
            this.DBTran = tran;
        }

        /// <summary>
        /// 用户自定义查询
        /// </summary>
        /// <param name="top">查询数量</param>
        /// <param name="customCondition">用户自定义条件</param>
        /// <returns>查询的结果集</returns>
        public List<T> Find(int top, String customCondition)
        {
            if (String.IsNullOrEmpty(customCondition))
            {
                customCondition = "1 = 1";
            }

            string sql, conSql="";
            if (top > 0)
            {
                conSql += " ORDER BY " + primaryKey + " LIMIT 0," + top;
            }

            sql = String.Format(selectSQL, "*", tableName, conSql);
            return DataConvert<T>.ToEntities(DBHelper.Query(sql, null));
        }

        /// <summary>
        /// 根据查询条件获取对象集合
        /// </summary>
        /// <param name="top">查询数量(0:查询所有)</param>
        /// <param name="conditions">查询（排序）项集合</param>
        /// <returns>查询结果集合</returns>
        public override List<T> FindByCondition(int top, List<Item> items)
        {
            //构造查询条件
            List<DbParameter> pars = new List<DbParameter>();

            //生成查询语句
            string conSql = getConditionSQL(items, pars);//sql条件部分
            string sql;
            if (top > 0)
            {
                conSql += " ORDER BY " + primaryKey + " LIMIT 0," + top;
            }

            sql = String.Format(selectSQL, "*", tableName, conSql);

            List<T> list = new List<T>();
            list = DataConvert<T>.ToEntities(DBHelper.Query(sql, pars));

            return list;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">每页大小</param>
        /// <param name="index">当前页索引</param>
        /// <param name="items">查询（排序）项集合</param>
        /// <returns>查询结果集合</returns>
        public List<T> FindByPage(int pageSize, int index, List<Item> items)
        {
            String sql = "SELECT * FROM " + tableName + " WHERE {0} LIMIT " + pageSize + " OFFSET " + (index - 1) * pageSize;

            //构造查询条件
            List<DbParameter> pars = new List<DbParameter>();
            //生成查询语句
            string conSql = getConditionSQL(items, pars);//sql条件部分

            sql = String.Format(sql, conSql);
            List<T> list = new List<T>();
            list = DataConvert<T>.ToEntities(DBHelper.Query(sql, pars));

            return list;
        }
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">每页大小</param>
        /// <param name="index">当前页索引</param>
        /// <param name="items">查询（排序）项集合</param>
        /// <returns>查询的分页数据</returns>
        public Paper<T> FindPager(int pageSize, int index, List<Item> items)
        {
            String sql = "SELECT * FROM " + tableName + " WHERE {0} LIMIT " + pageSize + " OFFSET " + (index - 1) * pageSize;

            //构造查询条件
            List<DbParameter> pars = new List<DbParameter>();
            //生成查询语句
            string conSql = getConditionSQL(items, pars);//sql条件部分

            sql = String.Format(sql, conSql);
            List<T> list = new List<T>();
            list = DataConvert<T>.ToEntities(DBHelper.Query(sql, pars));

            Paper<T> paper = null;
            if (list != null && list.Count > 0)
            {
                paper = new Paper<T>();
                paper.Current = index;
                paper.Total = GetCount(items);
                paper.PageCount = (int)Math.Ceiling(paper.Total / (decimal)pageSize);
                paper.Result = list;
            }

            return paper;
        }
    }
}
