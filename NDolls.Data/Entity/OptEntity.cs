using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDolls.Data.Entity
{
    public class OptEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="entity">操作实体</param>
        /// <param name="type">操作类型</param>
        public OptEntity(EntityBase entity, OptType type)
        {
            this.Entity = entity;
            this.Type = type;
        }

        /// <summary>
        /// 实体对象
        /// </summary>
        public EntityBase Entity { get; set; }

        /// <summary>
        /// 数据库操作类型
        /// </summary>
        public OptType Type { get; set; }
    }

    public enum OptType
    {
        /// <summary>
        /// 添加操作
        /// </summary>
        Create,

        /// <summary>
        /// 修改操作
        /// </summary>
        UpdateAllowedNull,

        /// <summary>
        /// 忽略Null值的修改操作
        /// </summary>
        UpdateIgnoreNull,

        /// <summary>
        /// 删除操作
        /// </summary>
        Delete,

        /// <summary>
        /// 保存操作（自动判断添加还是修改）
        /// </summary>
        Save
    }
}
