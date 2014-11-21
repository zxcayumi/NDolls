using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDolls.Data.Entity
{
    /// <summary>
    /// 项基类
    /// </summary>
    public class Item
    {
        /// <summary>
        /// 项类别
        /// </summary>
        protected ItemType itemType { get; set; }

    }

    /// <summary>
    /// 项类别
    /// </summary>
    public enum ItemType
    {
        ConditionItem,
        OrderItem
    }
}
