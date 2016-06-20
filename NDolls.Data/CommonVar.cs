using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDolls.Data
{
    public class CommonVar
    {
        /// <summary>
        /// SQL连接符
        /// </summary>
        public static String JoinTag
        {
            get 
            {
                String tag = "+";
                switch (DataConfig.DatabaseType)
                {
                    case DBType.SqlClient:
                        tag = "+";
                        break;
                    case DBType.SQLite:
                        tag = "||";
                        break;
                    default:
                        tag = "+";
                        break;
                }

                return tag;
            }
        }

        private static NDolls.Core.Log nLog = null;
        public static NDolls.Core.Log NLog
        {
            get
            {
                if (nLog == null)
                {
                    nLog = new Core.Log(".", Core.LogMod.Month);
                }
                return nLog;
            }
        }

        private static String isDebug;
        /// <summary>
        /// 是否调试模式（可将sql打印日志）
        /// </summary>
        public static String IsDebug
        {
            get 
            {
                try
                {
                    isDebug = System.Configuration.ConfigurationManager.AppSettings["IsDebug"].ToLower();
                }
                catch
                { }

                return isDebug;
            }
        }

        public static void WriteLog(String content) 
        {
            if (IsDebug == "true")
            {
                NLog.WriteLog(content);
            }
        }
    }
}
