using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilityNet.Http
{
    /// <summary>
    /// 在类里定义相关信息可自动生成表单
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class SubjectAndDescriptionAttribute : Attribute
    {
        public string Subject { get; private set; }
        public string Description { get; private set; }
        public bool ReadOnly { get; private set; }
        public object Value { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Index { get; private set; }

        public SubjectAndDescriptionAttribute(string subject, string desc, int index, bool readOnly = false)
        {
            this.Description = desc;
            this.Subject = subject;
            this.Index = index;
            this.ReadOnly = readOnly;
        }
    }
    /// <summary>
    /// 定义Uri传递值的参数
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class ParamNameAttribute : Attribute
    {
        /// <summary>
        /// 请求参数名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 请求参数默认值
        /// </summary>
        public object DefaultValue { get; private set; }
        /// <summary>
        /// 定义传递值的参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">默认值</param>
        public ParamNameAttribute(string name, object value = null)
        {
            this.Name = name;
            this.DefaultValue = value;
        }
    }
}
