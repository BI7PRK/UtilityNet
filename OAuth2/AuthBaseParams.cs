using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityNet.Http;
namespace UtilityNet.OAuth2
{
    /// <summary>
    /// Authorize请求的基类参数
    /// </summary>
    public class AuthUriBaseParams : IParams
    {
        /// <summary>
        /// 申请应用时分配的AppID/AppKey
        /// 默认 ParamName = client_id
        /// </summary>
        [ParamName("client_id")]
        public virtual string ClientID { get; set; }
        /// <summary>
        /// 请求类型
        /// 默认 ParamName = response_type， 值为 code
        /// </summary>
        [ParamName("response_type", "code")]
        public virtual string ResponseType { get; set; }
        /// <summary>
        /// 申请scope权限所需参数，可一次申请多个scope权限，用逗号分隔
        /// 默认 ParamName = scope
        /// </summary>
        [ParamName("scope")]
        public virtual string Scope { get; set; }
        /// <summary>
        /// 授权回调地址，站外应用需与设置的回调地址一致
        /// 默认 ParamName = redirect_uri
        /// </summary>
        [ParamName("redirect_uri")]
        public virtual string RedirectUri { get; set; }
        /// <summary>
        /// 用于保持请求和回调的状态，在回调时，会在Query Parameter中回传该参数
        /// 默认 ParamName = state
        /// </summary>
        [ParamName("state")]
        public virtual string State { get; set; }
        /// <summary>
        /// 授权页面的终端类型，取值参见各提供平台的API文档
        /// 默认 ParamName = display
        /// </summary>
        [ParamName("display")]
        public virtual string Display { get; set; }
    }
}
