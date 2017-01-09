using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UtilityNet.OAuth2
{
    /// <summary>
    /// 生成保存到数据库的SQL语句
    /// </summary>
    public class BuildDataValue<TEnty>
    {
        private string _SqlCommText = "INSERT INTO [OAuthConfig]([OAuthID],[KeyName],[KeyValue]) VALUES({0}, '{1}', '{2}');";

        public string SqlCommText
        {
            get { return _SqlCommText; }
            set { _SqlCommText = value; }
        }

        private TEnty _ConfigEntity;
        private int _ApiID;
        public BuildDataValue(TEnty enty, int apiId)
        {
            _ConfigEntity = enty;
            _ApiID = apiId;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (_ConfigEntity == null)
            {
                throw new ArgumentNullException();
            }
            var OfType = _ConfigEntity.GetType();
            var barr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty;
            foreach (var item in OfType.GetProperties(barr))
            {
                sb.AppendFormat(_SqlCommText, new object[] 
                { 
                    _ApiID,
                    item.Name, 
                    item.GetValue(_ConfigEntity, null) 
                });
            }
            return sb.ToString();
        }
    }
}
