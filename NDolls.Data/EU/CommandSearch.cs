using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDolls.Data;
using NDolls.Data.Entity;

namespace NDolls.Data.EU
{
    public class CommandSearch<T> : Command where T : EntityBase
    {
        private List<Item> conditions;
        private String[] keys;
        private String fields = "";
        private String conditionSql;

        /// <summary>
        /// 构造函数（返回List）
        /// </summary>
        /// <param name="conditions">查询条件集合</param>
        public CommandSearch(List<Item> conditions)
        {
            this.conditions = conditions;
        }

        /// <summary>
        /// 构造函数（返回DataTable）
        /// </summary>
        /// <param name="fields">自定义查询字段</param>
        /// <param name="conditions">查询条件集合</param>
        public CommandSearch(String fields,List<Item> conditions)
        {
            this.fields = fields;
            this.conditions = conditions;
        }
        
        /// <summary>
        /// 构造函数（返回Model）
        /// </summary>
        /// <param name="keys">主键</param>
        public CommandSearch(String[] keys)
        {
            this.keys = keys;
        }

        /// <summary>
        /// 构造函数（返回DataTable）
        /// </summary>
        /// <param name="conditionSql">自定义查询SQL条件</param>
        public CommandSearch(String conditionSql)
        {
            this.conditionSql = conditionSql;
        }

        private int pageSize = 0;
        /// <summary>
        /// 每页数据量
        /// </summary>
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        private int pageIndex = 0;
        /// <summary>
        /// 页索引
        /// </summary>
        public int PageIndex
        {
            get { return pageIndex; }
            set { pageIndex = value; }
        }

        /// <summary>
        /// 命令执行
        /// </summary>
        public override void Execute(DBTransaction tran)
        {
            IRepository<T> r = RepositoryFactory<T>.CreateRepository(tran);
            
            if(keys != null && keys.Length > 0)//按主键
            {
                Result = r.FindByPK(keys);
            }
            else if (!String.IsNullOrEmpty(conditionSql))
            {
                Result = r.FindByCustom("*", conditionSql);
            }
            else
            {
                if (pageSize <= 0 || pageIndex <= 0)
                {
                    if (!String.IsNullOrEmpty(fields))
                    {
                        Result = r.FindByCustom(fields, conditions);
                    }
                    else
                    {
                        Result = r.FindByCondition(conditions);//List<T>
                    }
                }
                else
                {
                    Result = r.FindPager(pageSize, pageIndex, conditions);//Paper<T>
                }
            }
        }

        public override string ToString()
        {
            return "SEARCH";
        }
    }
}
