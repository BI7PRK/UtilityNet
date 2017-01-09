using System;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace UtilityNet
{
    /// <summary>
    /// 简单的json处理
    /// </summary>
    public class JsonSerializer
    {
        private JavaScriptSerializer serializer;
        public JsonSerializer()
        {
            serializer = new JavaScriptSerializer();
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public T Deserialize<T>(string json)
        {
            if (string.IsNullOrEmpty(json)) 
                return default(T);
            try
            {
                return serializer.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
#if debug
                Console.WriteLine(ex.Message);
#endif

            }
            return default(T);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Serialize(object obj)
        {
            if (obj == null) return null;
            string jsonString = serializer.Serialize(obj);
            
            //调换Json的Date字符串
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
            Regex reg = new Regex(@"\\/Date\(((-\d|\d)+)\)\\/");
            serializer = null;
            return reg.Replace(jsonString, matchEvaluator);

        }

        /// <summary>
        /// 将Json序列化的时候由/Date（1294499956278+0800）转为字符串
        /// </summary>
        private static string ConvertJsonDateToDateString(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            result = dt.ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }
    }
}
