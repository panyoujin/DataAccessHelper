using System;

namespace DataAccessHelper.Cache.Memcache
{
    [Serializable]
    public class MemcacheKey
    {
        public string CKey { get; set; }
        /// <summary>
        /// 缓存KEY
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 缓存新增时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 关联表名称
        /// </summary>
        public string RelationTableName { get; set; }
        /// <summary>
        /// 到期时间
        /// </summary>
        public long ExpireTime { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
