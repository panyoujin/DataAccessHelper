using System;
using System.Collections.Generic;
using System.Data;

namespace DataAccessHelper.Interface
{
    public interface ISQLHelper
    {
        /// <summary>
        /// 执行sql命令，返回影响行数
        /// </summary>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="dictParams">参数</param>
        /// <returns></returns>
        //int ExecuteNonQuery(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams);

        /// <summary>
        /// 执行sql命令，返回影响行数
        /// </summary>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="dictParams">参数</param>
        /// <param name="isUseTrans">是否使用事务</param>
        /// <returns></returns>
        int ExecuteNonQuery(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams, bool isUseTrans);

        /// <summary>
        /// 执行sql命令，返回第一行第一列
        /// </summary>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="dictParams">参数</param>
        /// <returns></returns>
        //object ExecuteScalar(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams);

        /// <summary>
        /// 执行sql命令，返回第一行第一列
        /// </summary>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="dictParams">参数</param>
        /// <returns></returns>
        //T ExecuteScalar<T>(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams);

        /// <summary>
        /// 执行sql命令，返回第一行第一列
        /// </summary>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="dictParams">参数</param>
        /// <param name="isUseTrans">是否使用事务</param>
        /// <returns></returns>
        object ExecuteScalar(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams, bool isUseTrans);

        /// <summary>
        /// 执行sql命令，返回第一行第一列
        /// </summary>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="dictParams">参数</param>
        /// <param name="isUseTrans">是否使用事务</param>
        /// <returns></returns>
        T ExecuteScalar<T>(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams, bool isUseTrans);

        /// <summary>
        /// <summary>
        /// 执行sql命令，返回一条数据
        /// </summary>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="dictParams">参数</param>
        /// <returns></returns>
        IDataReader ExecuteReader(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams);


        /// <summary>
        /// 执行sql命令，返回IEnumerable<dynamic>
        /// </summary>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="dictParams">参数</param>
        /// <returns></returns>
        IEnumerable<dynamic> QueryForList(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams, bool isUseTrans);
        /// <summary>
        /// 执行sql命令，返回IEnumerable<T>
        /// </summary>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="dictParams">参数</param>
        /// <returns></returns>
        IEnumerable<T> QueryForList<T>(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams, bool isUseTrans);

        /// <summary>
        /// 执行sql命令，返回dynamic
        /// </summary>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="dictParams">参数</param>
        /// <returns></returns>
        dynamic QueryForObject(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams, bool isUseTrans);

        /// <summary>
        /// 执行sql命令，返回T
        /// </summary>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="dictParams">参数</param>
        /// <returns></returns>
        T QueryForObject<T>(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams, bool isUseTrans);

        /// <summary>
        /// 返回结果集和数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlText"></param>
        /// <param name="cmdType"></param>
        /// <param name="dictParams"></param>
        /// <param name="total"></param>
        /// <param name="isUseTrans"></param>
        /// <returns></returns>
        IEnumerable<T> QueryMultiple<T>(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams, out int total, bool isUseTrans);

        /// <summary>
        /// 返回多个结果集，通过委托
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sqlText"></param>
        /// <param name="cmdType"></param>
        /// <param name="dictParams"></param>
        /// <param name="func"></param>
        /// <param name="isUseTrans"></param>
        /// <returns></returns>
        IEnumerable<TReturn> QueryMultiple<TFirst, TSecond, TReturn>(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams, Func<IEnumerable<TFirst>, IEnumerable<TSecond>, IEnumerable<TReturn>> func, bool isUseTrans);
        
    }
}
