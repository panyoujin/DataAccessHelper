
using BF.DataAccessHelper.Helper;
using BF.DataAccessHelper.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace BF.DataAccessHelper.SQLAnalytical
{
    public class CacheSqlConfig
    {
        private static readonly Lazy<CacheSqlConfig> _instance = new Lazy<CacheSqlConfig>(() => new CacheSqlConfig());
        private static Dictionary<string, XmlNode> _sqlDic = new Dictionary<string, XmlNode>();
        static object _SqlLock = new object();
        private string _sqlConfigPath;
        /// <summary>
        /// SQL配置的路径 将会检索该目录和子目录下的所有xml缓存起来
        /// </summary>
        public string SqlConfigPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_sqlConfigPath))
                {
                    _sqlConfigPath = AppDomain.CurrentDomain.BaseDirectory + "/bin/SqlConfig";
                }
                if (!Directory.Exists(_sqlConfigPath))
                {
                    _sqlConfigPath = AppDomain.CurrentDomain.BaseDirectory + "/SqlConfig";
                }
                if (!Directory.Exists(_sqlConfigPath))
                {
                    _sqlConfigPath = ConfigHelper.GetConfigValue("SqlConfigPath", "");
                    if (string.IsNullOrWhiteSpace(_sqlConfigPath) || !Directory.Exists(_sqlConfigPath))
                    {
                        _sqlConfigPath = AppDomain.CurrentDomain.BaseDirectory;
                    }
                }
                return _sqlConfigPath;
            }
            set
            {
                _sqlConfigPath = value;
            }
        }
        public static CacheSqlConfig Instance
        {
            get
            {
                return _instance.Value;
            }
        }
        private CacheSqlConfig()
        {

        }
        /// <summary>
        /// 通过指定的KEY获取SQL
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isParam"></param>
        /// <returns></returns>
        public string GetSqlByKey(string key, Dictionary<string, object> keyValue)
        {

            var tempKey = key.ToLower();
            //如果缓存的SQL为空，或者在缓存中找不到对应的KEY 就从文件中加载
            if (_sqlDic == null || _sqlDic.Count <= 0 || !_sqlDic.ContainsKey(tempKey))
            {
                SqlConfigInit();
            }
            if (!_sqlDic.ContainsKey(tempKey))
            {
                return key;
                throw new Exception(string.Format("配置中找不到KEY：{0}", key));
            }
            Dictionary<string, object> keyValueTemp = ReplaceInjection(keyValue);
            var sqlDefinition = new SqlDefinition(_sqlDic[tempKey], keyValueTemp);
            return sqlDefinition.SqlCommand;
        }

        /// <summary>
        /// 替换输入字符串中包含的SQL敏感词
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        private Dictionary<string, object> ReplaceInjection(Dictionary<string, object> keyValue)
        {
            Dictionary<string, object> keyValueTemp = new Dictionary<string, object>();
            if (keyValue != null)
            {//临时注释
                var keyTemp = keyValue.Keys.ToList();
                foreach (var dic in keyTemp)
                {
                    if (keyValueTemp.ContainsKey(dic))
                    {
                        keyValueTemp[dic] = keyValue[dic];
                    }
                    else
                    {
                        keyValueTemp.Add(dic, keyValue[dic]);
                    }
                }
            }
            return keyValueTemp;
        }

        /// <summary>
        /// SQL配置加载
        /// </summary>
        private void SqlConfigInit()
        {
            _sqlDic.Clear();//调试的时候报索引 引用超出了界限.临时添加
            GetFileToXml(SqlConfigPath);
        }

        private XmlNode GetFileToXml(string path)
        {
            var dir = new DirectoryInfo(path);
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                if (file.Extension.ToLower() == ".xml")
                {
                    GetXml(file);
                }
            }
            var childDir = dir.GetDirectories();
            foreach (var d in childDir)
            {
                GetFileToXml(d.FullName);
            }
            return null;
        }

        private void GetXml(FileInfo file)
        {
            IConfigRefresher refresher = new FileInfoConfigRefresher(file.FullName);
            if (true || !refresher.IsLatest)
            {
                refresher.Refresh(() =>
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(file.FullName);
                    foreach (XmlNode nodeChild in doc.DocumentElement.ChildNodes)
                    {
                        if (nodeChild.NodeType != XmlNodeType.Element && nodeChild.Name.ToLower() != "data") continue;
                        if (nodeChild.Attributes["name"] == null) continue;
                        var key = XmlUtility.getNodeAttributeStringValue(nodeChild, "name").ToLower();
                        lock (_SqlLock)
                        {
                            if (!_sqlDic.ContainsKey(key))
                            {
                                _sqlDic.Add(key, nodeChild["SqlDefinition"]);
                            }
                            else
                            {
                                _sqlDic[key] = nodeChild["SqlDefinition"];
                            }
                        }

                    }
                });
            }
        }
    }
}
