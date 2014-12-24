using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDolls.Data.Attribute
{
    /// <summary>
    /// 用户自定义特性类
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class CustomAttribute : System.Attribute
    {
        /// <summary>
        /// 自定义信息构造函数
        /// </summary>
        /// <param name="cusType">用户自定义分类</param>
        /// <param name="cusName">用户自定义名称</param>
        /// <param name="cusValue">用户自定义值</param>
        /// <param name="cusMemo">备注</param>
        public CustomAttribute(String cusType,String cusName,String cusValue,String cusMemo)
        {
            this.CusType = cusType;
            this.CusName = cusName;
            this.CusValue = cusValue;
            this.CusMemo = cusMemo;
        }

        /// <summary>
        /// 自定义信息构造函数
        /// </summary>
        /// <param name="cusType">用户自定义分类</param>
        /// <param name="cusName">用户自定义名称</param>
        /// <param name="cusValue">用户自定义值</param>
        public CustomAttribute(String cusType, String cusName, String cusValue)
        {
            this.CusType = cusType;
            this.CusName = cusName;
            this.CusValue = cusValue;
        }

        /// <summary>
        /// 属性对应字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 用户自定义分类
        /// </summary>
        public string CusType { get; set; }

        /// <summary>
        /// 用户自定义名称
        /// </summary>
        public string CusName { get; set; }

        /// <summary>
        /// 用户自定义值
        /// </summary>
        public string CusValue { get; set; }

        /// <summary>
        /// 用户自定义备注
        /// </summary>
        public string CusMemo { get; set; }
    }
}
