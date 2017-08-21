using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;


namespace FengYun.NPoco.Data
{
    /// <summary>
    /// 扩展
    /// author:zhaixd
    /// datetime:2016.05.22
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Object数据转换为DataTable
        /// </summary>
        /// <param name="items">要转换的字典集合</param>
        /// <returns></returns>
        public static DataTable ToDataTable(this IEnumerable<IDictionary<string, object>> items)
        {
            var table = new DataTable();
            var p = items.FirstOrDefault();
            if (p == null)
                return table;
            //数据列类型转换
            Func<KeyValuePair<string, object>, DataColumn> _funConvertDataColumn = (x) =>
            {
                DataColumn col = new DataColumn(x.Key);
                if (x.Value is Guid)
                    col.DataType = typeof(Guid);
                if (x.Value is DateTime)
                    col.DataType = typeof(DateTime);
                else if (x.Value is Double)
                    col.DataType = typeof(Double);
                else if (x.Value is Decimal)
                    col.DataType = typeof(Decimal);
                else if (x.Value is Boolean)
                    col.DataType = typeof(Boolean);
                else if (x.Value is Int32)
                    col.DataType = typeof(Int32);
                else if (x.Value is Int64)
                    col.DataType = typeof(Int64);
                else if (x.Value is String)
                    col.DataType = typeof(String);
                else
                    col.DataType = typeof(Object);
                return col;
            };

            var headers = p.Select(x => _funConvertDataColumn(x)).ToArray();
            table.Columns.AddRange(headers);
            foreach (var parent in items)
            {
                table.Rows.Add(parent.Values.ToArray());
            }
            return table;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="source"></param>
        ///// <param name="times"></param>
        ///// <returns></returns>
        //public static IEnumerable<T> Repeat<T>(this IEnumerable<T> source, int times)
        //{
        //    source = source.ToArray();
        //    return Enumerable.Range(0, times).SelectMany(_ => source);
        //}
    }
}
