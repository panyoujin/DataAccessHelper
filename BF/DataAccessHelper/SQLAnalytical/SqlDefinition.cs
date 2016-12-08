using BF.DataAccessHelper.Helper;
using BF.DataAccessHelper.Utilities;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;

namespace BF.DataAccessHelper.SQLAnalytical
{
    /// <summary>
    /// SQL 节点定义
    /// </summary>
    [Serializable]
    public class SqlDefinition
    {
        /// <summary>
        /// SQL 的类型枚举
        /// </summary>
        public enum CommandType
        {
            Sql,
            StoreProcedure
        }
        #region ctor..
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="node"></param>
        public SqlDefinition(XmlNode node, Dictionary<string, object> keyValue)
        {
            this._keyValue = keyValue;
            Parse(node);
            _parseItems = GetAllParseItem(this._sql);
        }

        #endregion

        #region property define
        private readonly List<ParseItem> _parseItems = null;
        public List<ParseItem> ParseItems
        {
            get { return _parseItems; }
        }
        /// <summary>
        /// 执行的SQL命令
        /// </summary>
        private string _sql;
        public string SqlCommand
        {
            get
            {
                string outSql = _sql;
                if (this.ParseItems == null || this.ParseItems.Count <= 0) return outSql;
                foreach (ParseItem item in this.ParseItems)
                {
                    outSql = outSql + item.GetResult();
                }
                return outSql;
            }
            private set { _sql = value; }
        }
        public CommandType SqlType
        {
            get;
            set;
        }
        private ParameterInfoCollection _parameters;
        /// <summary>
        /// SQL 命令的参数
        /// </summary>
        public ParameterInfoCollection Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
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

        #region method define
        private void Parse(XmlNode node)
        {
            this.SqlCommand = XmlUtility.getNodeStringValue(node["SqlCommand"]);
            this.SqlType = (CommandType)Enum.Parse(typeof(CommandType), XmlUtility.getNodeAttributeStringValue(node, "type", "Sql"), true);
            SqlConfig.SqlConnStringName = XmlUtility.getNodeAttributeStringValue(node, "ConnStringName", ConfigHelper.GetConfigValue("ConnStringName", "BOSDbContext"));
            _parameters = new ParameterInfoCollection();
            ///添加参数集合
            foreach (XmlNode nodepara in XmlUtility.getSubNodeList(node, "Parameters/Parameter"))
            {
                _parameters.Add(new ParameterInfo(nodepara));
            }
        }

        /// <summary>
        /// 解析出SQL语句中需要待解析的内容
        /// </summary>
        /// <param name="sqlText"></param>
        /// <param name="isParam">是否参数化  flase:否 true:是</param>
        /// <returns></returns>
        private List<ParseItem> GetAllParseItem(string sqlText)
        {
            string returnSql = sqlText;
            ///试用正则表达式先找出关键字,关键字必须使用<%= %>包含起来
            ///for example : select * from user where (1=1) <%=User.Id=@id%>
            Regex regKeyword = new Regex("<%=.*?%>");
            //string afterReplace=regKeyword.Replace(sqlText, new MatchEvaluator(MatchKeyword));
            MatchCollection mc = regKeyword.Matches(sqlText);
            List<ParseItem> returnResult = new List<ParseItem>();
            foreach (Match c in mc)
            {
                string matchingSql = c.ToString();
                ///在原始的SQL中取出掉这些待解析的SQL
                var parseItem = new ParseItem(c.Value.Replace("<%=", "").Replace("%>", ""), this.KeyValue);
                //returnResult.Add(parseItem);
                _sql = _sql.Replace(matchingSql, parseItem.GetResult());
            }
            regKeyword = new Regex("<R%=.*?%R>");
            mc = regKeyword.Matches(_sql);
            foreach (Match c in mc)
            {
                string matchingSql = c.ToString();
                ///在原始的SQL中取出掉这些待解析的SQL
                var parseItem = new ParseItem(c.Value.Replace("<R%=", "").Replace("%R>", ""), this.KeyValue);
                //returnResult.Add(parseItem);
                _sql = _sql.Replace(matchingSql, parseItem.GetResult(false));
            }

            regKeyword = new Regex("@@.*?@@");
            mc = regKeyword.Matches(_sql);
            foreach (Match c in mc)
            {
                string matchingSql = c.ToString();
                ///在原始的SQL中取出掉这些待解析的SQL
                var parseItem = new ParseItem(c.Value.Replace("<%=", "").Replace("%>", ""), this.KeyValue);
                //returnResult.Add(parseItem);
                _sql = _sql.Replace(matchingSql, parseItem.GetResult());
            }
            return returnResult;
        }
        #endregion
    }
}
