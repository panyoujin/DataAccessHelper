using DataAccessHelper.Helper;
using DataAccessHelper.Interface;
using DataAccessHelper.Models;
using DataAccessHelper.SQLAnalytical;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

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

        public IDataReader ExecuteReader(string sqlKey, Dictionary<string, object> paramDic, bool isCache = false)
        {
            var sqlAnaly = CacheSqlConfig.Instance.GetSqlAnalyByKey(sqlKey, paramDic);
            return GetSQLHelper(sqlAnaly).ExecuteReader(sqlAnaly.SqlText, CommandType.Text, paramDic);
        }

        public object ExecuteScalar(string sqlKey, Dictionary<string, object> paramDic, bool isCache = false)
        {
            var sqlAnaly = CacheSqlConfig.Instance.GetSqlAnalyByKey(sqlKey, paramDic);
            return GetSQLHelper(sqlAnaly).ExecuteScalar(sqlAnaly.SqlText, CommandType.Text, paramDic, false);
        }

        public object ExecuteScalar(string sqlKey, Dictionary<string, object> paramDic, bool isUseTrans, bool isCache = false)
        {
            var sqlAnaly = CacheSqlConfig.Instance.GetSqlAnalyByKey(sqlKey, paramDic);
            return GetSQLHelper(sqlAnaly).ExecuteScalar(sqlAnaly.SqlText, CommandType.Text, paramDic, isUseTrans);
        }

        public DataSet QueryForDataSet(string sqlKey, Dictionary<string, object> paramDic, bool isCache = false)
        {
            var sqlAnaly = CacheSqlConfig.Instance.GetSqlAnalyByKey(sqlKey, paramDic);
            return GetSQLHelper(sqlAnaly).QueryForDataSet(sqlAnaly.SqlText, CommandType.Text, paramDic);
        }

        public DataTable QueryForDataTable(string sqlKey, Dictionary<string, object> paramDic, bool isCache = false)
        {
            var sqlAnaly = CacheSqlConfig.Instance.GetSqlAnalyByKey(sqlKey, paramDic);
            return GetSQLHelper(sqlAnaly).QueryForDataTable(sqlAnaly.SqlText, CommandType.Text, paramDic);
        }

        public List<T> QueryForList<T>(string sqlKey, Dictionary<string, object> paramDic, bool isCache = false) where T : new()
        {
            var sqlAnaly = CacheSqlConfig.Instance.GetSqlAnalyByKey(sqlKey, paramDic);
            var dt = GetSQLHelper(sqlAnaly).QueryForDataTable(sqlAnaly.SqlText, CommandType.Text, paramDic);
            return TableToList<T>(dt);
        }

        public T QueryForObject<T>(string sqlKey, Dictionary<string, object> paramDic, bool isCache = false) where T : new()
        {
            var sqlAnaly = CacheSqlConfig.Instance.GetSqlAnalyByKey(sqlKey, paramDic);
            var dt = GetSQLHelper(sqlAnaly).QueryForDataTable(sqlAnaly.SqlText, CommandType.Text, paramDic);
            return TableToObject<T>(dt);
        }

        public List<T> TableToList<T>(DataTable dt, string filter = "") where T : new()
        {
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }

            DataRow[] drs = dt.Select(filter);
            DataColumnCollection columns = dt.Columns;

            List<T> tList = new List<T>();
            foreach (DataRow dr in drs)
            {
                T t = DataRowToObject<T>(dr, columns);
                tList.Add(t);
            }
            return tList;
        }

        public T TableToObject<T>(DataTable dt) where T : new()
        {
            if (dt == null || dt.Rows.Count <= 0)
            {
                return default(T);
            }
            return DataRowToObject<T>(dt.Rows[0], dt.Columns);
        }

        private T DataRowToObject<T>(DataRow dr, DataColumnCollection columns) where T : new()
        {
            T t = new T();
            //将反射放在这个位置，防止重复反射
            Type objType = typeof(T);
            var itemArray = objType.GetProperties();
            foreach (DataColumn c in columns)
            {
                var item = itemArray.Where(a => string.Compare(a.Name, c.ColumnName, true) == 0).FirstOrDefault();
                if (item == null) continue;
                try
                {
                    if (item.PropertyType.IsGenericType && item.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    {
                        NullableConverter nullableConverter = new NullableConverter(item.PropertyType);
                        var convertType = nullableConverter.UnderlyingType;
                        var obj = Convert.ChangeType(dr[c.ColumnName], convertType);
                        ObjectSetValue.SetValue<T>(c.ColumnName, t, obj, item);

                    }
                    else
                    {
                        //防止多次的装箱拆箱操作
                        var obj = Convert.ChangeType(dr[c.ColumnName], item.PropertyType);
                        ObjectSetValue.SetValue<T>(c.ColumnName, t, obj, item);
                    }
                }
                catch
                {
                    //写日记
                }
            }

            return t;
        }


    }
}
