using System;
using System.Collections.Generic;
using System.Web.Caching;
using System.Text;
using System.Web;
using System.Collections;

namespace NDolls.Cache
{
    public class DotNetCache : ICache
    {
        public T Get<T>(String key)
        {
            object obj = HttpRuntime.Cache[key];
            if (obj != null)
                return (T)obj;
            else
                return default(T);
        }

        public void Set<T>(String key, T value)
        {
            HttpRuntime.Cache.Insert(key, value);
        }

        public void Set<T>(String key, T value,int minutes)
        {
            HttpRuntime.Cache.Insert(key, value, null, DateTime.Now.AddMinutes(minutes), System.Web.Caching.Cache.NoSlidingExpiration); 
        }

        public void Remove(String key)
        {
            HttpRuntime.Cache.Remove(key);
        }

        public void Clear()
        {
            IDictionaryEnumerator cacheEnum = HttpContext.Current.Cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                HttpContext.Current.Cache.Remove(cacheEnum.Key.ToString());
            }
        }
    }
}
