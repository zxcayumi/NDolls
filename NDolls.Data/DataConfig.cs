using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDolls.Data
{
    /// <summary>
    /// 数据库配置文件（设置后，运行期间全局有效）
    /// </summary>
    public class DataConfig
    {
        static DataConfig()
        {
            if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["DBType"]))
            {
                databaseType = (DBType)Enum.Parse(typeof(DBType),System.Configuration.ConfigurationManager.AppSettings["DBType"].ToString());
            }

            if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]))
            {
                connctionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
            }
        }

        private static DBType databaseType = DBType.SqlClient;
        /// <summary>
        /// 数据库类别
        /// </summary>
        public static DBType DatabaseType
        {
            get 
            {
                if (Enum.GetName(typeof(DBType), databaseType) != System.Configuration.ConfigurationManager.AppSettings["DBType"])
                {
                    return databaseType;
                }
                else
                {
                    return (DBType)Enum.Parse(typeof(DBType), System.Configuration.ConfigurationManager.AppSettings["DBType"]);
                }
            }
            set { DataConfig.databaseType = value; }
        }

        private static String connctionString;
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static String ConnectionString
        {
            get
            {
                if (!String.IsNullOrEmpty(connctionString) && connctionString != System.Configuration.ConfigurationManager.AppSettings["ConnectionString"])
                {
                    return connctionString;
                }
                else
                {
                    return System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
                }
            }
            set
            {
                connctionString = value;
            }
        }
    }

    public enum DBType
    {
        SqlClient,
        SQLite
    }

}
