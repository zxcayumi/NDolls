using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDolls.Data.Entity
{
    /// <summary>
    /// 查询排序项
    /// </summary>
    public class OrderItem : Item
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldName">查询字段(数据库列)名称</param>
        /// <param name="orderType">字段排序类别</param>
        public OrderItem(string fieldName, OrderType orderType)
        {
            this.ItemType = ItemType.OrderItem;

            this.FieldName = fieldName;
            this.OrderType = orderType;
        }

        public override void LoadParameters(StringBuilder sb, List<System.Data.Common.DbParameter> pars, JoinType joinType)
        {
            
        }
        
        /// <summary>
        /// 查询项字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 排序类别
        /// </summary>
        public OrderType OrderType { get; set; }
    }

    /// <summary>
    /// 排序类别
    /// </summary>
    public enum OrderType
    {
        ASC,
        DESC
    }
}
