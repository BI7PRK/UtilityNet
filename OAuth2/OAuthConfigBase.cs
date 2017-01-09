using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UtilityNet.OAuth2
{
    public class OAuthConfigBase
    {
        public OAuthConfigBase(string name)
        {
            this.AppName = name;
        }
        /// <summary>
        /// 接口名称
        /// </summary>
        [SubjectAndDescription("接口名称", "", 0, true)]
        public string AppName { get; private set; }
        /// <summary>
        /// 接口授权ID
        /// </summary>
        [SubjectAndDescription("接口ID", "开放接口提供方所给的AppID", 1)]
        public string AppId { get; set; }
        /// <summary>
        /// 接口授权密钥
        /// </summary>
        [SubjectAndDescription("接口密钥", "开放接口提供方所给的App Key", 2)]
        public string OAuthKey { get; set; }
    }
}