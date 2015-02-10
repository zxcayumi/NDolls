using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using NDolls.Data.Attribute;

namespace Sample.Data.Model
{
    [Entity("Sys_User","UserName")]
    public class Sys_User:NDolls.Data.Entity.EntityBase
    {
        /// <summary>
        /// UserName
        /// </summary>
        [DataField("UserName", "nvarchar")]
        [Validate("用户名", "Account")]//使用内置预制模式匹配
        [Custom("gridCol","用户名","text")]
        public string UserName
        { get; set; }

        /// <summary>
        /// Password
        /// </summary>		
        [DataField("Password", "nvarchar")]
        public string Password
        { get; set; }

        /// <summary>
        /// UserRole
        /// </summary>		
        [DataField("UserRole", "nvarchar")]
        [Validate("用户角色","应都为小写", "LowerCase")]//用户自定义模式匹配
        [Validate("用户角色","用户角色不符合要求", "^\\w+([+-.]\\w+)*@\\w+([-.]\\w+)*$")]//用户自定义模式匹配
        public string UserRole
        { get; set; }

        /// <summary>
        /// Status
        /// </summary>		
        [DataField("Status", "bit")]
        public bool? Status
        { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>		
        [DataField("CreateTime", "datetime")]
        public DateTime CreateTime
        { get; set; }

        /// <summary>
        /// UpdateTime
        /// </summary>		
        [DataField("UpdateTime", "datetime")]
        public DateTime UpdateTime
        { get; set; }

    }
}