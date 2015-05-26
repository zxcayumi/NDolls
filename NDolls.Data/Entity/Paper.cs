using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDolls.Data.Entity
{
    public class Paper<T> where T : EntityBase
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total
        { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int Current
        { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        { get; set; }

        /// <summary>
        /// 当前页对象集合
        /// </summary>
        public List<T> Result
        { get; set; }
    }
}
