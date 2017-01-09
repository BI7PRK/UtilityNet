using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityNet.Http;

namespace UtilityNet.OAuth2
{
    public class OpenIdBaseParams : IParams
    {
        /// <summary>
        /// 获取了 AccessToken
        /// 默认 ParamName = access_token
        /// </summary>
        [ParamName("access_token")]
        public virtual string AccessToken { get; set; }
        /// <summary>
        /// 语言，例如微信获取用户信息时就用到
        /// 默认 ParamName = lang
        /// </summary>
        [ParamName("lang")]
        public virtual string Language { get; set; }
    }
}
