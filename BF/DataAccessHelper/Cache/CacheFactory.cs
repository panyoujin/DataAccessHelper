using DataAccessHelper.Interface;
using System;

namespace DataAccessHelper.Cache
{
    public class CacheFactory
    {
        private static readonly ICacheBase _cacheBase = GetCacheBase();

        private static ICacheBase GetCacheBase()
        {
            ICacheBase cacheBase = null;
            switch (CacheConfig.CacheType)
            {
                case "Memcache":
                    cacheBase = null;// MySqlDBBase.Instance;
                    break;
                default:
                    throw new Exception("暂不支持" + CacheConfig.CacheType + "缓存");
            }
            return cacheBase;
        }
        private CacheFactory()
        {

        }
        public static ICacheBase CacheBase
        {
            get
            {
                return _cacheBase;
            }
        }
    }
}
