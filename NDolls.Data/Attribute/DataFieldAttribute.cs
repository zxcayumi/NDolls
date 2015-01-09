using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDolls.Data.Attribute
{
    /// <summary>
    /// 实体类字段特性描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DataFieldAttribute : System.Attribute
    {
        private string fieldName;//字段名称
        private string fieldType;//字段类型
        private bool isIdentity = false;//是否标识

        /// <summary>
        /// 特性构造函数
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="fieldType">字段类型</param>
        public DataFieldAttribute(string fieldName, string fieldType)
        {
            this.fieldName = fieldName;
            this.fieldType = fieldType;
        }

        /// <summary>
        /// 特性构造函数
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="fieldType">字段类型</param>
        /// <param name="isIdentity">是否标识</param>
        public DataFieldAttribute(string fieldName, string fieldType, bool isIdentity)
        {
            this.fieldName = fieldName;
            this.fieldType = fieldType;
            this.isIdentity = isIdentity;
        }

        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string FieldType
        {
            get { return fieldType; }
            set { fieldType = value; }
        }

        /// <summary>
        /// 是否标识
        /// </summary>
        public bool IsIdentity
        {
            get { return isIdentity; }
            set { isIdentity = value; }
        }
    }
}
