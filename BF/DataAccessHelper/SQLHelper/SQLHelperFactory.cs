using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DataAccessHelper.Interface;
using DataAccessHelper.Models;
using DataAccessHelper.SQLAnalytical;

namespace DataAccessHelper.SQLHelper
{
    public class SQLHelperFactory
    {
        #region Instance

        private static readonly Lazy<SQLHelperFactory> _instance = new Lazy<SQLHelperFactory>(() => new SQLHelperFactory());
        private SQLHelperFactory()
        {

        }

        public static SQLHelperFactory Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        #endregion

        private ISQLHelper GetSQLHelper(SqlAnalyModel model)
        {
            ISQLHelper sqlHelper = null;
            switch (model.DBType.ToLower())
            {
                case "mysql":
                    sqlHelper = new MySqlHelper(model.SqlConnStringName);
                    break;
                case "sqlserver":
                    sqlHelper = new SqlServerHelper(model.SqlConnStringName);
                    break;
                default:
                    throw new Exception("暂不支持" + model.DBType + "数据库");
            }
            return sqlHelper;
        }

        public int ExecuteNonQuery(string sqlKey, Dictionary<string, object> paramDic, bool isCache = false)
        {
            var sqlAnaly = CacheSqlConfig.Instance.GetSqlAnalyByKey(sqlKey, paramDic);
            return GetSQLHelper(sqlAnaly).ExecuteNonQuery(sqlAnaly.SqlText, CommandType.Text, paramDic, false);
        }

        public int ExecuteNonQuery(string sqlKey, Dictionary<string, object> paramDic, bool isUseTrans, bool isCache = false)
        {
            var sqlAnaly = CacheSqlConfig.Instance.GetSqlAnalyByKey(sqlKey, paramDic);
            return GetSQLHelper(sqlAnaly).ExecuteNonQuery(sqlAnaly.SqlText, CommandType.Text, paramDic, isUseTrans);
        }

        public IDataReader ExecuteReader(string sqlKey, Dictionary<string, object> paramDic, bool isUseTrans = false, bool isCache = false)
        {
            var sqlAnaly = CacheSqlConfig.Instance.GetSqlAnalyByKey(sqlKey, paramDic);
            return GetSQLHelper(sqlAnaly).ExecuteReader(sqlAnaly.SqlText, CommandType.Text, paramDic);
        }

        public object ExecuteScalar(string sqlKey, Dictionary<string, object> paramDic, bool isUseTrans = false, bool isCache = false)
        {
            var sqlAnaly = CacheSqlConfig.Instance.GetSqlAnalyByKey(sqlKey, paramDic);
            return GetSQLHelper(sqlAnaly).ExecuteScalar(sqlAnaly.SqlText, CommandType.Text, paramDic, isUseTrans);
        }

        public T ExecuteScalarByT<T>(string sqlKey, Dictionary<string, object> paramDic, bool isUseTrans = false, bool isCache = false)
        {
            var sqlAnaly = CacheSqlConfig.Instance.GetSqlAnalyByKey(sqlKey, paramDic);
            return GetSQLHelper(sqlAnaly).ExecuteScalar<T>(sqlAnaly.SqlText, CommandType.Text, paramDic, isUseTrans);
        }


        public List<dynamic> QueryForList(string sqlKey, Dictionary<string, object> paramDic, bool isUseTrans = false, bool isCache = false)
        {
            var sqlAnaly = CacheSqlConfig.Instance.GetSqlAnalyByKey(sqlKey, paramDic);
            var list = GetSQLHelper(sqlAnaly).QueryForList(sqlAnaly.SqlText, CommandType.Text, paramDic, isUseTrans);
            if (list == null)
            {
                return null;
            }
            return list.ToList();
        }

        public List<T> QueryForListByT<T>(string sqlKey, Dictionary<string, object> paramDic, bool isUseTrans = false, bool isCache = false)
        {
            var sqlAnaly = CacheSqlConfig.Instance.GetSqlAnalyByKey(sqlKey, paramDic);
            var list = GetSQLHelper(sqlAnaly).QueryForList<T>(sqlAnaly.SqlText, CommandType.Text, paramDic, isUseTrans);
            if (list == null)
            {
                return null;
            }
            return list.ToList();
        }

        public T QueryForObjectByT<T>(string sqlKey, Dictionary<string, object> paramDic, bool isUseTrans = false, bool isCache = false)
        {
            var sqlAnaly = CacheSqlConfig.Instance.GetSqlAnalyByKey(sqlKey, paramDic);
            var t = GetSQLHelper(sqlAnaly).QueryForObject<T>(sqlAnaly.SqlText, CommandType.Text, paramDic, isUseTrans);
            return t;
        }

    }
}
