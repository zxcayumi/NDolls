using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDolls.Data.Entity
{
    /// <summary>
    /// 查询条件项
    /// </summary>
    public class ConditionItem : Item
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldName">查询字段(数据库列)名称</param>
        /// <param name="fieldValue">查询字段值</param>
        /// <param name="conditionType">字段查询类别</param>
        public ConditionItem(string fieldName, object fieldValue, SearchType conditionType)
        {
            this.ItemType = ItemType.ConditionItem;

            this.FieldName = fieldName;
            this.FieldValue = fieldValue;
            this.ConditionType = conditionType;
        }

        /// <summary>
        /// 查询项字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 查询项值
        /// </summary>
        public object FieldValue { get; set; }

        /// <summary>
        /// 查询条件类型
        /// </summary>
        public SearchType ConditionType { get; set; }
    }

    public enum SearchType
    {
        /// <summary>
        /// 精确查询
        /// </summary>
        Accurate,

        /// <summary>
        /// 模糊查询
        /// </summary>
        Fuzzy,

        /// <summary>
        /// 不等于
        /// </summary>
        Unequal,

        /// <summary>
        /// 包含其中值
        /// </summary>
        ValuesIn,

        /// <summary>
        /// 不包含其中值
        /// </summary>
        ValuesNotIn,

        /// <summary>
        /// 大于
        /// </summary>
        Greater,

        /// <summary>
        /// 小于
        /// </summary>
        Lower,

        /// <summary>
        /// 大于等于
        /// </summary>
        GreaterEqual,

        /// <summary>
        /// 小于等于
        /// </summary>
        LowerEqual
    }
}
