using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

namespace UtilityNet.Http
{
    /// <summary>
    /// Http提交方法
    /// </summary>
    public abstract class HttpProxy
    {
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 9.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        private static int DefaultTimeout = 20 * 1000; //20秒

        private static WebProxy SetWebProxy
        {
            get
            {
                return new WebProxy
                {
                    UseDefaultCredentials = false
                };
            }
        }
        /// <summary>
        /// 生成提交字符串
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string ParamFormatString(IParams entity)
        {
            if (entity == null)
            {
                return "";
            }
            var entyType = entity.GetType();
            Console.WriteLine(entyType.FullName);
            var barr = BindingFlags.Instance | BindingFlags.Public;
            List<string> strUrl = new List<string>();
            foreach (var item in entyType.GetProperties(barr))
            {
                var useValue = item.GetValue(entity, null);
                var name = item.Name;
                var customAttr = (ParamNameAttribute)item.GetCustomAttributes(typeof(ParamNameAttribute), true).FirstOrDefault();
                if (customAttr != null)
                {
                    name = customAttr.Name;
                    var value = customAttr.DefaultValue;
                    if (useValue == null && value != null)
                    {
                        useValue = value;
                    }
                }

                if (!string.IsNullOrEmpty(name) && useValue != null)
                {
                    strUrl.Add(name + "=" + useValue);
                }
            }
            return string.Join("&", strUrl.ToArray());
        }

