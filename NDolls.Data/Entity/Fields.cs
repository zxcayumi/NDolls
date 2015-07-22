using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDolls.Data.Attribute;

namespace NDolls.Data.Entity
{
    public class Fields
    {
        public Fields()
        {
            DataFields = new List<DataFieldAttribute>();
            AssociationFields = new List<AssociationAttribute>();
            ValidateFields = new List<ValidateAttribute>();
            OrderFields = new List<OrderAttribute>();
            CustomFields = new List<CustomAttribute>();
        }

        /// <summary>
        /// 数据字段集合
        /// </summary>
        public List<DataFieldAttribute> DataFields { get; set; }

        /// <summary>
        /// 关联字段集合
        /// </summary>
        public List<AssociationAttribute> AssociationFields { get; set; }

        /// <summary>
        /// 验证字段集合
        /// </summary>
        public List<ValidateAttribute> ValidateFields { get; set; }

        /// <summary>
        /// 排序字段集合
        /// </summary>
        public List<OrderAttribute> OrderFields { get; set; }

        /// <summary>
        /// 用户自定义集合
        /// </summary>
        public List<CustomAttribute> CustomFields { get; set; }
    }
}
