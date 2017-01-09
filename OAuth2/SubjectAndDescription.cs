using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilityNet.OAuth2
{
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
}
