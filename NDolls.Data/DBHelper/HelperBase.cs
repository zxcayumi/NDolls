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
        public static string ConnectionString
        {
            get 
            {
                return DataConfig.ConnectionString; 
            }
        }

    }
}
