using DataAccessHelper.Extend;
using MemcachedProviders.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessHelper.Cache.Memcache
{
    public class MemcacheManager
    {
        private static readonly Lazy<MemcacheManager> _instance = new Lazy<MemcacheManager>(() => new MemcacheManager());


        private MemcacheManager()
        {

        }
        public static MemcacheManager Instance
        {
            get
            {
                return _instance.Value;
            }
        }
        #region 缓存key管理
        private static string KeyDic = "YXT_KeyDic";
        public List<MemcacheKey> GetMemcacheKeyLis()
        {
            List<MemcacheKey> MemcacheKeyList = Get<List<MemcacheKey>>(KeyDic);
            if (MemcacheKeyList == null)
            {
                MemcacheKeyList = new List<MemcacheKey>();
            }
            return MemcacheKeyList;
        }
        public void AddKeyDic(string key, string tbNames = "", string description = "", long expireTime = 60)
        {
            try
            {
                AsyncAddKeyDic(key, tbNames, description, expireTime);
            }
            catch (Exception ex)
            {
                try
                {
                    SyncAddKeyDic(key, tbNames, description, expireTime);
                }
                catch (Exception sex)
                {
                    sex.Source = sex.Source + ex.ToJson();
                    throw sex;
                }
            }
        }
        public async void AsyncAddKeyDic(string key, string tbNames = "", string description = "", long expireTime = 60)
        {
            try
            {
                await Task.Delay(1);

                if (string.IsNullOrWhiteSpace(key))
                {
                    return;
                }
                List<MemcacheKey> MemcacheKeyList = GetMemcacheKeyLis();
                if (MemcacheKeyList == null)
                {
                    MemcacheKeyList = new List<MemcacheKey>();
                }
                //清除过期的key
                DateTime cTime = DateTime.MinValue;
                MemcacheKeyList = MemcacheKeyList.Where(k =>
                {
                    DateTime.TryParse(k.CreateTime, out cTime);
                    return cTime > DateTime.Now.AddMinutes(-1 * k.ExpireTime);
                }).ToList();
                var item = MemcacheKeyList.Where(m => m.Key == key).SingleOrDefault();
                if (item != null)
                {

                    item.Key = key;
                    item.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    item.RelationTableName = tbNames;
                    item.ExpireTime = expireTime;
                    item.Description = description;

                }
                else
                {
                    MemcacheKeyList.Add(new MemcacheKey()
                    {
                        Key = key,
                        CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        RelationTableName = tbNames,
                        ExpireTime = expireTime,
                        Description = description
                    });
                }
                var a = DistCache.Add(KeyDic, MemcacheKeyList);
            }
            catch
            {

            }
        }

        public void SyncAddKeyDic(string key, string tbNames = "", string description = "", long expireTime = 60)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(key))
                {
                    return;
                }
                List<MemcacheKey> MemcacheKeyList = GetMemcacheKeyLis();
                if (MemcacheKeyList == null)
                {
                    MemcacheKeyList = new List<MemcacheKey>();
                }
                //清除过期的key
                DateTime cTime = DateTime.MinValue;
                MemcacheKeyList = MemcacheKeyList.Where(k =>
                {
                    DateTime.TryParse(k.CreateTime, out cTime);
                    return cTime > DateTime.Now.AddMinutes(-1 * k.ExpireTime);
                }).ToList();
                var item = MemcacheKeyList.Where(m => m.Key == key).SingleOrDefault();
                if (item != null)
                {

                    item.Key = key;
                    item.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    item.RelationTableName = tbNames;
                    item.ExpireTime = expireTime;
                    item.Description = description;

                }
                else
                {
                    MemcacheKeyList.Add(new MemcacheKey()
                    {
                        Key = key,
                        CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        RelationTableName = tbNames,
                        ExpireTime = expireTime,
                        Description = description
                    });
                }
                var a = DistCache.Add(KeyDic, MemcacheKeyList, 60 * 24);
            }
            catch
            {

            }
        }

        /// <summary>
        /// 根据表名称清除对应的缓存，在添加缓存的时候加上涉及到的表名称
        /// </summary>
        /// <param name="tbName"></param>
        public void RemoveKeyDicByTbName(string tbName)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(tbName))
                {
                    return;
                }
                tbName = tbName.ToLower();
                List<MemcacheKey> MemcacheKeyList = GetMemcacheKeyLis();

                var keyList = MemcacheKeyList.Where(t => t.RelationTableName.ToLower().Contains(tbName)).Select(m => m.Key).ToList();

                MemcacheKeyList.RemoveAll(t => t.RelationTableName.ToLower().Contains(tbName));
                DistCache.Add(KeyDic, MemcacheKeyList);

                keyList = keyList.Distinct().ToList();
                foreach (var item in keyList)
                {
                    DistCache.Remove(item);
                }

            }
            catch
            {

            }
        }
        /// <summary>
        /// 批量指定KEY清除缓存
        /// </summary>
        /// <param name="keys"></param>
        public void RemoveKeyDic(string keys)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keys))
                {
                    return;
                }
                List<MemcacheKey> MemcacheKeyList = GetMemcacheKeyLis();
                var keyArray = keys.Split(',').ToList();
                MemcacheKeyList.RemoveAll(m => !string.IsNullOrWhiteSpace(m.Key) && keyArray.Contains(m.Key));
                DistCache.Add(KeyDic, MemcacheKeyList);
                foreach (var key in keyArray)
                {
                    if (!string.IsNullOrWhiteSpace(keys))
                    {
                        DistCache.Remove(key);
                    }
                }
            }
            catch
            {

            }
        }
        /// <summary>
        /// 模糊匹配删除缓存
        /// </summary>
        /// <param name="key"></param>
        public void RemoveLikeKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return;
            }
            var MemcacheKeyList = GetMemcacheKeyLis();

            var keyList = MemcacheKeyList.Where(m => m.Key.ToLower().IndexOf(key.ToLower()) >= 0).Select(mm => mm.Key).ToList();

            MemcacheKeyList.RemoveAll(m => keyList.Contains(m.Key));
            DistCache.Add(KeyDic, MemcacheKeyList);

            foreach (var item in keyList)
            {
                DistCache.Remove(item);
            }
        }
        #endregion


        public void Add(string key, object value, string tbNames = "", string description = "")
        {
            if (value == null || string.IsNullOrWhiteSpace(key))
            {
                return;
            }
            var a = DistCache.Add(key, value);
            AddKeyDic(key, tbNames, description, DistCache.DefaultExpireTime);
        }

        public void Add(string key, object value, int expireTime, string tbNames = "", string description = "")
        {
            if (value == null || string.IsNullOrWhiteSpace(key))
            {
                return;
            }
            var a = DistCache.Add(key, value, TimeSpan.FromMinutes(expireTime));
            AddKeyDic(key, tbNames, description, expireTime);
        }

        public object Get(string key)
        {
            return DistCache.Get(key);
        }

        public T Get<T>(string key)
        {
            return DistCache.Get<T>(key);
        }

        public IDictionary<string, object> Get(string[] key)
        {
            return DistCache.Get(key);
        }

        public void Remove(string key)
        {
            var a = DistCache.Remove(key);
            RemoveKeyDic(key);
        }

        public void RemoveAll()
        {
            DistCache.RemoveAll();
        }

        public bool TryGet<T>(string key, out T result)
        {
            bool flag = false;
            result = this.Get<T>(key);
            if (result != null)
            {
                if (result is IEnumerable<T>)
                {
                    if ((result as IEnumerable<T>).Count() > 0)
                        flag = true;
                }
                else
                {
                    flag = true;
                }
            }
            return flag;
        }

        public bool CompressAdd(string key, object value, string tbNames = "", string description = "")
        {
            if (value == null || string.IsNullOrWhiteSpace(key))
            {
                return false;
            }
            AddKeyDic(key, tbNames, description, DistCache.DefaultExpireTime);
            byte[] compressData = CompressHelper.Compress(value);
            var rst = DistCache.Add(key, compressData);
            return rst;
        }

        public bool CompressAdd(string key, object value, int expireTime, string tbNames = "", string description = "")
        {
            if (value == null || string.IsNullOrWhiteSpace(key))
            {
                return false;
            }
            AddKeyDic(key, tbNames, description, expireTime);
            byte[] compressData = CompressHelper.Compress(value);
            var rst = DistCache.Add(key, compressData, TimeSpan.FromMinutes(expireTime));
            return rst;
        }

        public T DecompressGet<T>(string key)
        {
            var decompressData = DistCache.Get<byte[]>(key);
            if (decompressData != null)
                return CompressHelper.Decompress<T>(decompressData);
            //return default(T);
            else
                return default(T);
        }

    }
}
