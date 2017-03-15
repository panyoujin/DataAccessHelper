using DataAccessHelper.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace DataAccessHelper.Extensions
{
    public class DataTableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static List<T> TableToList<T>(DataTable dt, string filter = "") where T : new()
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
        /// <summary>
        /// 将指定行转换为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public static T TableToObject<T>(DataTable dt, int rowIndex = 0) where T : new()
        {
            if (rowIndex < 0 || dt == null || dt.Rows.Count < rowIndex)
            {
                return default(T);
            }
            return DataRowToObject<T>(dt.Rows[rowIndex], dt.Columns);
        }
        /// <summary>
        /// 把一列转换为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        private static T DataRowToObject<T>(DataRow dr, DataColumnCollection columns) where T : new()
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
