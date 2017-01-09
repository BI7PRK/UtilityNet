using System.Text.RegularExpressions;

namespace UtilityNet.OAuth2
{
    public abstract class ResponseBase
    {
        public ResponseBase(string resbody)
        {
            this.ResonseBody = resbody;

            if (string.IsNullOrWhiteSpace(this.ResonseBody))
            {
                this.StateCode = -1;
                this.Message = "无返回信息";
                this.IsError = true;
                return;
            }
            var body = this.ResonseBody.TrimEnd().TrimStart();

            if (!body.StartsWith("{") && !body.EndsWith("}"))
            {
                this.StateCode = -1;
                this.Message = this.ResonseBody;
                this.IsError = true;
                return;
            }

            bool result = false;
            var pattern = "\\{\"errcode\":(\\d+|-\\d+),\"errmsg\":\"(.+?)\"\\}";
            Regex re = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var match = re.Match(this.ResonseBody);
            if (match.Success)
            {
                this.StateCode = long.Parse(match.Groups[1].Value);
                this.Message = match.Groups[2].Value;
                if (string.IsNullOrEmpty(this.Message))
                {
                    this.Message = match.Groups[2].Value;
                }

                result = this.StateCode != 0L;
            }
            else
            {
                this.Message = "OK";
            }
            this.IsError = result;
        }
        /// <summary>
        /// 是否错误，必须进行判断才可以获取错误信息（本判断基于微信平台）
        /// </summary>
        public bool IsError
        {
            get;
            private set;
        }

        /// <summary>
        /// 返回代码
        /// </summary>
        public long StateCode { get; set; }
        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 响应的原始内容
        /// </summary>
        public string ResonseBody { get; private set; }

    }
}
