using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

namespace NDolls.Data.Util
{
    /// <summary>
    /// 数据转换类
    /// </summary>
    /// <typeparam name="T">实体类类型</typeparam>
    public class DataConvert<T> 
    {
        private static System.Web.Script.Serialization.JavaScriptSerializer js =
            new System.Web.Script.Serialization.JavaScriptSerializer();

        /// <summary>
        /// 数据行对应到实体类对象
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>实体对象</returns>
        public static T ToEntity(DataRow row)
        {
            Type type = typeof(T);
            T model = System.Activator.CreateInstance<T>();

            foreach (PropertyInfo pi in type.GetProperties())
            {
                if (!row.Table.Columns.Contains(pi.Name) || row[pi.Name] == null || row[pi.Name].ToString() == "")
                    continue;

                try
                {
                    //判断字段类型是否可以为空类型
                    if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    {
                        if (row[pi.Name] != null)
                        {
                            System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(pi.PropertyType);
                            pi.SetValue(model, Convert.ChangeType(row[pi.Name], nullableConverter.UnderlyingType), null);
                        }
                    }
                    else
                    {
                        pi.SetValue(model, Convert.ChangeType(row[pi.Name], pi.PropertyType), null);
                    }

                }
                catch
                { }
            }

            return model;
        }

        /// <summary>
        /// 数据集合对应到实体类对象集合
        /// </summary>
        /// <param name="dt">数据集合</param>
        /// <returns>实体类对象集合</returns>
        public static List<T> ToEntities(DataTable dt)
        {
            List<T> list = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                T model = ToEntity(row);

                #region 级联对象加载
                //关联对象加载
                EntityUtil.SetAssociation(model);
                #endregion

                list.Add(model);
            }

            return list;
        }

        /// <summary>
        /// 将json转化为具体对象
        /// </summary>
        /// <param name="json">json格式字符串对象</param>
        /// <returns>类对象</returns>
        public static T JsonToEntity(String json)
        {
            T obj = js.Deserialize<T>(json);
            return obj;
        }

        /// <summary>
        /// 将对象转化为json
        /// </summary>
        /// <param name="json">json格式字符串对象</param>
        /// <returns>json对象</returns>
        public static String EntityToJson(T obj)
        {
            String json = js.Serialize(obj);
            return json;
        }

        /// <summary>
        /// 将对象集合转化为json字符串
        /// </summary>
        /// <param name="objs">对象结合</param>
        /// <returns>json字符串</returns>
        public static String EntitiesToJson(List<T> objs)
        {
            String json = js.Serialize(objs);
            return json;
        }
    }
}
