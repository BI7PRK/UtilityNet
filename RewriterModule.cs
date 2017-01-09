using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace UtilityNet
{
    /// <summary>
    /// 在 Web.config 中配置：
    /// <para>
    ///  &lt;configSections&gt;
    ///  &lt;section name="UrlRewriterConfig" requirePermission="true"  type="UtilityNet.UrlRewriterConfig,UtilityNet"/&gt;
    ///  &lt;/configSections&gt;
    ///  &lt;system.webServer&gt;
    ///  &lt;modules runAllManagedModulesForAllRequests="true"&gt;
    ///  &lt;add name="MyRewriter" type="UtilityNet.URLRewriter"/&gt;
    ///  &lt;/modules&gt;
    ///  &lt;/system.webServer&gt;
    /// </para>
    /// </summary>
    public class RewriterModule : IHttpModule
    {
        public void Dispose() { }

        public void Init(HttpApplication app)
        {
            app.BeginRequest += new EventHandler(this.URLRequest);
            //app.AuthenticateRequest += new EventHandler(this.URLRequest);
        }

        protected void URLRequest(object sender, EventArgs e)
        {
            HttpContext context = (sender as HttpApplication).Context;
            var obj = GetExecutionPath(context);

            //context.Response.Write("URLRequest: " + obj.RewritePath);
            //context.Response.Write("QueryString: " + obj.QueryString);

            if (!string.IsNullOrEmpty(obj.RewritePathAndQuery))
            {
                context.RewritePath(obj.RewritePath, obj.PathInfo, obj.QueryString);
            }
            else if (!string.IsNullOrEmpty(obj.RedirectUrl))
            {
                context.Response.Redirect(obj.RedirectUrl, true);
            }
            else if (!File.Exists(context.Server.MapPath(context.Request.Path)))
            {
                throw new HttpException(404, "Page Not Found");
            }
        }

        internal struct ExecutionPath
        {
            public string RewritePathAndQuery;
            public string RedirectUrl;
            public string QueryString;
            public string RewritePath;
            public string RawUrl;
            public string PathInfo;
        }


        internal static ExecutionPath GetExecutionPath(HttpContext context)
        {
            var obj = new ExecutionPath();
            obj.PathInfo = context.Request.PathInfo;
            obj.RawUrl = context.Request.RawUrl.ToLower();
            //读取配置信息
            var config = ConfigurationManager.GetSection("UrlRewriterConfig") as UrlRewriterConfig;
            //遍历配置信息并使用正则匹配
            foreach (UrlRewriterItem item in config.Rules)
            {
                if (item.RewriteMode == RewriteMode.Remote)
                {
                    obj.RawUrl = context.Request.Url.ToString();
                }
                var mc = Regex.Match(obj.RawUrl, item.Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (mc.Success)
                {
                    //将匹配后的参数保存到一个数组或列表中
                    List<string> args = new List<string>();
                    for (int i = 1; i < mc.Groups.Count; i++)
                    {
                        args.Add(mc.Groups[i].Value);
                    }
                    //格式化重写路径，替换相应位置的参数
                    obj.RedirectUrl = string.Format(item.Redirect, args.ToArray());
                    if (!string.IsNullOrEmpty(item.SendTo))
                    {
                        obj.RewritePathAndQuery = string.Format(item.SendTo, args.ToArray());
                    }
                    args = null;
                    break;
                }
            }
            string[] arr;
            if (!string.IsNullOrEmpty(obj.RewritePathAndQuery))
            {
                arr = obj.RewritePathAndQuery.Split('?');
            }
            else
            {
                arr = obj.RawUrl.Split('?');
            }

            if (arr.Length >= 1 || arr.Length == 0)
            {
                obj.RewritePath = arr[0];
            }

            if (arr.Length == 2)
            {
                obj.QueryString = arr[1];
            }
            return obj;
        }

    }

}
