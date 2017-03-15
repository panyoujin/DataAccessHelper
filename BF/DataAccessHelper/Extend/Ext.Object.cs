using System;
using System.Collections.Generic;
using System.Reflection;

namespace DataAccessHelper.Extend
{
    public static partial class Ext
    {

        private static readonly BindingFlags bf = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic;


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

        /// <summary>
        /// 获取指定属性的值
        /// </summary>
        /// <param name="data">数据</param>
        public static object GetByProperties(this object data, string name)
        {
            Type objType = data.GetType();
            PropertyInfo[] props = objType.GetProperties(bf);
            foreach (PropertyInfo item in props)
            {
                if (item.Name == name)
                {
                    return item.GetValue(data);
                }
            }
            return null;
        }

        /// <summary>
        /// 将对象的指定属性值和传入值进行比较
        /// </summary>
        /// <param name="data"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns>一个 32 位有符号整数，该整数指示此实例在排序顺序中是位于 value 参数之前、之后还是与其出现在同一位置。 值 Condition 小于零 此实例位于 value 之前。 零 此实例在排序顺序中的位置与 value 相同。 大于零 此实例位于 value 之后。 - 或 - value 为 null。</returns>
        public static int PropertiesCompareValue(this object data, string name, string value)
        {
            Type objType = data.GetType();
            PropertyInfo[] props = objType.GetProperties(bf);
            var res = 0;
            foreach (PropertyInfo item in props)
            {
                if (item.Name == name)
                {
                    var temp = item.GetValue(data);
                    switch (item.PropertyType.FullName)
                    {
                        case "System.String":
                            res = value.CompareTo(temp) * -1;
                            break;
                        case "System.Int32":
                            var intTemp = 0;
                            int.TryParse(value, out intTemp);
                            res = intTemp.CompareTo(temp) * -1;
                            break;
                        case "System.DateTime":
                            DateTime dateTemp = DateTime.Now;
                            DateTime.TryParse(value, out dateTemp);
                            res = dateTemp.CompareTo(temp) * -1;
                            break;
                        case "System.Single":
                            float fTemp = 0.0f;
                            float.TryParse(value, out fTemp);
                            res = fTemp.CompareTo(temp) * -1;
                            break;

                        case "System.Double":
                            Double dTemp = 0.0d;
                            Double.TryParse(value, out dTemp);
                            res = dTemp.CompareTo(temp) * -1;
                            break;
                        default:
                            var fName = item.PropertyType.FullName;
                            break;
                    }
                    //return value.CompareTo(temp) * -1;
                }
            }
            return res;
        }
    }
}
