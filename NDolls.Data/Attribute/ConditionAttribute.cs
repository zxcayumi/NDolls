using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDolls.Data.Entity;

namespace NDolls.Data.Attribute
{
    /// <summary>
    /// 字段条件描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ConditionAttribute : System.Attribute
    {
        private SearchType searchType = SearchType.Accurate;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldName">条件字段</param>
        /// <param name="fieldValue">条件内容</param>
        /// <param name="searchType">查询方式</param>
        public ConditionAttribute(String fieldName, Object fieldValue, SearchType searchType)
        {
            this.FieldName = fieldName;
            this.FieldValue = fieldValue;
            this.searchType = searchType;
        }

        /// <summary>
        /// 排序方式
        /// </summary>
        public Entity.SearchType SearchType
        {
            get { return searchType; }
            set { searchType = value; }
        }

        /// <summary>
        /// 属性对应字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 属性对应字段值
        /// </summary>
        public Object FieldValue { get; set; }
    }
}
