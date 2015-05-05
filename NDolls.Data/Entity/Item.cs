using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace NDolls.Data.Entity
{
    /// <summary>
    /// 查询项
    /// </summary>
    public abstract class Item
    {
        /// <summary>
        /// 查询项类别
        /// </summary>
        public ItemType ItemType { get; set; }
        
        /// <summary>
        /// 加载生成SQL参数
        /// </summary>
        /// <param name="sb">SQL语句</param>
        /// <param name="pars">SQL参数集合</param>
        /// <param name="joinType">查询项连接类型</param>
        public abstract void LoadParameters(StringBuilder sb,List<DbParameter> pars,JoinType joinType);
    }

    /// <summary>
    /// 查询项类别
    /// </summary>
    public enum ItemType
    {
        ConditionItem,
        ConditionGroup,
        OrderItem
    }

    /// <summary>
    /// 查询项连接类型
    /// </summary>
    public enum JoinType
    {
        AND,
        OR,
        None
    }
}
