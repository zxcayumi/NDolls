using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using NDolls.Data.Util;

namespace NDolls.NET
{
    public class WebAPIClient
    {
        /// <summary>
        /// 获取数据对象
        /// </summary>
        /// <typeparam name="T">数据对象类型</typeparam>
        /// <param name="getUrl">WebAPI地址</param>
        /// <returns>获取的数据对象</returns>
        public static string Invoke(String APIUrl)
        {
            System.Net.WebClient c = new System.Net.WebClient();
            c.Encoding = Encoding.UTF8;
            try
            {
                string json = c.DownloadString(APIUrl);
                return json;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取数据对象
        /// </summary>
        /// <typeparam name="T">数据对象类型</typeparam>
        /// <param name="getUrl">WebAPI地址</param>
        /// <returns>获取的数据对象</returns>
        public static T GetModel<T>(String APIUrl)
        {
            System.Net.WebClient c = new System.Net.WebClient();
            c.Encoding = Encoding.UTF8;
            try
            {
                string json = c.DownloadString(APIUrl);
                T obj = DataConvert<T>.JsonToEntity(json);
                return obj;
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 获取数据对象
        /// </summary>
        /// <typeparam name="T">数据对象类型</typeparam>
        /// <param name="getUrl">WebAPI地址</param>
        /// <returns>获取的数据对象</returns>
        public static List<T> GetModelList<T>(String APIUrl)
        {
            System.Net.WebClient c = new System.Net.WebClient();
            c.Encoding = Encoding.UTF8;
            string json = c.DownloadString(APIUrl);
            List<T> list = DataConvert<List<T>>.JsonToEntity(json);

            return list;
        }

        /// <summary>
        /// 数据添加
        /// </summary>
        /// <typeparam name="T">数据对象类型</typeparam>
        /// <param name="model">要添加的数据对象</param>
        /// <param name="APIUrl">WebAPI地址</param>
        public static bool Add<T>(T model, String APIUrl)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(APIUrl);
            string param = DataConvert<T>.EntityToJson(model);
            byte[] bs = Encoding.UTF8.GetBytes(param);
            req.Method = "POST";
            req.ContentType = "text/json";
            req.ContentLength = bs.Length;

            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
                reqStream.Close();
            }

            return Receive(req);
        }

        /// <summary>
        /// 数据修改
        /// </summary>
        /// <typeparam name="T">数据对象类型</typeparam>
        /// <param name="model">要修改的数据对象</param>
        /// <param name="APIUrl">WebAPI地址</param>
        public static bool Update<T>(T model, String APIUrl)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(APIUrl);
            string param = DataConvert<T>.EntityToJson(model);
            byte[] bs = Encoding.UTF8.GetBytes(param);
            req.Method = "PUT";
            req.ContentType = "text/json";
            req.ContentLength = bs.Length;

            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
                reqStream.Close();
            }

            return Receive(req);
        }

        /// <summary>
        /// 按主键删除
        /// </summary>
        /// <param name="pk">主键信息</param>
        /// <param name="APIUrl">WebAPI地址</param>
        /// <returns></returns>
        public static bool Delete(String pk, String APIUrl)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(APIUrl + "?id=" + pk);
            req.Method = "DELETE";

            return Receive(req);
        }

        #region 辅助类
        public static bool Receive(HttpWebRequest req)
        {
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)req.GetResponse();
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
            }

            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                string responseData = reader.ReadToEnd();
            }

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.Created:
                    return true;
                default:
                    return false;
            }
        }
        #endregion
    }
}
