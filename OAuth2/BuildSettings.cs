using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace UtilityNet.OAuth2
{
    public abstract class BuildSettings<T> where T : OAuthConfigBase, new()
    {
        public static string GenerationHtmlForm(T entity)
        {
            if (entity == null) entity = new T();
            var OfType = entity.GetType();
            var barr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty;
            var list = new List<SubjectAndDescriptionAttribute>();
            
            foreach (var item in OfType.GetProperties(barr))
            {
                var customAttr = item.GetCustomAttributes(typeof(SubjectAndDescriptionAttribute), true);
                if (customAttr!= null && customAttr.Length > 0)
                {
                    var attr = (SubjectAndDescriptionAttribute)customAttr[0];
                    attr.Value = item.GetValue(entity, null);
                    attr.Name = item.Name.ToLower();
                    list.Add(attr);
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul>");
            sb.Append(string.Join("\r\n", list.OrderBy(i => i.Index).Select(attr =>
            {
                string str = "";
                str += "<li>";
                str += attr.Subject + ":";
                if (attr.ReadOnly)
                {
                    str += attr.Value;
                }
                else
                {
                    str += "<input type=\"text\" value=\"" + attr.Value + "\" id=\"" + attr.Name + "\" name=\"" + attr.Name + "\" />";
                }
                str += "<span>" + attr.Description + "</span>";
                str += "</li>";
                return str;
            })));
            sb.Append("</ul>");

            return sb.ToString();
        }
    }
}