using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UtilityNet
{
    public abstract class CookieHelper
    {
        public static void SetCookie(string cookieName, string cookieValue)
        {
            SetCookie(cookieName, cookieValue, null);
        }

        public static void SetCookie(string cookieName, string cookieValue, DateTime expires)
        {
            SetCookie(cookieName, cookieValue, null, null, expires);
        }
        public static void SetCookie(string cookieName, string cookieValue, string domain)
        {
            SetCookie(cookieName, cookieValue, domain, null);
        }

        public static void SetCookie(string cookieName, string cookieValue, string domain, string path)
        {
            SetCookie(cookieName, cookieValue, domain, path, DateTime.MinValue);
        }
        /// <summary>
        /// 设置 cookie
        /// </summary>
        /// <param name="cookieName">名称</param>
        /// <param name="cookieValue">值</param>
        /// <param name="domain">域</param>
        /// <param name="path">路径</param>
        /// <param name="expires">过期时间</param>
        public static void SetCookie(string cookieName, string cookieValue, string domain, string path, DateTime? expires)
        {
            HttpContext.Current.Response.Cookies.Remove(cookieName);

            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Value = HttpContext.Current.Server.UrlEncode(cookieValue);
            if ((expires.HasValue) && (expires.Value != DateTime.MinValue))
                cookie.Expires = expires.Value;

            cookie.Path = (string.IsNullOrEmpty(path)) ? "/" : path;

            if (!string.IsNullOrEmpty(domain))
                cookie.Domain = domain;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        /// <summary>
        /// 获取cookie值
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static string GetCookie(string cookieName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie == null)
                return string.Empty;
            return HttpContext.Current.Server.UrlDecode(cookie.Value);
        }
        /// <summary>
        /// 设置过期
        /// </summary>
        /// <param name="cookieName"></param>
        public static void Remove(string cookieName)
        {
            SetCookie(cookieName, "", DateTime.Now.AddDays(-1));
        }
    }
}