using FastReflectionLib;
using System;
using System.ComponentModel;
using System.Reflection;

namespace BF.DataAccessHelper.Helper
{
    public class ObjectSetValue
    {
        /// <summary>
        /// 实体快速反射赋值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldName">字段名（不区分大小写）</param>
        /// <param name="model">要赋值的实体对象</param>
        /// <param name="objValue">某一属性的值</param>
        /// <param name="item">反射后的值</param>
        public static void SetValue<T>(string fieldName, T model, object objValue, PropertyInfo item)
        {
            if (objValue == null)
            {
                return;
            }
            string propName = item.Name;
            if (string.Compare(propName, fieldName, true) != 0)
            {
                return;
            }
            try
            {
                var innerType = item.PropertyType;
                //可空类型特殊处理
                if (innerType.IsGenericType
                    && innerType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    if (objValue != null)
                    {
                        var converter = new NullableConverter(innerType);
                        var mapType = converter.UnderlyingType;

                        var realValue = Convert.ChangeType(objValue, mapType);
                        item.FastSetValue(model, realValue);
                    }

                }
                else
                {
                    item.FastSetValue(model, objValue);
                }

            }
            catch
            {
                object realValue = null;
                try
                {
                    realValue = Convert.ChangeType(objValue, item.PropertyType);
                    item.FastSetValue(model, realValue);
                }
                catch
                {
                }
            }
        }
    }
}
