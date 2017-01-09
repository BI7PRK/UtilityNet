using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace UtilityNet.Extension
{
    public class EnumDisplayAttribute : Attribute
    {
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="display">显示的文字</param>
        public EnumDisplayAttribute(string display)
        {
            Display = display;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="display">显示的文字</param>
        /// <param name="remark">说明</param>
        public EnumDisplayAttribute(string display, string remark)
        {
            Display = display;
            Remark = remark;
        }

        /// <summary>
        /// 显示的文字
        /// </summary>
        public string Display
        {
            get;
            private set;
        }
        /// <summary>
        /// 枚举值说明
        /// </summary>
        public string Remark
        {
            get;
            private set;
        }

    }
    /// <summary>
    /// 枚举扩展
    /// </summary>
    public static class EnumDisplayExtension
    {
        private static EnumDisplayAttribute GetAttribute(Enum _enum)
        {
            Type type = _enum.GetType();
            FieldInfo field = type.GetField(_enum.ToString());
            if (field == null)
                return null;
            return (EnumDisplayAttribute)
                (field.GetCustomAttributes(typeof(EnumDisplayAttribute), false)
                .FirstOrDefault());
        }

        public static string GetDisplay(this Enum _enum)
        {
            var attr = GetAttribute(_enum);
            return attr == null ? "" : attr.Display;
        }

        public static string GetRemark(this Enum _enum)
        {
            var attr = GetAttribute(_enum);
            return attr == null ? "" : attr.Remark;
        }
    }
}