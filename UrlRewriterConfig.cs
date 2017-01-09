using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace UtilityNet
{
    /// <summary>
    /// 在 Web.config 中配置：
    /// <para>
    /// &lt;UrlRewriterConfig&gt;
    /// &lt;Rules>
    /// &lt;add Pattern="【正则表达式】" SendTo="【目标地址】" /&gt;
    /// &lt;/Rules&gt;
    /// </para>
    /// </summary>
    public class UrlRewriterConfig : ConfigurationSection
    {
        [ConfigurationProperty("Rules")]
        public UrlRewriterCollection Rules
        {
            get
            {
                return this["Rules"] as UrlRewriterCollection;
            }
        }
    }
    /// <summary>
    /// URL重写模式
    /// </summary>
    public enum RewriteMode
    { 
        /// <summary>
        /// 远程化
        /// </summary>
        Remote,
        /// <summary>
        /// 本地化
        /// </summary>
        Local
    }

    public class UrlRewriterCollection : ConfigurationElementCollection
    {

        protected override ConfigurationElement CreateNewElement()
        {
            return new UrlRewriterItem();
        }

        protected override object GetElementKey(System.Configuration.ConfigurationElement element)
        {
            return ((UrlRewriterItem)element).Pattern;
        }
    }

    public class UrlRewriterItem : ConfigurationElement
    {
        protected override void DeserializeElement(System.Xml.XmlReader reader, bool serializeCollectionKey)
        {
            base.DeserializeElement(reader, serializeCollectionKey);
        }
        [ConfigurationProperty("Pattern", IsRequired = true)]
        public string Pattern
        {
            get
            {
                return this["Pattern"] as string;
            }
        }
        [ConfigurationProperty("SendTo", IsRequired = true)]
        public string SendTo
        {
            get
            {
                return this["SendTo"].ToString();
            }
        }

        [ConfigurationProperty("Redirect", IsRequired = false)]
        public string Redirect
        {
            get
            {
                return this["Redirect"].ToString();
            }
        }

        [ConfigurationProperty("RewriteMode", IsRequired = false, DefaultValue = RewriteMode.Local)]
        public RewriteMode RewriteMode
        {
            get
            {
                var str = this["RewriteMode"].ToString();
                RewriteMode result = RewriteMode.Local;
                if (Enum.TryParse<RewriteMode>(str, true, out result))
                {
                    return result;
                }
                return UtilityNet.RewriteMode.Local;
            }
        }

    }
}
