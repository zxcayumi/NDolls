using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDolls.Data.Entity;

namespace NDolls.Data
{
    public static class RepositoryFactory<T> where T:EntityBase
    {
        private static Dictionary<string, IRepository<T>> repositories = new Dictionary<string, IRepository<T>>();

        /// <summary>
        /// 获取Repository容器
        /// </summary>
        /// <param name="key">容器key</param>
        /// <returns>对应的Repository容器</returns>
        public static IRepository<T> CreateRepository(string key)
        {
            if (repositories.ContainsKey(key))
            {
                return repositories[key];
            }
            else
            {
                IRepository<T> r = new Repository<T>();
                repositories.Add(key, r);
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
            if (repositories.ContainsKey(type.Name))
            {
                return repositories[type.Name];
            }
            else
            {
                //此处需改进，动态适应多数据库(Repository对应的是SQLServer数据库)？？？？
                Type dyType = typeof(Repository<>).MakeGenericType(type);
                return Activator.CreateInstance(dyType);
            }
        }

    }
}
