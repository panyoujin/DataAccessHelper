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

        public int ExecuteNonQuery(string sqlKey, Dictionary<string, object> paramDic, bool isUseTrans = false)
        {
            var sqlAnaly = CacheSqlConfig.Instance.GetSqlAnalyByKey(sqlKey, paramDic);
            return GetSQLHelper(sqlAnaly).ExecuteNonQuery(sqlAnaly.SqlText, CommandType.Text, paramDic, isUseTrans);
        }

        public IDataReader ExecuteReader(string sqlKey, Dictionary<string, object> paramDic, bool isUseTrans = false)
        {
            var sqlAnaly = CacheSqlConfig.Instance.GetSqlAnalyByKey(sqlKey, paramDic);
            return GetSQLHelper(sqlAnaly).ExecuteReader(sqlAnaly.SqlText, CommandType.Text, paramDic);
        }

        public object ExecuteScalar(string sqlKey, Dictionary<string, object> paramDic, bool isUseTrans = false)
        {
            var sqlAnaly = CacheSqlConfig.Instance.GetSqlAnalyByKey(sqlKey, paramDic);
            return GetSQLHelper(sqlAnaly).ExecuteScalar(sqlAnaly.SqlText, CommandType.Text, paramDic, isUseTrans);
        }

        public T ExecuteScalarByT<T>(string sqlKey, Dictionary<string, object> paramDic, bool isUseTrans = false)
        {
            var sqlAnaly = CacheSqlConfig.Instance.GetSqlAnalyByKey(sqlKey, paramDic);
            return GetSQLHelper(sqlAnaly).ExecuteScalar<T>(sqlAnaly.SqlText, CommandType.Text, paramDic, isUseTrans);
        }


        public List<dynamic> QueryForList(string sqlKey, Dictionary<string, object> paramDic, bool isUseTrans = false)
        {
            var sqlAnaly = CacheSqlConfig.Instance.GetSqlAnalyByKey(sqlKey, paramDic);
            var list = GetSQLHelper(sqlAnaly).QueryForList(sqlAnaly.SqlText, CommandType.Text, paramDic, isUseTrans);
            if (list == null)
            {
                return null;
            }
            return list.ToList();
        }

        public List<T> QueryForListByT<T>(string sqlKey, Dictionary<string, object> paramDic, bool isUseTrans = false)
        {
            var sqlAnaly = CacheSqlConfig.Instance.GetSqlAnalyByKey(sqlKey, paramDic);
            var list = GetSQLHelper(sqlAnaly).QueryForList<T>(sqlAnaly.SqlText, CommandType.Text, paramDic, isUseTrans);
            if (list == null)
            {
                return null;
            }
            return list.ToList();
        }

        public dynamic QueryForObject(string sqlKey, Dictionary<string, object> paramDic, bool isUseTrans = false)
        {
            var sqlAnaly = CacheSqlConfig.Instance.GetSqlAnalyByKey(sqlKey, paramDic);
            var t = GetSQLHelper(sqlAnaly).QueryForObject(sqlAnaly.SqlText, CommandType.Text, paramDic, isUseTrans);
            return t;
        }

        public T QueryForObjectByT<T>(string sqlKey, Dictionary<string, object> paramDic, bool isUseTrans = false)
        {
            var sqlAnaly = CacheSqlConfig.Instance.GetSqlAnalyByKey(sqlKey, paramDic);
            var t = GetSQLHelper(sqlAnaly).QueryForObject<T>(sqlAnaly.SqlText, CommandType.Text, paramDic, isUseTrans);
            return t;
        }
        /// <summary>
        /// 返回结果集和数量 专为分页功能而准备  数据集的sql在前面，返回数量的在后面
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlKey"></param>
        /// <param name="paramDic"></param>
        /// <param name="total"></param>
        /// <param name="isUseTrans"></param>
        /// <returns></returns>
        public IEnumerable<T> QueryMultipleByPage<T>(string sqlKey, Dictionary<string, object> paramDic, out int total, bool isUseTrans = false)
        {

            var sqlAnaly = CacheSqlConfig.Instance.GetSqlAnalyByKey(sqlKey, paramDic);
            var t = GetSQLHelper(sqlAnaly).QueryMultipleByPage<T>(sqlAnaly.SqlText, CommandType.Text, paramDic, out total, isUseTrans);
            return t;
        }

        /// <summary>
        /// 返回多个结果集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlKey"></param>
        /// <param name="paramDic"></param>
        /// <param name="total"></param>
        /// <param name="isUseTrans"></param>
        /// <returns></returns>
        public IEnumerable<TReturn> QueryMultiple<TFirst, TSecond, TReturn>(string sqlKey, Dictionary<string, object> paramDic, Func<IEnumerable<TFirst>, IEnumerable<TSecond>, IEnumerable<TReturn>> func, bool isUseTrans = false)
        {
            var sqlAnaly = CacheSqlConfig.Instance.GetSqlAnalyByKey(sqlKey, paramDic);
            var t = GetSQLHelper(sqlAnaly).QueryMultiple<TFirst, TSecond, TReturn>(sqlAnaly.SqlText, CommandType.Text, paramDic, func, isUseTrans);
            return t;
        }
    }
}
