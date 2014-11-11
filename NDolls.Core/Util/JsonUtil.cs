using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDolls.Core.Util
{
    public class JsonUtil
    {
        /// <summary>
        /// 将字典信息转为json对象
        /// </summary>
        /// <param name="dic">字典</param>
        /// <returns>json对象</returns>
        public static String ToJson(Dictionary<String, String> dic)
        {
            StringBuilder json = new StringBuilder();
            foreach (string key in dic.Keys)
            {
                json.Append("\"" + key + "\":" + "\"" + dic[key] + "\",");
            }

            return "{" + json.ToString().TrimEnd(',') + "}";
        }
    }
}
