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
    }
}
