using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessHelper.Extensions
{
    public static class ObjectExtend
    {
        /// <summary>
        /// 把对象的属性转换为Dictionary<string, object>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDictionary(this object obj)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            var t = obj.GetType();
            foreach (var item in t.GetProperties())
            {
                dic.Add(item.Name, item.GetValue(obj));
            }
            return dic;
        }
    }
}
