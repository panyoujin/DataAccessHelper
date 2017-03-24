using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using DataAccessHelper.Helper;
using DataAccessHelper.Interface;

namespace DataAccessHelper.SQLHelper
{
    public class MySqlHelper : ISQLHelper
    {
        #region Fields

        public MySqlHelper(string sqlConnStringName)
        {
            SqlConnStringName = sqlConnStringName;
        }

        private string SqlConnStringName { get; set; }


        private string ConnectionString
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings[SqlConnStringName].ConnectionString;
            }
        }
        //private static readonly BindingFlags bf = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic;

        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// 执行sql命令，返回影响行数 (启用事务)
        /// </summary>
        /// <param name="strSqlConn">数据库连接字符串</param>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="dictParams">sql命令的参数数组（可为空）</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                try
                {
                    return conn.Execute(sqlText, dictParams);
                }
                catch (Exception ex)
                {
                    ex.Source = ex.Source + sqlText;
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 执行sql命令，返回影响行数 
        /// </summary>
        /// <param name="strSqlConn">数据库连接字符串</param>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="dictParams">sql命令的参数数组（可为空）</param>
        /// <param name="isUseTrans">是否启用事务</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams, bool isUseTrans)
        {
            if (isUseTrans)
            {
                return ExecuteNonQuery(sqlText, cmdType, dictParams);
            }
            var result = 0;
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                MySqlTransaction trans = conn.BeginTransaction();
                try
                {
                    result = conn.Execute(sqlText, dictParams, conn.BeginTransaction());
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    ex.Source = ex.Source + sqlText;
                    throw ex;
                }
            }
            return result;
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// 执行sql命令，返回第一行第一列（启用事务）
        /// </summary>
        /// <param name="strSqlConn">数据库连接字符串</param>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="sqlParams">sql命令参数 （可为空）</param>
        /// <returns></returns>
        public object ExecuteScalar(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                try
                {
                    return conn.ExecuteScalar(sqlText, dictParams);
                }
                catch (Exception ex)
                {
                    ex.Source = ex.Source + sqlText;
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 执行sql命令，返回第一行第一列（启用事务）
        /// </summary>
        /// <param name="strSqlConn">数据库连接字符串</param>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="sqlParams">sql命令参数 （可为空）</param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                try
                {
                    return conn.ExecuteScalar<T>(sqlText, dictParams);
                }
                catch (Exception ex)
                {
                    ex.Source = ex.Source + sqlText;
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 执行sql命令，返回第一行第一列
        /// </summary>
        /// <param name="strSqlConn">数据库连接字符串</param>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="sqlParams">sql命令参数 （可为空）</param>
        /// <param name="isUseTrans">是否使用事务</param>
        /// <returns></returns>
        public object ExecuteScalar(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams, bool isUseTrans)
        {
            if (isUseTrans)
            {
                return ExecuteScalar(sqlText, cmdType, dictParams);
            }
            object result = null;
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                MySqlTransaction trans = conn.BeginTransaction();
                try
                {
                    result = conn.ExecuteScalar(sqlText, dictParams, conn.BeginTransaction());
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    ex.Source = ex.Source + sqlText;
                    throw ex;
                }
            }
            return result;
        }
        /// <summary>
        /// 执行sql命令，返回第一行第一列
        /// </summary>
        /// <param name="strSqlConn">数据库连接字符串</param>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="sqlParams">sql命令参数 （可为空）</param>
        /// <param name="isUseTrans">是否使用事务</param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams, bool isUseTrans)
        {
            if (isUseTrans)
            {
                return ExecuteScalar<T>(sqlText, cmdType, dictParams);
            }
            T t = default(T);
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                MySqlTransaction trans = conn.BeginTransaction();
                try
                {
                    t = conn.ExecuteScalar<T>(sqlText, dictParams, conn.BeginTransaction());
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    ex.Source = ex.Source + sqlText;
                    throw ex;
                }
            }
            return t;
        }
        #endregion

        #region ExecuteReader


        public IDataReader ExecuteReader(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams)
        {

            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                try
                {
                    return conn.ExecuteReader(sqlText, dictParams);
                }
                catch (Exception ex)
                {
                    ex.Source = ex.Source + sqlText;
                    throw ex;
                }
            }
        }

        #endregion ExecuteReader

        #region Query


        public IEnumerable<dynamic> QueryForList(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                try
                {
                    return conn.Query(sqlText, dictParams);
                }
                catch (Exception ex)
                {
                    ex.Source = ex.Source + sqlText;
                    throw ex;
                }
            }
        }
        public IEnumerable<dynamic> QueryForList(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams, bool isUseTrans)
        {
            if (!isUseTrans)
            {
                return QueryForList(sqlText, cmdType, dictParams);
            }
            IEnumerable<dynamic> result = default(IEnumerable<dynamic>);
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                MySqlTransaction trans = conn.BeginTransaction();
                try
                {
                    result = conn.Query(sqlText, dictParams);
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    ex.Source = ex.Source + sqlText;
                    throw ex;
                }
            }
            return result;
        }

        public IEnumerable<T> QueryForList<T>(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                try
                {
                    return conn.Query<T>(sqlText, dictParams);
                }
                catch (Exception ex)
                {
                    ex.Source = ex.Source + sqlText;
                    throw ex;
                }
            }
        }
        public IEnumerable<T> QueryForList<T>(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams, bool isUseTrans)
        {
            if (!isUseTrans)
            {
                return QueryForList<T>(sqlText, cmdType, dictParams);
            }

            IEnumerable<T> result = default(IEnumerable<T>);
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                MySqlTransaction trans = conn.BeginTransaction();
                try
                {
                    result = conn.Query<T>(sqlText, dictParams);
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    ex.Source = ex.Source + sqlText;
                    throw ex;
                }
            }
            return result;
        }

        public dynamic QueryForObject(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                try
                {
                    return conn.QueryFirst(sqlText, dictParams);
                }
                catch (Exception ex)
                {
                    ex.Source = ex.Source + sqlText;
                    throw ex;
                }
            }
        }
        public dynamic QueryForObject(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams, bool isUseTrans)
        {
            if (!isUseTrans)
            {
                return QueryForObject(sqlText, cmdType, dictParams);
            }

            dynamic result = default(dynamic);
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                MySqlTransaction trans = conn.BeginTransaction();
                try
                {
                    result = conn.QueryFirst(sqlText, dictParams);
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    ex.Source = ex.Source + sqlText;
                    throw ex;
                }
            }
            return result;
        }

        public T QueryForObject<T>(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                try
                {
                    return conn.QueryFirst<T>(sqlText, dictParams);
                }
                catch (Exception ex)
                {
                    ex.Source = ex.Source + sqlText;
                    throw ex;
                }
            }
        }

        public T QueryForObject<T>(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams, bool isUseTrans)
        {
            if (!isUseTrans)
            {
                return QueryForObject<T>(sqlText, cmdType, dictParams);
            }

            T result = default(T);
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                MySqlTransaction trans = conn.BeginTransaction();
                try
                {
                    result = conn.QueryFirst<T>(sqlText, dictParams);
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    ex.Source = ex.Source + sqlText;
                    throw ex;
                }
            }
            return result;
        }
        #endregion Query







    }
}
