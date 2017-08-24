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
    public static class ObjectExtensions
    {
        /// <summary>
        /// 转换值到指定的类型
        /// </summary>
        /// <param name="value">要转换的值</param>
        /// <param name="targetType">目标类型</param>
        /// <returns></returns>
        public static object ParseTypeValue(this object value, Type targetType)
        {
            try
            {
                if (value != null && targetType == value.GetType()) return value;
                if (targetType.IsEnum)
                {
                    if (value is string)
                        return Enum.Parse(targetType, value as string);
                    else
                        return Enum.ToObject(targetType, value);
                }
                if (value != null && targetType == typeof(Guid))
                {
                    Guid guid;
                    if (!Guid.TryParse(value.ToString(), out guid))
                    {
                        guid = Guid.Empty;
                    }
                    return guid;
                }
                if (value == null && targetType.IsValueType) return Activator.CreateInstance(targetType);

                if (value is IConvertible && value != null) return Convert.ChangeType(value, targetType);

                return value;
            }
            catch (Exception Ex)
            {
                return value;
            }
        }
    }
}
