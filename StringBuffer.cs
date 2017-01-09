using System;
using System.Text;
using System.Collections.Generic;

namespace UtilityNet
{
    /// <summary>
    /// StringBuilder扩展
    /// </summary>
	public class StringBuffer
	{
		private StringBuilder m_InnerBuilder = new StringBuilder();

		public StringBuilder InnerBuilder
		{
			get { return m_InnerBuilder; }
		}
        /// <summary>
        /// 获取长度
        /// </summary>
        public int Length
        {
            get
            {
                return m_InnerBuilder.Length;
            }
        }
        /// <summary>
        /// 删除项
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        public void Remove(int startIndex, int length)
        {
            m_InnerBuilder.Remove(startIndex, length);
        }
        /// <summary>
        /// 替换项
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        public void Replace(string oldValue, string newValue)
        {
            m_InnerBuilder.Replace(oldValue, newValue);
        }
        /// <summary>
        /// 初始货
        /// </summary>
		public StringBuffer()
		{
			m_InnerBuilder = new StringBuilder();
		}
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="capacity">此实例的建议起始大小。</param>
		public StringBuffer(int capacity)
		{
			m_InnerBuilder = new StringBuilder(capacity);
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">用于初始化实例值的字符串。如果 value 为 null，则新的 System.Text.StringBuilder 将包含空字符串（即包含 System.String.Empty）</param>
		public StringBuffer(string value)
		{
			m_InnerBuilder = new StringBuilder(value);
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"> 建议起始大小。</param>
        /// <param name="maxCapacity">最多大小</param>
		public StringBuffer(int capacity, int maxCapacity)
		{
			m_InnerBuilder = new StringBuilder(capacity, maxCapacity);
		}

		public StringBuffer(string value, int capacity)
		{
			m_InnerBuilder = new StringBuilder(value, capacity);
		}

		public StringBuffer(string value, int startIndex, int length, int capacity)
		{
			m_InnerBuilder = new StringBuilder(value, startIndex, length, capacity);
		}

		public StringBuffer(StringBuilder innerBuilder)
		{
			m_InnerBuilder = innerBuilder;
		}

		public static StringBuffer operator +(StringBuffer buffer, bool value)
		{
			buffer.InnerBuilder.Append(value);

			return buffer;
		}
		public static StringBuffer operator +(StringBuffer buffer, byte value)
		{
			buffer.InnerBuilder.Append(value);

			return buffer;
		}
		public static StringBuffer operator +(StringBuffer buffer, char value)
		{
			buffer.InnerBuilder.Append(value);

			return buffer;
		}
		public static StringBuffer operator +(StringBuffer buffer, char[] value)
		{
			buffer.InnerBuilder.Append(value);

			return buffer;
		}
		public static StringBuffer operator +(StringBuffer buffer, decimal value)
		{
			buffer.InnerBuilder.Append(value);

			return buffer;
		}
		public static StringBuffer operator +(StringBuffer buffer, double value)
		{
			buffer.InnerBuilder.Append(value);

			return buffer;
		}
		public static StringBuffer operator +(StringBuffer buffer, float value)
		{
			buffer.InnerBuilder.Append(value);

			return buffer;
		}
		public static StringBuffer operator +(StringBuffer buffer, int value)
		{
			buffer.InnerBuilder.Append(value);

			return buffer;
		}
		public static StringBuffer operator +(StringBuffer buffer, long value)
		{
			buffer.InnerBuilder.Append(value);

			return buffer;
		}
		public static StringBuffer operator +(StringBuffer buffer, object value)
		{
			buffer.InnerBuilder.Append(value);

			return buffer;
		}
		public static StringBuffer operator +(StringBuffer buffer, sbyte value)
		{
			buffer.InnerBuilder.Append(value);

			return buffer;
		}
		public static StringBuffer operator +(StringBuffer buffer, short value)
		{
			buffer.InnerBuilder.Append(value);

			return buffer;
		}
		public static StringBuffer operator +(StringBuffer buffer, string value)
		{
			buffer.InnerBuilder.Append(value);

			return buffer;
		}
		public static StringBuffer operator +(StringBuffer buffer, uint value)
		{
			buffer.InnerBuilder.Append(value);

			return buffer;
		}
		public static StringBuffer operator +(StringBuffer buffer, ulong value)
		{
			buffer.InnerBuilder.Append(value);

			return buffer;
		}
		public static StringBuffer operator +(StringBuffer buffer, ushort value)
		{
			buffer.InnerBuilder.Append(value);

			return buffer;
		}

		public override string ToString()
		{
			return InnerBuilder.ToString();
		}
	}
}