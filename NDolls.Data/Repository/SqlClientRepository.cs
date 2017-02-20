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
    class SqlClientRepository<T> : RepositoryBase<T>, IRepository<T> where T : EntityBase
    {
        public SqlClientRepository()
            : base()
        { }

        /// <summary>
        /// 构造函数，根据实体类型初始化实体信息(对应表名、表主键)
        /// </summary>
        /// <param name="type">实体类型</param>
        public SqlClientRepository(Type type)
            : base(type)
        { }

        /// <summary>
        /// 构造函数，根据实体类型初始化实体信息(对应表名、表主键)
        /// </summary>
        /// <param name="type">实体类型</param>
        public SqlClientRepository(DBTransaction tran)
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
            string sql = String.Format(selectSQL, "TOP " + top + " *", tableName, customCondition);
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
        /// <param name="pageSize">每页大小</param>
        /// <param name="index">当前页索引</param>
        /// <param name="items">查询（排序）项集合</param>
        /// <returns>查询结果集合</returns>
        public List<T> FindByPage(int pageSize, int index, List<Item> items)
        {
            String sql = "SELECT * FROM " +
                "(SELECT ROW_NUMBER() OVER ({0}) AS rownum ,* FROM " + tableName + " WHERE {1}) AS temp " +
                "WHERE temp.rownum BETWEEN " + ((index - 1) * pageSize + 1) + " AND " + pageSize * index;

            //构造查询条件
            List<DbParameter> pars = new List<DbParameter>();
            //生成查询语句
            string searchSql = getSearchSQL(items, pars);//sql条件部分
            string orderSql = getOrderSQL(items);

            if (String.IsNullOrEmpty(orderSql))//默认按主键排序
            {
                sql = String.Format(sql, "ORDER BY " + primaryKey, searchSql);
            }
            else
            {
                sql = String.Format(sql, orderSql, searchSql);
            }

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
        /// <returns>分页数据信息</returns>
        public Paper<T> FindPager(int pageSize, int index, List<Item> items)
        {
            //String sql = "SELECT * FROM " +
            //    "(SELECT ROW_NUMBER() OVER (ORDER BY " + primaryKey + ") AS rownum ,* FROM " + tableName + " WHERE {0}) AS temp " +
            //    "WHERE temp.rownum BETWEEN " + ((index - 1) * pageSize + 1) + " AND " + pageSize * index;

            ////构造查询条件
            //List<DbParameter> pars = new List<DbParameter>();
            ////生成查询语句
            //string conSql = getConditionSQL(items, pars);//sql条件部分

            //if (conSql.Contains("ORDER"))
            //{
            //    sql = String.Format(sql, conSql.Substring(0, conSql.IndexOf("ORDER")));
            //    sql += " " + conSql.Substring(conSql.IndexOf("ORDER"));
            //}
            List<T> list = FindByPage(pageSize,index,items);//new List<T>();
            //list = DataConvert<T>.ToEntities(DBHelper.Query(sql, pars));
            
            Paper<T> paper = null;
            if (list != null && list.Count > 0)
            {
                paper = new Paper<T>();
                paper.Current = index;
                paper.Total = GetCount(items.FindAll(p => p.ItemType != ItemType.OrderItem));
                paper.PageCount = (int)Math.Ceiling(paper.Total / (decimal)pageSize);
                paper.Result = list;
            }

            return paper;
        }
    }
}
