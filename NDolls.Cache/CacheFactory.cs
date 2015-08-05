using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDolls.Cache
{
    public class CacheFactory
    {
        public static ICache CreateCache()
        {
            String cacheType = System.Configuration.ConfigurationManager.AppSettings["CacheType"];
            if (String.IsNullOrEmpty(cacheType))
            {
                cacheType = "DotNet";
            }

            Type type = Type.GetType("NDolls.Cache." + cacheType + "Cache");
            object obj = Activator.CreateInstance(type);

            return obj as ICache;
        }
    }
}
