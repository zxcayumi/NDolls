using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace NDolls.Data
{
    public class HelperBase
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString
        {
            get 
            {
                return DataConfig.ConnectionString; 
            }
        }

        /// <summary>
        /// 数据库连接
        /// </summary>
        public DbConnection Connection
        {
            get
            {
                return SQLFactory.CreateDBConnection();
            }
        }
    }
}
