using DataAccessHelper.Helper;
using DataAccessHelper.Models;
using DataAccessHelper.Utilities;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;

namespace DataAccessHelper.SQLAnalytical
{
    /// <summary>
    /// SQL 节点定义
    /// </summary>
    [Serializable]
    public class SqlDefinition
    {
        #region ctor..
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="node"></param>
        public SqlDefinition(XmlNode node)
        {
            this.SqlCommand = XmlUtility.getNodeStringValue(node["SqlCommand"]);

            this.SqlDBType = XmlUtility.getNodeAttributeStringValue(node, "type");
            this.SqlConnStringName = XmlUtility.getNodeAttributeStringValue(node, "ConnStringName", ConfigHelper.GetConfigValue("ConnStringName", "DbContext"));
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
        private string SqlCommand
        {
            get
            {
                string outSql = _sql;
                if (this.ParseItems == null || this.ParseItems.Count <= 0) return outSql;
                foreach (ParseItem item in this.ParseItems)
                {
                    outSql = outSql + item.GetResult(this.SqlDBType);
                }
                return outSql;
            }
            set { _sql = value; }
        }

        private string SqlConnStringName { get; set; }
        private string SqlDBType { get; set; }
        public SqlAnalyModel SqlAnaly(Dictionary<string, object> keyValue)
        {
            SqlAnalyModel model = new SqlAnalyModel();
            GetAllParseItem(_sql, keyValue);
            model.SqlText = SqlCommand;
            model.SqlConnStringName = SqlConnStringName;
            model.DBType = SqlDBType;
            return model;
        }
        
        #endregion

        #region method define
        

        /// <summary>
        /// 解析出SQL语句中需要待解析的内容
        /// </summary>
        /// <param name="sqlText"></param>
        /// <param name="isParam">是否参数化  flase:否 true:是</param>
        /// <returns></returns>
        private List<ParseItem> GetAllParseItem(string sqlText, Dictionary<string, object> KeyValue)
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
                var parseItem = new ParseItem(c.Value.Replace("<%=", "").Replace("%>", ""), KeyValue);
                //returnResult.Add(parseItem);
                _sql = _sql.Replace(matchingSql, parseItem.GetResult(this.SqlDBType));
            }
            regKeyword = new Regex("<R%=.*?%R>");
            mc = regKeyword.Matches(_sql);
            foreach (Match c in mc)
            {
                string matchingSql = c.ToString();
                ///在原始的SQL中取出掉这些待解析的SQL
                var parseItem = new ParseItem(c.Value.Replace("<R%=", "").Replace("%R>", ""), KeyValue);
                //returnResult.Add(parseItem);
                _sql = _sql.Replace(matchingSql, parseItem.GetResult(this.SqlDBType, false));
            }

            regKeyword = new Regex("@@.*?@@");
            mc = regKeyword.Matches(_sql);
            foreach (Match c in mc)
            {
                string matchingSql = c.ToString();
                ///在原始的SQL中取出掉这些待解析的SQL
                var parseItem = new ParseItem(c.Value.Replace("<%=", "").Replace("%>", ""), KeyValue);
                //returnResult.Add(parseItem);
                _sql = _sql.Replace(matchingSql, parseItem.GetResult(this.SqlDBType));
            }
            return returnResult;
        }
        #endregion
    }
}
