using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;

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
                    json.Append("\"" + key + "\":" + "\"" + EncodeData(dic[key].ToString()) + "\",");
                }
                else
                {
                    json.Append("\"" + key + "\":" + TransDate(js.Serialize(dic[key])) + ",");
                }
            }

            return "{" + json.ToString().TrimEnd(',') + "}";
        }

        /// <summary>
        /// 特定Model对象转为json字符串
        /// </summary>
        public static String ModelToJson<T>(T model)
        {
            if (model != null)
            {
                return TransDate(js.Serialize(model));
            }
            else
            {
                return null;
            }            
        }

        /// <summary>
        /// List对象集合转为json字符串
        /// </summary>
        public static String ListToJson<T>(List<T> list)
        {
            if (list != null)
            {
                return TransDate(js.Serialize(list));
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
        /// dataTable转换成Json格式  
        /// </summary>
        public static string TableToJson(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"");
            jsonBuilder.Append(dt.TableName);
            jsonBuilder.Append("\":[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }

        /// <summary>
        /// 将Date(****)类型的日期转为年月日格式
        /// </summary>
        private static String TransDate(String str)
        {
            str = Regex.Replace(str, @"\\/Date\((-?\d+)\)\\/", match =>
            {
                DateTime dt = new DateTime(1970, 1, 1);
                dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                dt = dt.ToLocalTime();
                if (!dt.ToString().Contains("0001"))
                {
                    return dt.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    return "";
                }
            });

            return str;
        }

        /// <summary>
        /// 对json数据内容（非完整的json字符串）进行编码，避免非法字符
        /// </summary>
        /// <param name="s">json数据内容</param>
        /// <returns>编码后的json数据内容</returns>
        public static string EncodeData(string s)
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
