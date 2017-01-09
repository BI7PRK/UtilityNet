using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using UtilityNet.Extension;
using System.IO;
using System.Security.Cryptography;


namespace UtilityNet
{
    public abstract class Utility
    {
 
        public static string MD5Encrypt(string str, bool shortChar = false)
        {
            return str.MD5Encrypt(shortChar);
        }
        /// <summary>
        /// 返回上一个页面
        /// </summary>
        public static void RedirectBack()
        {
            var Response = HttpContext.Current.Response;
            var url = RefPathAndQuery;
            if (string.IsNullOrEmpty(url)) return;
            Response.Redirect(url);
        }
        /// <summary>
        /// 获取上一页请求的 Url
        /// </summary>
        public static string RefPathAndQuery
        {
            get
            {
                var Request = HttpContext.Current.Request;

                if (Request.UrlReferrer == null) return "";
                return Request.UrlReferrer.PathAndQuery;
            }
        }

        /// <summary>
        /// 去除字符串中的 HTML 代码
        /// </summary>
        /// <param name="html">传入字符串</param>
        /// <returns>返回 TEXT 文本</returns>
        public static string RemoveHtml(string html)
        {
            Regex re = new Regex("<.*?>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            html = re.Replace(html, "");
            html = html.Replace("\r", "");
            html = html.Replace("\n", "");
            html = html.Replace("&nbsp;", "");
            re = null;
            return html.Replace("　", "");

        }


        /// <summary>
        /// 过滤HTML内容里的脚本
        /// </summary>
        /// <param name="sourceHtml">HTML内容</param>
        /// <returns>返回过滤后的</returns>
        public static string FilterScript(string sourceHtml)
        {
            string scriptPattern = @"<script.*?>(.|\n)*?</script\s*>|(?<=<\w+.*?)\son\w+="".+?""(?=.*?>)|<\w{2,}\s+[^>]*?javascript:[^>]*?>";
            var regx = new Regex(scriptPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            sourceHtml = regx.Replace(sourceHtml, string.Empty);
            regx = null;
            return sourceHtml;
        }

        /// <summary>
        /// 转到JS用的string
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns></returns>
        public static string ToJavaScriptString(string text)
        {
            StringBuffer buffer = new StringBuffer(text);
            buffer.Replace("\\", @"\\");
            buffer.Replace("\t", @"\t");
            buffer.Replace("\n", @"\n");
            buffer.Replace("\r", @"\r");
            buffer.Replace("\"", @"\""");
            buffer.Replace("\'", @"\'");
            buffer.Replace("/", @"\/");
            return buffer.ToString();
        }
        /// <summary>
        /// 文字串长度截断
        /// </summary>
        /// <param name="s">传入的字符串</param>
        /// <param name="length">要输出的字符数</param>
        /// <returns></returns>
        public static string CutString(string s, int length)
        {
            if (string.IsNullOrEmpty(s))
                return s;
            else
                s = RemoveHtml(s);
            byte[] strbyte = Encoding.Default.GetBytes(s);

            if (length >= strbyte.Length)
                return s;
            int n = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (Convert.ToInt32(s.ToCharArray()[i]) > 255)
                {
                    n += 2;
                }
                else
                {
                    n++;
                }
                if (n >= length)
                    break;
            }
            return Encoding.Default.GetString(strbyte, 0, n) + "...";
        }
        /// <summary>
        /// 获取文件MD5化值
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 获取用户IP地址
        /// </summary>
        public static string IPAddress
        {
            get {
                if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null) 
                {
                    return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
                else
                {
                    return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                } 
            }
        }
    }
}
