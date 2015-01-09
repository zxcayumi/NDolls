using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using NDolls.Data.Attribute;

namespace Test.Model
{
    //ECom_Work
    [Entity("ECom_Work", "WorkID")]
    public class ECom_Work : NDolls.Data.Entity.EntityBase
    {
        /// <summary>
        /// 活动编号
        /// </summary>		
        [DataField("WorkID", "nvarchar")]
        public string WorkID
        { get; set; }

        /// <summary>
        /// 活动类别(接活、放活)
        /// </summary>		
        [DataField("WorkType", "nvarchar")]
        public string WorkType
        { get; set; }

        /// <summary>
        /// 活动信息标题
        /// </summary>		
        [DataField("Title", "nvarchar")]
        public string Title
        { get; set; }

        /// <summary>
        /// 省
        /// </summary>		
        [DataField("Province", "nvarchar")]
        public string Province
        { get; set; }

        /// <summary>
        /// 市
        /// </summary>		
        [DataField("City", "nvarchar")]
        public string City
        { get; set; }

        /// <summary>
        /// 区
        /// </summary>		
        [DataField("District", "nvarchar")]
        public string District
        { get; set; }

        /// <summary>
        /// Address
        /// </summary>		
        [DataField("Address", "nvarchar")]
        public string Address
        { get; set; }

        /// <summary>
        /// 标题颜色(暂不用)
        /// </summary>		
        [DataField("TitleColor", "nvarchar")]
        public string TitleColor
        { get; set; }

        /// <summary>
        /// 行业分类(暂不用)
        /// </summary>		
        [DataField("Classification", "nvarchar")]
        public string Classification
        { get; set; }

        /// <summary>
        /// 详细内容
        /// </summary>		
        [DataField("Contents", "ntext")]
        public string Contents
        { get; set; }

        /// <summary>
        /// 展示图片1
        /// </summary>		
        [DataField("Img1", "nvarchar")]
        public string Img1
        { get; set; }

        /// <summary>
        /// 展示图片2
        /// </summary>		
        [DataField("Img2", "nvarchar")]
        public string Img2
        { get; set; }

        /// <summary>
        /// 展示图片3
        /// </summary>		
        [DataField("Img3", "nvarchar")]
        public string Img3
        { get; set; }

        /// <summary>
        /// 过期日期
        /// </summary>		
        [DataField("ExpirationDate", "datetime")]
        public DateTime ExpirationDate
        { get; set; }

        /// <summary>
        /// 发布用户名
        /// </summary>		
        [DataField("UserName", "nvarchar")]
        public string UserName
        { get; set; }

        /// <summary>
        /// 静态访问地址
        /// </summary>		
        [DataField("HTMLUrl", "nvarchar")]
        public string HTMLUrl
        { get; set; }

        /// <summary>
        /// 短访问地址
        /// </summary>		
        [DataField("ShortUrl", "nvarchar")]
        public string ShortUrl
        { get; set; }

        /// <summary>
        /// 数据状态
        /// </summary>		
        [DataField("Status", "bit")]
        public bool Status
        { get; set; }

        /// <summary>
        /// 备注
        /// </summary>		
        [DataField("Memo", "nvarchar")]
        public string Memo
        { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        [DataField("CreateTime", "datetime")]
        public DateTime CreateTime
        { get; set; }

        /// <summary>
        /// 最近一次修改时间
        /// </summary>		
        [DataField("UpdateTime", "datetime")]
        public DateTime UpdateTime
        { get; set; }

    }
}