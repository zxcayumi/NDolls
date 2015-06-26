using NDolls.Data.Attribute;
using NDolls.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.Model
{
    /// <summary>
    /// 系统字典表(Sys_Dictionary)Model
    /// </summary>
    [Entity("Sys_Dictionary", "DID")]
    public class Sys_Dictionary : EntityBase
    {
        /// <summary>
        /// 字典项ID(采用无连接符GUID，Guid.NewGuid().ToString(“N”))
        /// </summary>
        [DataField("DID", "nvarchar")]
        [Validate("字典项ID不允许为空")]
        public string DID { get; set; }

        /// <summary>
        /// 是否字典类别说明
        /// </summary>
        [DataField("IsCaption", "bit")]
        [Validate("是否字典类别说明不允许为空")]
        public bool IsCaption { get; set; }

        /// <summary>
        /// 字典类别
        /// </summary>
        [DataField("DType", "nvarchar")]
        [Validate("字典类别不允许为空")]
        public string DType { get; set; }

        /// <summary>
        /// 字典项名称
        /// </summary>
        [DataField("DName", "nvarchar")]
        [Validate("字典项名称不允许为空")]
        public string DName { get; set; }

        /// <summary>
        /// 排列顺序
        /// </summary>
        [DataField("Sequence", "nvarchar")]
        public string Sequence { get; set; }

        /// <summary>
        /// 外部程序集扩展(存储外部动态链接库调用信息，如无特殊情况存空即可)
        /// </summary>
        [DataField("ExtDll", "nvarchar")]
        public string ExtDll { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataField("Memo", "nvarchar")]
        public string Memo { get; set; }

        /// <summary>
        /// 数据状态
        /// </summary>
        [DataField("Status", "int")]
        public int Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataField("CreateTime", "datetime")]
        [Validate("创建时间不允许为空")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最近修改日期
        /// </summary>
        [DataField("UpdateTime", "datetime")]
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 表修改人
        /// </summary>
        [DataField("Modifier", "nvarchar")]
        public string Modifier { get; set; }
    }
}
