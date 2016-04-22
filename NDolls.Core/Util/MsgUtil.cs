using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDolls.Core.Util
{
    public class MsgUtil
    {
        /// <summary>
        /// 向前台返回html格式信息
        /// </summary>
        /// <param name="msg">html数据</param>
        public static void ShowHTML(System.Web.HttpContext page, string html)
        {
            page.Response.ContentType = "text/html";
            page.Response.Write(html);
            //page.Response.Flush();
        }

        /// <summary>
        /// 向前台输出json数据
        /// </summary>
        /// <param name="msg">json数据</param>
        public static void ShowJson(System.Web.HttpContext page, string msg)
        {
            msg = msg.Replace("\n", "\\n");
            msg = msg.Replace("\r", "\\r");

            page.Response.ContentType = "application/json";
            page.Response.Write(msg);
            //page.Response.Flush();
        }
    }
}