        private static string DictionaryString(Dictionary<string, object> dict)
        {
            if (dict == null || dict.Count == 0) return string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (var s in dict)
            {
                sb.Append("&" + s.Key + "=" + s.Value.ToString());
            }
            return sb.ToString().TrimStart('&');
        }
        /// <summary>
        /// Post提交
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static HttpResult HttpPost(string url, Dictionary<string, object> dict)
        {
            CookieCollection ck = null;
            return HttpPost(url, DictionaryString(dict), ref ck);
        }
        /// <summary>
        /// Post提交
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param">IParams 接口，内置属性信息可定义提交键值</param>
        /// <returns></returns>
        public static HttpResult HttpPost(string url, IParams param)
        {
            CookieCollection ck = null;
            return HttpPost(url, ParamFormatString(param), ref ck);
        }
        /// <summary>
        /// 包含Cookies信息的POST请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <param name="cookie">如果登陆时，创建一个新对象传入。</param>
        /// <returns></returns>
        public static HttpResult HttpPost(string url, string param, ref CookieCollection cookie)
        {
            var result = new HttpResult();
            result.RequesUrl = url;
            result.StatusCode = HttpStatusCode.BadRequest;
            
            HttpWebRequest request = null;
            try
            {
                byte[] post = Encoding.UTF8.GetBytes(param);
                request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.UserAgent = DefaultUserAgent;
                request.Proxy = SetWebProxy;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                request.ContentLength = post.Length;
                if (cookie != null)
                {
                    request.AllowAutoRedirect = false;
                    request.CookieContainer = new CookieContainer();
                    request.CookieContainer.Add(cookie);
                    request.KeepAlive = true;
                }
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(post, 0, post.Length);
                    reqStream.Close();
                }

                //LogHelper.WriteLog(Encoding.UTF8.GetString(post));

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    result.StatusCode = response.StatusCode;
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    result.Response = reader.ReadToEnd();
                    reader.Close();

                    if (cookie != null)
                    {
                        response.Cookies = request.CookieContainer.GetCookies(request.RequestUri);
                        cookie = response.Cookies;
                    }
                }
            }
            catch (WebException webEx)
            {
                result.Response = webEx.Message;
            }
            catch (UriFormatException urlEx)
            {
                result.Response = urlEx.Message;
            }
            finally
            {
                if (request != null)
                {
                    request.Abort();
                    request = null;
                }
            }
            return result;
        }
        


        /// <summary>
        /// Get方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static HttpResult HttpGet(string url, Dictionary<string, object> dict)
        {
            return HttpGet(url, DictionaryString(dict));
        }
        /// <summary>
        /// Get方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param">IParams 接口，内置属性信息可定义提交键值</param>
        /// <returns></returns>
        public static HttpResult HttpGet(string url, IParams param)
        {
            return HttpGet(url, ParamFormatString(param));
        }
       /// <summary>
        /// 包含Cookies信息的GET请求
       /// </summary>
       /// <param name="url"></param>
       /// <param name="query"></param>
       /// <param name="cookie">提交的Cookies信息</param>
       /// <returns></returns>
        public static HttpResult HttpGet(string url, string query, CookieCollection cookie = null)
        {
            var result = new HttpResult();
            result.RequesUrl = url;
            result.StatusCode = HttpStatusCode.BadRequest;
            HttpWebRequest request = null;
            try
            {
                //Console.WriteLine("HttpGet: " + query);
                request = (HttpWebRequest)HttpWebRequest.Create(url.TrimEnd('?') + (!string.IsNullOrEmpty(query) ? "?" + query : ""));
                request.Method = "GET";
                request.Proxy = SetWebProxy;
                request.UserAgent = DefaultUserAgent;
                request.ContentType = "text/html;charset=UTF-8";
                if (cookie != null)
                {
                    request.CookieContainer = new CookieContainer();
                    request.CookieContainer.Add(cookie);
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    result.Response = reader.ReadToEnd();
                    reader.Close();
                    response.Close();
                }
            }
            catch (WebException webEx)
            {
                result.Response = webEx.Message;
            }
            catch (UriFormatException urlEx)
            {
                result.Response = urlEx.Message;
            }
            finally
            {
                if (request != null)
                {
                    request.Abort();
                    request = null;
                }
            }
            return result;
        }
        /// <summary>
        /// 获取一个图片数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static Image GetImage(string url, CookieCollection cookie = null)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Proxy = SetWebProxy;
                if (cookie != null)
                {
                    request.CookieContainer = new CookieContainer();
                    request.CookieContainer.Add(cookie);
                }
                var stream = ((HttpWebResponse)request.GetResponse()).GetResponseStream();
                var img = Image.FromStream(stream);
                stream.Close();
                request.Abort();
                return img;
            }
            catch (Exception ex)
            {
#if Debug
                Console.WriteLine(ex.Message);
#endif
            }

            return null;
        }

        /// <summary>
        /// 下载一个文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="saveName"></param>
        /// <param name="cookie"></param>
        /// <param name="ReadAction">回调(total, read)</param>
        /// <returns></returns>
        public static string DowloadFile(string url, string saveName, CookieCollection cookie = null, Action<long, long> ReadAction = null)
        {
            try
            {
                HttpWebRequest myReqyest = (HttpWebRequest)HttpWebRequest.Create(url);
                myReqyest.Proxy = SetWebProxy;
                myReqyest.UserAgent = DefaultUserAgent;
                myReqyest.Method = "GET";
                if (cookie != null)
                {
                    myReqyest.CookieContainer = new CookieContainer();
                    myReqyest.CookieContainer.Add(cookie);
                }
                HttpWebResponse getRespinst = (HttpWebResponse)myReqyest.GetResponse();
                long totalBytes = getRespinst.ContentLength;

                Stream _stream = getRespinst.GetResponseStream();
                Stream newStream = new FileStream(saveName, FileMode.Create);

                byte[] setbyte = new byte[1024];
                int reader = _stream.Read(setbyte, 0, setbyte.Length);
                long totalDownloadedByte = reader;
                while (reader > 0)
                {
                    newStream.Write(setbyte, 0, reader);
                    reader = _stream.Read(setbyte, 0, setbyte.Length);
                    totalDownloadedByte += reader;
                    if (ReadAction != null)
                    {
                        ReadAction.Invoke(totalBytes, totalDownloadedByte);
                    }
                }
                newStream.Close();
                newStream.Dispose();
                _stream.Close();
                getRespinst.Close();
                myReqyest.Abort();
                return saveName;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// 模拟表单上传文件
        /// </summary>
        /// <param name="url">上传地址</param>
        /// <param name="file">要上传的文件路径</param>
        /// <param name="fileId">表单的文件字段</param>
        /// <param name="cookie">发送Cookies信息</param>
        /// <returns>返回服务器响应</returns>
        public static string FormPostFile(string url, string file, string fileId, CookieCollection cookie = null)
        {
            string strBoundary = "----------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + strBoundary + "\r\n");

            StringBuilder sb = new StringBuilder();
            sb.Append("--");
            sb.Append(strBoundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"");
            sb.Append(fileId);
            sb.Append("\"; filename=\"");
            sb.Append(Path.GetFileName(file));
            sb.Append("\"");
            sb.Append("\r\n");
            sb.Append("Content-Type: ");
            sb.Append("application/octet-stream");
            sb.Append("\r\n");
            sb.Append("\r\n");

            string strPostHeader = sb.ToString();
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(strPostHeader);
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Proxy = SetWebProxy;
                request.ContentType = "multipart/form-data; boundary=" + strBoundary;
                request.Method = "POST";
                request.UserAgent = DefaultUserAgent;
                // This is important, otherwise the whole file will be read to memory anyway...
                request.AllowWriteStreamBuffering = false;
                if (cookie != null)
                {
                    request.AllowAutoRedirect = false;
                    request.CookieContainer = new CookieContainer();
                    request.CookieContainer.Add(cookie);
                    request.KeepAlive = true;
                }
                var oFileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                long length = postHeaderBytes.Length + oFileStream.Length + boundaryBytes.Length;
                request.ContentLength = length;
                using (var oRequestStream = request.GetRequestStream())
                {
                    oRequestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                    // Stream the file contents in small pieces (4096 bytes, max).
                    byte[] buffer = new byte[checked((uint)Math.Min(4096, (int)oFileStream.Length))];
                    int bytesRead = 0;
                    while ((bytesRead = oFileStream.Read(buffer, 0, buffer.Length)) != 0)
                        oRequestStream.Write(buffer, 0, bytesRead);
                    oFileStream.Close();

                    // Add the trailing boundary
                    oRequestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                }
                using (var oWResponse = request.GetResponse())
                {
                    Stream s = oWResponse.GetResponseStream();
                    return new StreamReader(s).ReadToEnd();
                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 提交流，并下载流。用于网络授权
        /// </summary>
        public static void PostAndDownload(Stream postedStream, string uploadURL, string localFile)
        {
            try
            {
                FileInfo loc = new FileInfo(localFile);
                if (loc.Exists)
                    loc.Attributes = FileAttributes.Normal;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uploadURL);
                request.Method = "POST";
                request.Proxy = SetWebProxy;
                request.ServicePoint.Expect100Continue = false;
                request.AllowWriteStreamBuffering = false;
                request.KeepAlive = false;
                request.Timeout = 2000 * 60;
                request.ContentType = "application/octetstream";
                //request.SendChunked = true;
                // 设置SendChunked为true而不必设ContentLength, 反之亦然
                request.ContentLength = postedStream.Length;
                using (Stream requestStream = request.GetRequestStream())
                {
                    var buffer = new byte[postedStream.Length];
                    postedStream.Read(buffer, 0, (int)postedStream.Length);
                    requestStream.Write(buffer, 0, (int)postedStream.Length);
                }

                var webResp = (HttpWebResponse)request.GetResponse();
                byte[] outBuffer = new byte[1024];
                Stream readStream = webResp.GetResponseStream();

                Stream fileStream = new FileStream(localFile, FileMode.Create);
                int ready = 0;
                while ((ready = readStream.Read(outBuffer, 0, outBuffer.Length)) > 0)
                {
                    fileStream.Write(outBuffer, 0, ready);
                }
                readStream.Close();
                fileStream.Close();


                loc.Attributes = FileAttributes.Hidden | FileAttributes.ReadOnly;
                loc = null;

                webResp.Close();
                request.Abort();
            }
            catch (WebException ex)
            {

                throw ex;
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

    }
}
