using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDolls.Cache
{
    public interface ICache
    {
        T Get<T>(String key);
        void Set<T>(String key, T value);
        void Set<T>(String key, T value, int minutes);
        void Remove(String key);
        void Clear();
    }
}
