using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDolls.Data.Attribute
{
    /// <summary>
    /// 实体类特性描述
    /// </summary>
    public class EntityAttribute : System.Attribute
    {
        private string tableName;
        private string pK;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tableName">数据表名称</param>
        /// <param name="pk">主键</param>
        public EntityAttribute(string tableName, string pk)
        {
            this.tableName = tableName;
            this.pK = pk;
        }

        /// <summary>
        /// 主键
        /// </summary>
        public string PK
        {
            get { return pK; }
            set { pK = value; }
        }

        /// <summary>
        /// 类名称
        /// </summary>
        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }
    }
}
