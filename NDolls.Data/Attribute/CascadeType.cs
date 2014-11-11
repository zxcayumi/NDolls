using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDolls.Data.Attribute
{
    /// <summary>
    /// 关联对象操作类型
    /// </summary>
    public enum CascadeType
    {
        /// <summary>
        /// 仅作为查询
        /// </summary>
        SELECT,

        /// <summary>
        /// 级联修改
        /// </summary>
        UPDATE,

        /// <summary>
        /// 级联保存(添加、修改)
        /// </summary>
        SAVE,

        /// <summary>
        /// 级联删除
        /// </summary>
        DELETE,

        /// <summary>
        /// 级联非删除操作
        /// </summary>
        UNDELETE,

        /// <summary>
        /// 所有操作级联
        /// </summary>
        ALL,

        /// <summary>
        /// 无级联操作
        /// </summary>
        NONE
    }
}
