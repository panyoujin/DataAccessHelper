using System.Collections.Generic;
using System.Data;

namespace BF.DataAccessHelper.Interface
{
    public interface IDBBase
    {
        #region ExecuteNonQuery

        /// <summary>
        /// 执行sql命令，返回影响行数 (启用事务)
        /// </summary>
        /// <param name="key">SQL配置关键字</param>
        /// <param name="paramDic">参数集合</param>
        /// <returns></returns>
        int ExecuteNonQuery(string sqlKey, Dictionary<string, object> paramDic, string sqlConnStringName = "",bool isCache=false);

        /// <summary>
        /// 执行sql命令，返回影响行数
        /// </summary>
        /// <param name="key">SQL配置关键字</param>
        /// <param name="paramDic">参数集合</param>
        /// <param name="isUseTrans">是否启用事务</param>
        /// <returns></returns>
        int ExecuteNonQuery(string sqlKey, Dictionary<string, object> paramDic, bool isUseTrans, string sqlConnStringName = "",bool isCache=false);

        #endregion ExecuteNonQuery

        #region ExecuteNonQuery

        /// <summary>
        /// 执行sql命令，返回影响行数 (启用事务)
        /// </summary>
        /// <param name="key">SQL配置关键字</param>
        /// <param name="paramDic">参数集合</param>
        /// <returns></returns>
        int BatchInsert<T>(string tbName, string[] columeArr, IList<T> listModels, string sqlConnStringName = "",bool isCache=false) where T : class, new();

        #endregion ExecuteNonQuery


        #region ExecuteScalar

        /// <summary>
        /// 执行sql命令，返回第一行第一列（启用事务）
        /// </summary>
        /// <param name="key">SQL配置关键字</param>
        /// <param name="paramDic">参数集合</param>
        /// <returns></returns>
        object ExecuteScalar(string sqlKey, Dictionary<string, object> paramDic, string sqlConnStringName = "",bool isCache=false);

        /// <summary>
        /// 执行sql命令，返回第一行第一列
        /// </summary>
        /// <param name="key">SQL配置关键字</param>
        /// <param name="paramDic">参数集合</param>
        /// <param name="isUseTrans">是否启用事务</param>
        /// <returns></returns>
        object ExecuteScalar(string sqlKey, Dictionary<string, object> paramDic, bool isUseTrans, string sqlConnStringName = "",bool isCache=false);

        #endregion ExecuteScalar


        #region Collections

        /// <summary>
        /// 执行sql命令，返回IDataReader
        /// </summary>
        /// <param name="key">SQL配置关键字</param>
        /// <param name="paramDic">参数集合</param>
        /// <returns></returns>
        IDataReader ExecuteReader(string sqlKey, Dictionary<string, object> paramDic, string sqlConnStringName = "",bool isCache=false);


        /// <summary>
        /// 执行sql命令，返回DataSet
        /// </summary>
        /// <param name="key">SQL配置关键字</param>
        /// <param name="paramDic">参数集合</param>
        /// <returns></returns>
        DataSet QueryForDataSet(string sqlKey, Dictionary<string, object> paramDic, string sqlConnStringName = "",bool isCache=false);


        /// <summary>
        /// 执行sql命令，返回DataTable
        /// </summary>
        /// <param name="key">SQL配置关键字</param>
        /// <param name="paramDic">参数集合</param>
        /// <returns></returns>
        DataTable QueryForDataTable(string sqlKey, Dictionary<string, object> paramDic, string sqlConnStringName = "",bool isCache=false);


        /// <summary>
        /// 执行sql命令，返回对象列表
        /// </summary>
        /// <param name="key">SQL配置关键字</param>
        /// <param name="paramDic">参数集合</param>
        /// <returns></returns>
        List<T> QueryForList<T>(string sqlKey, Dictionary<string, object> paramDic, string sqlConnStringName = "",bool isCache=false) where T : new();



        /// <summary>
        /// 通过查询 返回第一行的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="paramDic"></param>
        /// <returns></returns>
        T QueryForObject<T>(string sqlKey, Dictionary<string, object> paramDic, string sqlConnStringName = "",bool isCache=false) where T : new();

        List<T> TableToList<T>(DataTable dt, string filter = "") where T : new();

        /// <summary>
        /// Table第一行转换为指定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        T TableToObject<T>(DataTable dt) where T : new();
        #endregion Collections
    }
}
