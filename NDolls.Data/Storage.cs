using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDolls.Data.Entity;

namespace NDolls.Data
{
    class Storage
    {
        private static Dictionary<Type, String> tableNameDic = new Dictionary<Type, string>();
        /// <summary>
        /// 数据表名称缓存字典(根据数据类型获取该类型对应的数据库表)
        /// </summary>
        public static Dictionary<Type, String> TableNameDic
        {
            get { return Storage.tableNameDic; }
            set { Storage.tableNameDic = value; }
        }

        private static Dictionary<String, String> primaryKeyDic = new Dictionary<String, string>();
        /// <summary>
        /// 数据表主键缓存字典（根据数据表名称获取该表对应的主键信息）
        /// </summary>
        public static Dictionary<String, String> PrimaryKeyDic
        {
            get { return Storage.primaryKeyDic; }
            set { Storage.primaryKeyDic = value; }
        }

        private static Dictionary<Type, Fields> classFieldsDic = new Dictionary<Type, Fields>();
        /// <summary>
        /// 实体类字段信息字段（根据实体类类型获取其字段信息）
        /// </summary>
        public static Dictionary<Type, Fields> ClassFieldsDic
        {
            get { return Storage.classFieldsDic; }
            set { Storage.classFieldsDic = value; }
        }

        private static Dictionary<String, IDBHelper> dbHelperDic = new Dictionary<string, IDBHelper>();
        /// <summary>
        /// 数据库帮助类字典
        /// </summary>
        internal static Dictionary<String, IDBHelper> DBHelperDic
        {
            get { return Storage.dbHelperDic; }
            set { Storage.dbHelperDic = value; }
        }
    }

    class Messages
    {
        /// <summary>
        /// 是否为空验证错误提示
        /// </summary>
        public static String NullableError
        {
            get { return "不允许为空"; }
        }

        /// <summary>
        /// 正则表达式验证错误提示
        /// </summary>
        public static String ExpressionError
        {
            get { return "内容格式错误"; }
        }
    }
}
