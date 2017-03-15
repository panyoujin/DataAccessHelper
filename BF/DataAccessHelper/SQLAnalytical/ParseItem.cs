using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DataAccessHelper.SQLAnalytical
{
    /// <summary>
    /// SQLCommand 中需要解析的项
    /// </summary>
    public class ParseItem
    {
        #region property define
        /// <summary>
        /// 整个被<%=%>包围起来的内容
        /// </summary>
        public string ItemContent
        {
            get;
            set;
        }
        /// <summary>
        /// 需要被解析出来被替换的关键字
        /// </summary>
        private List<KeywordVariable> _keywords = new List<KeywordVariable>();
        public List<KeywordVariable> Keywords
        {
            get { return _keywords; }
            set { _keywords = value; }
        }
        private Dictionary<string, object> _keyValue;
        /// <summary>
        /// 关键字和值的集合 解析SQL使用
        /// </summary>
        public Dictionary<string, object> KeyValue
        {
            get
            {
                return _keyValue ?? new Dictionary<string, object>();
            }
        }
        #endregion

        #region ctor
        public ParseItem(string fullContent, Dictionary<string, object> keyValue)
        {
            this._keyValue = keyValue;
            this.ItemContent = fullContent;
            ResolveKeywords(fullContent);
        }
        #endregion

        #region method
        /// <summary>
        /// 将文本中的关键字全部解析出来
        /// </summary>
        /// <param name="fullContent"></param>
        private void ResolveKeywords(string fullContent)
        {
            Regex rgKeyword = new Regex("@@.*?@@");
            MatchCollection mc = rgKeyword.Matches(fullContent);
            foreach (Match m in mc)
            {
                var keyword = new KeywordVariable(m.Value);
                keyword.IsNull = fullContent.Length != m.Value.Length;
                if (KeyValue != null && KeyValue.ContainsKey(keyword.KeyName))
                {
                    keyword.Value = KeyValue[keyword.KeyName];
                }
                _keywords.Add(keyword);
            }
        }
        /// <summary>
        /// 返回解析后的结果
        /// </summary>
        /// <param name="adapter"></param>
        /// <returns></returns>
        public virtual string GetResult(string type, bool isParam = true)
        {
            ///至一个全部参数为空的标记，如果全部参数为空，则整个
            ///解析串返回空
            bool allEmpty = false;
            ///如果根本没有参数，直接返回串内容
            if (this.Keywords.Count == 0) return this.ItemContent;
            string returnValue = ItemContent;
            foreach (KeywordVariable keyItem in this.Keywords)
            {
                string result = keyItem.Value + "";
                if (string.IsNullOrEmpty(result) && keyItem.IsNull) allEmpty = true;
                if (isParam)
                {
                    switch (type.ToLower())
                    {
                        case "sqlserver":
                            returnValue = returnValue.Replace(keyItem.Keyword, string.Format("@{0}", keyItem.KeyName));
                            break;
                        case "mysql":
                            returnValue = returnValue.Replace(keyItem.Keyword, string.Format("?{0}", keyItem.KeyName));
                            break;
                        default:
                            returnValue = returnValue.Replace(keyItem.Keyword, string.Format("@{0}", keyItem.KeyName));
                            break;
                    }
                }
                else
                {
                    returnValue = returnValue.Replace(keyItem.Keyword, string.Format("{0}", keyItem.Value));
                }
            }
            if (allEmpty) return string.Empty;

            return returnValue;
        }
        #endregion
    }
}
