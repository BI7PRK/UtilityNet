using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace UtilityNet.Extension
{
    public static class EnumExtend
    {
        /// <summary>
        /// 获取枚举的 Description
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum enumValue)
        {
            var type = enumValue.GetType();
            var field = type.GetField(enumValue.ToString());
            if (field == null) return string.Empty;
            var descriptions = field.GetCustomAttributes(typeof(DescriptionAttribute), true) as DescriptionAttribute[];

            if (descriptions.Any())
                return descriptions.First().Description;

            return string.Empty;
        }
        /// <summary>
        /// 获取枚举的 DefaultValue
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static object GetDefaultValue(this Enum enumValue)
        {
            var type = enumValue.GetType();
            var field = type.GetField(enumValue.ToString());
            if (field == null) return string.Empty;
            var value = field.GetCustomAttributes(typeof(DefaultValueAttribute), true) as DefaultValueAttribute[];

            if (value.Any())
                return value.First().Value;

            return string.Empty;
        }
        /// <summary>
        /// 获取DefaultValue值转成String类型
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetStringValue(this Enum enumValue)
        {
            return GetDefaultValue(enumValue).ToString();
        }
    }
    /// <summary>
    /// 将枚举遍历
    /// </summary>
    /// <typeparam name="T">枚举</typeparam>
    public class EnumForEach<T>
    {
        /// <summary>
        /// 将枚举遍历成字典
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, T> GetDictionary()
        {
            Dictionary<string, T> dict = new Dictionary<string, T>();
            Type sourceType = typeof(T);
            foreach (string key in Enum.GetNames(sourceType))
            {
                dict.Add(key, (T)Enum.Parse(sourceType, key));
            }
            return dict;
        }
    }
}
