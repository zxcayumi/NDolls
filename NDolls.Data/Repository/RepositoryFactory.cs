using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDolls.Data.Entity;

namespace NDolls.Data
{
    public static class RepositoryFactory<T> where T:EntityBase
    {
        private static Dictionary<string, object> repositories = new Dictionary<string, object>();

        /// <summary>
        /// 获取Repository容器
        /// </summary>
        /// <param name="key">容器key</param>
        /// <returns>对应的Repository容器</returns>
        public static IRepository<T> CreateRepository(string key)
        {
            if (repositories.ContainsKey(key))
            {
                return (IRepository<T>)repositories[key];
            }
            else
            {
                Type sd = typeof(T);
                Type type = Type.GetType("NDolls.Data." + DataConfig.DatabaseType + "Repository`1").MakeGenericType(typeof(T));
                IRepository<T> r = Activator.CreateInstance(type) as IRepository<T>;
                repositories.Add(key, r);
                return r;
            }
        }

        /// <summary>
        /// 获取Repository容器
        /// </summary>
        /// <param name="key">容器key</param>
        /// <returns>对应的Repository容器</returns>
        public static IRepository<T> CreateRepository(string key,DBTransaction tran)
        {
            if (repositories.ContainsKey(key + "tran"))
            {
                return (IRepository<T>)repositories[key + "tran"];
            }
            else
            {
                Type sd = typeof(T);
                Type type = Type.GetType("NDolls.Data." + DataConfig.DatabaseType + "Repository`1").MakeGenericType(typeof(T));
                IRepository<T> r = Activator.CreateInstance(type, tran) as IRepository<T>;
                repositories.Add(key + "tran", r);
                return r;
            }
        }

        /// <summary>
        /// 获取指定类型的容器Repository容器
        /// </summary>
        /// <param name="type">指定的类型</param>
        /// <returns>指定类型的容器动态类</returns>
        public static object CreateRepository(Type type)
        {
            if (repositories.ContainsKey(type.FullName))
            {
                return repositories[type.FullName];
            }
            else
            {
                Type dtyp = Type.GetType("NDolls.Data." + DataConfig.DatabaseType + "Repository`1").MakeGenericType(type);
                object obj = Activator.CreateInstance(dtyp);
                repositories.Add(type.FullName, obj);
                return obj;
            }
        }

        /// <summary>
        /// 获取指定类型的容器Repository容器
        /// </summary>
        /// <param name="type">指定的类型</param>
        /// <returns>指定类型的容器动态类</returns>
        public static object CreateRepository(Type type,DBTransaction tran)
        {
            if (repositories.ContainsKey(type.FullName + "tran"))
            {
                return repositories[type.FullName + "tran"];
            }
            else
            {
                Type dtyp = Type.GetType("NDolls.Data." + DataConfig.DatabaseType + "Repository`1").MakeGenericType(type);
                object obj = Activator.CreateInstance(dtyp, tran);
                repositories.Add(type.FullName + "tran", obj);
                return obj;
            }
        }

    }
}
