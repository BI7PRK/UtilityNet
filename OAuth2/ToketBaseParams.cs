using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityNet.Http;

namespace UtilityNet.OAuth2
{
    /// <summary>
    /// 获取AccessToket的请求参数
    /// </summary>
    public class ToketBaseParams : IParams
    {
        /// <summary>
        /// 申请应用时分配的AppID/AppKey
        /// 默认ParamName=client_id
        /// </summary>
        [ParamName("client_id")]
        public virtual string ClientID { get; set; }
        /// <summary>
        /// 申请应用时分配的AppSecret。 
        /// 默认ParamName=client_secret
        /// </summary>
        [ParamName("client_secret")]
        public virtual string ClientSecret { get; set; }
        /// <summary>
        /// 请求的类型
        /// 默认ParamName=grant_type， 值为 authorization_code
        /// </summary>
        [ParamName("grant_type", "authorization_code")]
        public virtual string GrantType { get; set; }
        /// <summary>
        /// 回调地址
        /// 默认ParamName=redirect_uri
        /// </summary>
        [ParamName("redirect_uri")]
        public virtual string RedirectUri { get; set; }

        /// <summary>
        /// 换取Toket的代码
        /// 默认ParamName=code
        /// </summary>
        [ParamName("code")]
        public virtual string Code { get; set; }
    }
}
