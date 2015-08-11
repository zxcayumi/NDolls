using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDolls.Data.Attribute
{
    /// <summary>
    /// 字段排序描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class OrderAttribute : System.Attribute
    {
        private Entity.OrderType orderType = Entity.OrderType.ASC;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="orderType">排序方式</param>
        public OrderAttribute(Entity.OrderType orderType)
        {
            this.orderType = orderType;
        }

        /// <summary>
        /// 排序方式
        /// </summary>
        public Entity.OrderType OrderType
        {
            get { return orderType; }
            set { orderType = value; }
        }

        /// <summary>
        /// 属性对应字段名
        /// </summary>
        public string FieldName { get; set; }
    }
}
