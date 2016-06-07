using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDolls.Data.Entity;

namespace NDolls.Data.Attribute
{
    /// <summary>
    /// 字段条件描述(对级联查询进一步细化筛选用)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class AssocConditionAttribute : System.Attribute
    {
        private SearchType searchType = SearchType.Accurate;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldName">被查询对象的条件字段</param>
        /// <param name="fieldValue">被查询对象条件字段值</param>
        /// <param name="searchType">查询方式</param>
        public AssocConditionAttribute(String fieldName,Object fieldValue, SearchType searchType)
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
