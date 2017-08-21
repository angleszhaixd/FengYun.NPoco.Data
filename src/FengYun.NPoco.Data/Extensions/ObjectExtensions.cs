using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;


namespace FengYun.NPoco.Data
{
    /// <summary>
    /// 扩展
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Object数据转换为DataTable
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(this IEnumerable<object[]> items)
        {
            DataTable dt = new DataTable();
            foreach (var item in items)
            {
                dt.Rows.Add(item);
            }
            return dt;
        }
    }
}
