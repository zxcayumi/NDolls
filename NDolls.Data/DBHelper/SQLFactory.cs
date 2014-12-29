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
    }
}
