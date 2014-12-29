using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace NDolls.Data
{
    public class HelperBase
    {
        private static string connectionString;
        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string ConnectionString
        {
            get { return HelperBase.connectionString; }
        }

        static HelperBase()
        {
            if(String.IsNullOrEmpty(DataConfig.ConnectionString))
            {
                connectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
            }
            else
            {
                connectionString = DataConfig.ConnectionString;
            }
        }
    }
}
