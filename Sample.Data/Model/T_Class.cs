using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using NDolls.Data.Attribute;
using NDolls.Data.Entity;

namespace Sample.Data.Model
{
    [Entity("T_Class", "ID")]
    public class T_Class : EntityBase
    {

        /// <summary>
        /// 系统编号
        /// </summary>		
        [DataField("ID", "nvarchar")]
        [Validate("不允许为空")]
        public string ID { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>		
        [DataField("ClassName", "nvarchar")]
        public string ClassName { get; set; }

        /// <summary>
        /// 班级类别
        /// </summary>		
        [DataField("ClassType", "nvarchar")]
        public string ClassType { get; set; }

        /// <summary>
        /// 班主任
        /// </summary>		
        [DataField("Teacher", "nvarchar")]
        public string Teacher { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>		
        [DataField("CreatDate", "datetime")]
        public DateTime CreatDate { get; set; }

        /// <summary>
        /// 学生数量
        /// </summary>		
        [DataField("StuCount", "int")]
        public int StuCount { get; set; }
    }
}