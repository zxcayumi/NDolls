using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDolls.Data.Attribute
{
    /// <summary>
    /// 字段验证特性类
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class ValidateAttribute : System.Attribute
    {
        /// <summary>
        /// 构造函数(默认不允许为空)
        /// </summary>
        /// <param name="fieldDesc">字段描述</param>
        public ValidateAttribute(string fieldDesc)
        {
            this.FieldDesc = fieldDesc;
            this.Nullable = false;
            this.Expression = "" ;
        }

        /// <summary>
        /// 构造函数(默认不允许为空)
        /// </summary>
        /// <param name="fieldDesc">字段描述</param>
        /// <param name="expression">正则验证表达式 或 内置正则表达式key</param>
        public ValidateAttribute(string fieldDesc, string expression)
        {
            this.FieldDesc = fieldDesc;
            this.Nullable = false;
            this.Expression = expression;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldName">需验证字段的名称（属性变量名）</param>
        /// <param name="fieldDesc">字段描述</param>
        /// <param name="expression">正则表达式</param>
        public ValidateAttribute(string fieldName, string fieldDesc, string expression)
        {
            this.FieldName = fieldName;
            this.FieldDesc = fieldDesc;
            this.Nullable = false;
            this.Expression = expression;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldDesc">字段描述</param>
        /// <param name="nullable">是否允许为空</param>
        /// <param name="expression">正则表达式</param>
        public ValidateAttribute(string fieldDesc,bool nullable,string expression)
        {
            this.FieldDesc = fieldDesc;
            this.Nullable = nullable;
            this.Expression = expression;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldName">需验证字段的名称（属性变量名）</param>
        /// <param name="fieldDesc">字段描述</param>
        /// <param name="nullable">是否允许为空</param>
        /// <param name="expression">正则表达式</param>
        public ValidateAttribute(string fieldName,string fieldDesc, bool nullable, string expression)
        {
            this.FieldName = fieldName;
            this.FieldDesc = fieldDesc;
            this.Nullable = nullable;
            this.Expression = expression;
        }

        /// <summary>
        /// 属性对应字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 字段描述
        /// </summary>
        public string FieldDesc { get; set; }

        /// <summary>
        /// 是否可为空
        /// </summary>
        public bool Nullable { get; set; }

        /// <summary>
        /// 验证正则表达式
        /// </summary>
        public string Expression { get; set; }
    }
}
