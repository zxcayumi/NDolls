using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDolls.Core.Util
{
    public class JsonUtil
    {
        private static System.Web.Script.Serialization.JavaScriptSerializer js =
            new System.Web.Script.Serialization.JavaScriptSerializer();

        /// <summary>
        /// 将字典信息转为json对象
        /// </summary>
        /// <param name="dic">字典</param>
        /// <returns>json对象</returns>
        public static String DicToJson(Dictionary<String, object> dic)
        {
            StringBuilder json = new StringBuilder();
            foreach (string key in dic.Keys)
            {
                if (dic[key].GetType() == typeof(String))
                {
                    json.Append("\"" + key + "\":" + "\"" + EncodeJData(dic[key].ToString()) + "\",");
                }
                else
                {
                    json.Append("\"" + key + "\":" + "\"" + js.Serialize(dic[key]) + "\",");
                }
            }

            return "{" + json.ToString().TrimEnd(',') + "}";
        }

        /// <summary>
        /// 特定Model对象转为json字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static String ModelToJson<T>(T model)
        {
            if (model != null)
            {
                return js.Serialize(model);
            }
            else
            {
                return null;
            }
            
        }

        /// <summary>
        /// 将json字符串转换为特定的Model对象
        /// </summary>
        /// <typeparam name="T">特定对象的类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <returns>Model对象</returns>
        public static T JsonToModel<T>(String json)
        {
            try
            {
                T obj = js.Deserialize<T>(json);
                return obj;
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 对json数据内容（非完整的json字符串）进行编码，避免非法字符
        /// </summary>
        /// <param name="s">json数据内容</param>
        /// <returns>编码后的json数据内容</returns>
        public static string EncodeJData(string s)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        int i = (int)c;
                        if (i < 32 || i > 127)
                        {
                            sb.AppendFormat("\\u{0:X04}", i);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }

            return sb.ToString();
        }
    }
}
