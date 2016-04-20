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

        #region 数据库相关

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

        private static Boolean allowAssociation = true;
        /// <summary>
        /// 是否开启级联查询
        /// </summary>
        public static Boolean AllowAssociation
        {
            get { return DataConfig.allowAssociation; }
            set { DataConfig.allowAssociation = value; }
        }

        #endregion

        #region 认证相关

        private static String authAssembleName;
        /// <summary>
        /// 操作认证所在程序集名称
        /// </summary>
        public static String AuthAssembleName
        {
            get
            {
                if (!String.IsNullOrEmpty(authAssembleName) && authAssembleName != System.Configuration.ConfigurationManager.AppSettings["AuthAssembleName"])
                {
                    return authAssembleName;
                }
                else
                {
                    return System.Configuration.ConfigurationManager.AppSettings["AuthAssembleName"];
                }
            }
            set
            {
                authAssembleName = value;
            }
        }

        private static String authClassName;
        /// <summary>
        /// 操作认证所在类FullName
        /// </summary>
        public static String AuthClassName
        {
            get
            {
                if (!String.IsNullOrEmpty(authClassName) && authClassName != System.Configuration.ConfigurationManager.AppSettings["AuthClassName"])
                {
                    return authClassName;
                }
                else
                {
                    return System.Configuration.ConfigurationManager.AppSettings["AuthClassName"];
                }
            }
            set
            {
                authClassName = value;
            }
        }

        private static String authMethodName;
        /// <summary>
        /// 操作认证方法名
        /// </summary>
        public static String AuthMethodName
        {
            get
            {
                if (!String.IsNullOrEmpty(authMethodName) && authMethodName != System.Configuration.ConfigurationManager.AppSettings["AuthMethodName"])
                {
                    return authMethodName;
                }
                else
                {
                    return System.Configuration.ConfigurationManager.AppSettings["AuthMethodName"];
                }
            }
            set
            {
                authMethodName = value;
            }
        }

        #endregion
    }

    public enum DBType
    {
        SqlClient,
        SQLite
    }

}
