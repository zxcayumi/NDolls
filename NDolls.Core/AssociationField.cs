using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDolls.Core
{
    public class AssociationField
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="fieldType">字段类型</param>
        /// <param name="fieldValue">字段值</param>
        public AssociationField(string fieldName, string fieldType, object fieldValue)
        {
            this.FieldName = fieldName;
            this.FieldType = fieldType;
            this.FieldValue = fieldValue;
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
    }
}
