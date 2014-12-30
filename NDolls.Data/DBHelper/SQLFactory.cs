using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace NDolls.Data
{
    public class SQLFactory
    {
        private static DbProviderFactory sqlFactory = 
            DbProviderFactories.GetFactory("System.Data." + Enum.GetName(typeof(DBType), DataConfig.DatabaseType));

        /// <summary>
        /// 创建数据库执行语句参数
        /// </summary>
        public static DbParameter CreateParameter(String pName,object pValue)
        {
            DbParameter param = sqlFactory.CreateParameter();

            param.ParameterName = pName;
            param.Value = pValue;

            return param;
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        public static DbConnection CreateDBConnection()
        {
            DbConnection conn = sqlFactory.CreateConnection();
            conn.ConnectionString = DataConfig.ConnectionString;
            return conn;
        }

        /// <summary>
        /// 创建数据库帮助对象
        /// </summary>
        public static IDBHelper CreateDBHelper()
        {
            String key = "NDolls.Data." + Enum.GetName(typeof(DBType), DataConfig.DatabaseType);
            if (Storage.DBHelperDic.ContainsKey(key))
            {
                return Storage.DBHelperDic[key];
            }
            else
            {
                Type type = Type.GetType("NDolls.Data." + Enum.GetName(typeof(DBType), DataConfig.DatabaseType) + "Helper");
                IDBHelper helper = Activator.CreateInstance(type) as IDBHelper;
                Storage.DBHelperDic[key] = helper;

                return helper;
            }
        }
    }
}
