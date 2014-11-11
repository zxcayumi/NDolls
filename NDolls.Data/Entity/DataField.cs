using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDolls.Data.Entity
{
    public class DataField
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="fieldType">字段类型</param>
        /// <param name="fieldValue">字段值</param>
        public DataField(string fieldName, string fieldType, object fieldValue)
        {
            this.FieldName = fieldName;
            this.FieldType = fieldType;
            this.FieldValue = fieldValue;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="fieldType">字段类型</param>
        /// <param name="fieldValue">字段值</param>
        /// <param name="isIdentity">是否标识</param>
        public DataField(string fieldName, string fieldType, object fieldValue, bool isIdentity)
        {
            this.FieldName = fieldName;
            this.FieldType = fieldType;
            this.FieldValue = fieldValue;
            this.IsIdentity = isIdentity;
        }

        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 字段类型[暂未用到]
        /// </summary>
        public string FieldType { get; set; }

        /// <summary>
        /// 字段值
        /// </summary>
        public object FieldValue { get; set; }

        /// <summary>
        /// 是否标识
        /// </summary>
        public bool IsIdentity { get; set; }
    }
}
