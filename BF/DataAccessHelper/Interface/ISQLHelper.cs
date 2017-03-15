using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        int ExecuteNonQuery(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams);

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
        object ExecuteScalar(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams);

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
        /// <summary>
        /// 执行sql命令，返回一条数据
        /// </summary>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="dictParams">参数</param>
        /// <returns></returns>
        IDataReader ExecuteReader(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams);

        /// <summary>
        /// 执行sql命令，返回DataSet
        /// </summary>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="dictParams">参数</param>
        /// <returns></returns>
        DataSet QueryForDataSet(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams);

        /// <summary>
        /// 执行sql命令，返回DataTable
        /// </summary>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="dictParams">参数</param>
        /// <returns></returns>
        DataTable QueryForDataTable(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams);
    }
}
