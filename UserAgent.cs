using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace UtilityNet
{
    /// <summary>
    /// 获取客户端请求信息
    /// </summary>
    public class UserAgent
    {

        private HttpBrowserCapabilities browser;
        private string userAgent;
        /// <summary>
        /// 提供来访者信息的类
        /// </summary>
        public UserAgent()
        {
            var _Request = HttpContext.Current.Request;
            browser = _Request.Browser;
            userAgent = _Request.UserAgent;
            this.TargetUrl = _Request.Url.PathAndQuery;
            var uri = _Request.UrlReferrer;
            this.FromUrl = uri == null ? "" : uri.ToString();

            this.GetIPAddress();
        }

        private void GetIPAddress()
        {
            string result = String.Empty;
            result = HttpContext.Current.Request.ServerVariables["HTTP_VIA"];
            if (!string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(result))
                {
                    //可能有代理 
                    if (result.IndexOf(".") == -1) //没有"."肯定是非IPv4格式 
                        result = null;
                    else
                    {
                        if (result.IndexOf(",") >= 0)
                        {
                            //有","，估计多个代理。取第一个不是内网的IP。 
                            result = result.Replace(" ", "").Replace("'", "").Replace(";", "");
                            string[] temparyip = result.Split(',');
                            for (int i = 0; i < temparyip.Length; i++)
                            {
                                if (IsIPAddress(temparyip[i]))
                                {
                                    if (!temparyip[i].StartsWith("10.") && !temparyip[i].StartsWith("192.168") && !temparyip[i].StartsWith("172.16."))
                                    {
                                        result = temparyip[i]; //找到不是内网的地址
                                        break;
                                    }
                                }
                            }
                        }
                        if (!IsIPAddress(result)) //代理即是IP格式 
                        {
                            result = null; //代理中的内容 非IP，取IP k
                        }
                    }
                }
            }
            else
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            if (string.IsNullOrEmpty(result))
            {
                result = "000.000.000.000";
            }
            this.IPAddress = result;
        }

        private bool IsIPAddress(string str)
        {
            if (string.IsNullOrWhiteSpace(str) || str.Length < 7 || str.Length > 15)
                return false;

            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}{1}";
            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            var result = regex.IsMatch(str);
            regex = null;
            return result;
        }  


        /// <summary>
        /// 原始的UserAgent信息
        /// </summary>
        public string UserAgentString
        {
            get
            {
                return userAgent;
            }
        }
        /// <summary>
        /// 是否为搜索引擎
        /// </summary>
        public bool IsCrawler
        {
            get
            {
                return browser.Crawler;
            }
        }
        /// <summary>
        /// 是否为移动设备
        /// </summary>
        public bool IsMobile
        {
            get
            {
                return browser.IsMobileDevice 
                    && !browser.Win16 
                    && !browser.Win32;
            }
        }
        /// <summary>
        /// 访问来源
        /// </summary>
        public string FromUrl
        {
            get;
            private set;
        }
        /// <summary>
        /// 访问目标
        /// </summary>
        public string TargetUrl
        {
            get;
            private set;
        }

        /// <summary>
        /// 移动设备型号
        /// </summary>
        public string DeviceModel
        {
            get
            {
                string model = string.Empty;
                if (!string.IsNullOrEmpty(userAgent) && IsMobile)
                {
                    if (userAgent.IndexOf("iPhone") > -1)
                    {
                        var re = new Regex("Version/(.*?)\\s", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        var m = re.Match(userAgent);
                        if (m.Success)
                        {
                            model = "iPhone " + m.Groups[1].Value;
                        }
                        re = null;
                    }
                    else
                    {
                        var regEx = new Regex("\\((.+?)\\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        var mmath = regEx.Match(userAgent);
                        if (mmath.Success)
                        {
                            var str = mmath.Groups[1].Value;
                            var from = str.Split(';').LastOrDefault();
                            if (!string.IsNullOrEmpty(from))
                            {
                                model = from.Substring(0, from.IndexOf("Build/"));
                            }
                        }
                        regEx = null;
                    }
                }
                return model;
            }
        }

        /// <summary>
        /// 系统平台
        /// </summary>
        public string Platform
        {
            get
            {
                if (!string.IsNullOrEmpty(userAgent) && IsMobile)
                {
                    if (userAgent.IndexOf("iPhone") > -1)
                    {
                        return "iPhone OS";
                    }
                    else
                    {
                        var regEx = new Regex("\\((.+?)\\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        var mmath = regEx.Match(userAgent);
                        if (mmath.Success)
                        {
                            var str = mmath.Groups[1].Value;
                            var arr = str.Split(';');
                            if (arr.Length > 3)
                            {
                                return arr[arr.Length - 3];
                            }
                            else if (arr.Length > 0 && arr.Length < 3)
                            {
                                return arr[1];
                            }
                        }
                        regEx = null;
                    }

                }
                return browser.Platform;

            }
        }

        /// <summary>
        /// 浏览器名称
        /// </summary>
        public string Browser
        {
            get
            {
                var arr = MobileBrowser();
                return (arr != null) ? arr.FirstOrDefault() : browser.Browser;
            }
        }

        /// <summary>
        /// 浏览器版本
        /// </summary>
        public string BrowserVersion
        {
            get
            {
                var arr = MobileBrowser();
                return (arr != null) ? arr.LastOrDefault() : browser.Version;
            }
        }

        /// <summary>
        /// 来访IP
        /// </summary>
        public string IPAddress { get; private set; }

        private string[] MobileBrowser()
        {
            if (IsMobile && !string.IsNullOrEmpty(userAgent))
            {
                var re = new Regex(" ([a-z]+)/([\\d+\\.]{2,})",
                    RegexOptions.Compiled | RegexOptions.IgnoreCase);
                var ms = re.Matches(userAgent);
                if (ms.Count > 0)
                {
                    var m = ms[ms.Count - 1];
                    return new string[] { m.Groups[1].Value, m.Groups[2].Value };
                }
                re = null;
            }
            return null;
        }
    }
}