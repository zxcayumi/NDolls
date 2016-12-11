using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Data.SqlClient;

namespace NDolls.Data.Util
{
    public class NetTest
    {

        #region 采用Socket方式，测试服务器连接 
        /// <summary> 
        /// 采用Socket方式，测试服务器连接 
        /// </summary> 
        /// <param name="host">服务器主机名或IP</param> 
        /// <param name="port">端口号</param> 
        /// <param name="millisecondsTimeout">等待时间：毫秒</param> 
        /// <returns></returns> 
        private static bool TestConnection(string host, int port, int millisecondsTimeout)
        {
            TcpClient client = new TcpClient();
            try
            {
                var ar = client.BeginConnect(host, port, null, null);
                ar.AsyncWaitHandle.WaitOne(millisecondsTimeout);
                return client.Connected;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                client.Close();
            }

        }

        public static bool TestConnection()
        {
            string ip = DataConfig.ConnectionString.Split(';')[0].Split('=')[1];
            string[] strs = ip.Split(',');
            int port = 1433;
            if (strs.Length > 1)
            {
                ip = strs[0];
                port = int.Parse(strs[1]);
            }
            if(TestConnection(ip,port,500))
            {
                return true;
            }
            else
            {
                throw new Exception("DB Connection Error");
            }
        }
        #endregion

        /// <summary> 
        /// 数据库连接操作，可替换为你自己的程序 
        /// </summary> 
        /// <param name="ConnectionString">连接字符串</param> 
        /// <returns></returns> 
        public static bool TestConnection(string ConnectionString)
        {
            bool result = true;
            try
            {
                SqlConnection m_myConnection = new SqlConnection(ConnectionString);
                m_myConnection.Open();
                //数据库操作...... 
                m_myConnection.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                result = false;
            }
            return result;
        }
    }
}
