using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;

namespace DataStoreLib.Utils
{
    public static class CacheManager
    {
        public static void Add<T>(string key, T o)
        {
            HttpRuntime.Cache.Insert(
                key,
                o,
                null,
                DateTime.Now.AddHours(12),
                Cache.NoSlidingExpiration);
        }

        public static void Remove(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }

        public static void Clear()
        {
            List<string> toRemove = new List<string>();
            foreach (DictionaryEntry cacheItem in HttpRuntime.Cache)
            {
                toRemove.Add(cacheItem.Key.ToString());
            }

            foreach (string key in toRemove)
            {
                HttpRuntime.Cache.Remove(key);
            }
        }

        public static bool Exists(string key)
        {
            return HttpRuntime.Cache[key] != null;
        }

        public static bool TryGet<T>(string key, out T value)
        {
            value = (T)HttpRuntime.Cache[key];
            if (value != null)
            {
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }
    }
}
