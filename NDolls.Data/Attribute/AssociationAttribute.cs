using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDolls.Data.Entity;

namespace NDolls.Data.Attribute
{
    public class AssociationAttribute : System.Attribute
    {
        ///// <summary>
        ///// 关联特性构造函数
        ///// </summary>
        ///// <param name="refField">与关联对象关联的字段名</param>
        ///// <param name="associationType">关联对象类别</param>
        ///// <param name="cascadeType">关联对象级联操作方式</param>
        //public AssociationAttribute(string refField, AssociationType associationType, CascadeType cascadeType)
        //{
        //    this.RefField = refField;
        //    this.AssType = associationType;
        //    this.CasType = cascadeType;
        //}

        ///// <summary>
        ///// 关联特性构造函数(默认只查询关联对象)
        ///// </summary>
        ///// <param name="fieldName">与关联对象关联的字段名</param>
        ///// <param name="associationType">关联对象类别</param>
        //public AssociationAttribute(string refField, AssociationType associationType)
        //{
        //    this.RefField = refField;
        //    this.AssType = associationType;
        //    this.CasType = CascadeType.SELECT;
        //}

        /// <summary>
        /// 关联特性构造函数(默认为级联查询)
        /// </summary>
        /// <param name="curField">当前主对象字段名</param>
        /// <param name="objField">关联对象字段名</param>
        /// <param name="associationType">关联对象类别</param>
        public AssociationAttribute(string curField, string objField, AssociationType associationType)
        {
            this.CurField = curField;
            this.ObjField = objField;
            this.AssType = associationType;
            this.CasType = CascadeType.SELECT;
        }

        /// <summary>
        /// 关联特性构造函数(默认为级联查询)
        /// </summary>
        /// <param name="top">查询数量</param>
        /// <param name="associationType">关联对象类别</param>
        public AssociationAttribute(int top, AssociationType associationType)
        {
            this.CurField = "";
            this.ObjField = "";
            this.Top = top;
            this.AssType = associationType;
            this.CasType = CascadeType.SELECT;
        }

        /// <summary>
        /// 关联特性构造函数(默认为级联查询)
        /// </summary>
        /// <param name="curField">当前主对象字段名</param>
        /// <param name="objField">关联对象字段名</param>
        /// <param name="top">查询数量</param>
        /// <param name="associationType">关联对象类别</param>
        public AssociationAttribute(string curField, string objField, int top, AssociationType associationType)
        {
            this.CurField = curField;
            this.ObjField = objField;
            this.Top = top;
            this.AssType = associationType;
            this.CasType = CascadeType.SELECT;
        }

        /// <summary>
        /// 关联特性构造函数
        /// </summary>
        /// <param name="curFieldName">当前类字段</param>
        /// <param name="objField">关联类关联字段</param>
        /// <param name="associationType">关联对象类别</param>
        /// <param name="cascadeType">关联对象级联操作方式</param>
        public AssociationAttribute(string curFieldName, string objField, AssociationType associationType, CascadeType cascadeType)
        {
            this.CurField = curFieldName;
            this.ObjField = objField;
            this.AssType = associationType;
            this.CasType = cascadeType;
        }

        /// <summary>
        /// 配置关联特性属性的变量名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 当前对象引用字段名
        /// </summary>
        public string CurField { get; set; }

        /// <summary>
        /// 查询数量
        /// </summary>
        public int Top { get; set; }

        /// <summary>
        /// 关联目标对象字段名
        /// </summary>
        public string ObjField { get; set; }

        /// <summary>
        /// 关联关系类别
        /// </summary>
        public AssociationType AssType { get; set; }

        /// <summary>
        /// 关联对象级联操作方式
        /// </summary>
        public CascadeType CasType { get; set; }

    }
}
