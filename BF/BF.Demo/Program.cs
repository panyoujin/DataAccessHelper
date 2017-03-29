using BF.Demo.Log;
using DataAccessHelper.Extend;
using DataAccessHelper.SQLHelper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BF.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, object> dic = new { LoginTime = DateTime.Now, StartDate = DateTime.Now.AddYears(-2), EndDate = DateTime.Now, UserId = "1c120d87-12d4-44cb-9f20-5c3f318e3393" }.ToDictionary();
            try
            {
                LogFactory.GetLogger().Debug(string.Format("InsertLoginLog 开始 {0}", dic.ToJson()));
                var r = SQLHelperFactory.Instance.ExecuteNonQuery("InsertLoginLog", dic, true);
                LogFactory.GetLogger().Debug(string.Format("InsertLoginLog 结束 {0}", r));
            }
            catch (Exception ex)
            {
                LogFactory.GetLogger().Error(string.Format("InsertLoginLog 异常 {0}", ex));
            }
            try
            {
                LogFactory.GetLogger().Debug(string.Format("GetData 开始 {0}", dic.ToJson()));
                int total = 0;
                var o = SQLHelperFactory.Instance.QueryMultiple<object, int, object>("GetData", dic, (l, t) =>
                {
                    total = t.FirstOrDefault();
                    return l;
                });
                LogFactory.GetLogger().Debug(string.Format("GetData 结束 {0} 数量：{1}", o.FirstOrDefault(), total));
            }
            catch (Exception ex)
            {
                LogFactory.GetLogger().Error(string.Format("GetData 异常 {0}", ex));
            }
            Console.Read();
        }
    }

}
