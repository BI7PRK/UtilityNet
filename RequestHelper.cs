using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UtilityNet
{
    /// <summary>
    /// HttpMethod
    /// </summary>
    public enum Method
    {
        /// <summary>
        /// Get / Post
        /// </summary>
        All,
        /// <summary>
        /// Get
        /// </summary>
        Get,
        /// <summary>
        /// Post
        /// </summary>
        Post,
        /// <summary>
        /// Head
        /// </summary>
        Head
    }

    /// <summary>
    /// 接收值方法类
    /// </summary>
    [Serializable()]
    public class RequestHelper : IDisposable
    {
        public void Dispose()
        {
            
        }
        private Method _method = Method.All;
        private HttpRequest _Request;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="method"></param>
        public RequestHelper()
        {
            _Request = HttpContext.Current.Request;
        }


        public static RequestHelper Params
        {
            get { return new RequestHelper(); }
        }

        /// <summary>
        /// 接收Request[]方法. 已Trim()
        /// </summary>
        public string Widely(string key)
        {
            var str = Keys<string>(key);
            return str == null ? "" : str.Trim();
        }
        public T Widely<T>(string key)
        {
            return Keys<T>(key);
        }
        /// <summary>
        /// 接收Get方法. 已Trim()
        /// </summary>
        public string Get(string key)
        {
            _method = Method.Get;
            var str = Keys<string>(key);
            return str == null ? "" : str.Trim();
        }

        public T Get<T>(string key)
        {
            _method = Method.Get;
            return Keys<T>(key);
        }

       /// <summary>
        /// 接收Post方法. 已Trim()
       /// </summary>
       /// <param name="key"></param>
        /// <param name="html">是否允许 Html</param>
       /// <returns></returns>
        public string Post(string key, bool html = true)
        {
            _method = Method.Post;
            var str = Keys<string>(key);
            if (string.IsNullOrEmpty(str))
                return str;
            return html ? str : 
                str.Replace("<", "&lt;")
                .Replace(">", "&gt;");
        }
        public T Post<T>(string key)
        {
            _method = Method.Post;
            return Keys<T>(key);
        }

        /// <summary>
        /// 接收Headers参数. 已Trim()
        /// </summary>
        public string Head(string key)
        {
            _method = Method.Head;
            var str = Keys<string>(key);
            return str == null ? "" : str.Trim();
        }
        public T Head<T>(string key)
        {
            _method = Method.Head;
            return Keys<T>(key);
        }

        /// <summary>
        /// 不截断空格
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string Password(string name)
        {
            return this.Keys<string>(name);
        }

        /// <summary>
        /// 检查键名是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool HashKey(string key)
        {
            return (_Request.Params.Count > 0 && _Request.Params.AllKeys.Any(k => k == key))
                 || (_Request.Headers.Count > 0 && _Request.Headers.AllKeys.Any(k => k == key));
        }

        /// <summary>
        /// 泛型定义
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        private T Keys<T>(string name)
        {
            if (!HashKey(name))
            {
                return default(T);
                //throw new Exception(name + " 不存在");
            }

            string str = string.Empty;
            switch (_method) { 
                case Method.Get:
                    str = _Request.QueryString[name];
                    break;
                case Method.Post:
                    str = _Request.Form[name];
                    break;
                case Method.Head :
                     str = _Request.Headers[name];
                     break;
                default:
                    str = _Request[name];
                    break;
            }
            return ITryParse.TryParse<T>(str, default(T));
        }

        
        /// <summary>
        /// 获取SQL安全值，去掉[']符号
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string SqlSafe(string key)
        {
            return this.Keys<string>(key).Replace("'", "''");
        }
    }
}