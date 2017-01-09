using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace UtilityNet.Extension
{
    public static class StringExtend
    {
        /// <summary>
        /// 过滤html标签，格式化换行
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToHtml(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var str = text.Replace("&", "&amp;");
            str = str.Replace(" ", "&nbsp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("\r\n", "<br />");
            str = str.Replace("\r", "<br />");
            str = str.Replace("\n", "<br />");
            return str;
        }
        /// <summary>
        /// 过滤 &lt;及&gt;
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string NotHtml(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            return text;
        }
        /// <summary>
        /// 还原 &lt;及&gt;
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string FormatHtmlChar(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            text = text.Replace("&lt;", "<");
            text = text.Replace("&gt;", ">");
            return text;
        }

        /// <summary>
        /// 将字母首字大写化
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string FirstUpper(this string s)
        {
            if (string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s)) return s;
            string first = s.Substring(0, 1).ToUpper();
            string any = s.Substring(1);
            return string.Concat(first, any);
        }


        /// <summary>
        /// MD5加密，默认为32位
        /// </summary>
        /// <param name="chars">要加密的字符串</param>
        /// <param name="shortx">true时，输出16位</param>
        /// <returns></returns>
        public static string MD5Encrypt(this string chars, bool shortx = false)
        {
            if (string.IsNullOrEmpty(chars)) return "";
            string strHex = string.Empty;
            MD5CryptoServiceProvider md = new MD5CryptoServiceProvider();
            byte[] result = md.ComputeHash(Encoding.Default.GetBytes(chars));
            for (int i = 0; i < 16; i++)
            {
                strHex += string.Format("{0:x2}", result[i]);
            }
            if (shortx)
            {
                strHex = strHex.Substring(8, 16);
            }
            return strHex.ToUpper();
        }

        private static string PRIVATE_DESKEY = "goslam.cn";

        /// <summary>
        /// DES加密，使用内内置密钥
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <returns></returns>
        /// </summary>
        public static string DESEncrypt(this string toEncrypt)
        {
            return DESEncrypt(toEncrypt, PRIVATE_DESKEY);
        }

        /// <summary>
        ///  DES加密
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <param name="pKey">指定钥，长度不限</param>
        /// <returns></returns>
        /// </summary>
        public static string DESEncrypt(this string toEncrypt, string pKey)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(pKey.MD5Encrypt());
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        /// <summary>
        /// DES解密，使用内内置密钥
        /// </summary>
        /// <param name="toDecrypt"></param>
        /// <returns></returns>
        public static string DESDecrypt(this string toDecrypt)
        {
            return DESDecrypt(toDecrypt, PRIVATE_DESKEY);
        }
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="toDecrypt"></param>
        /// <param name="pKey">指定钥，长度不限</param>
        /// <returns></returns>
        public static string DESDecrypt(this string toDecrypt, string pKey)
        {
            if (string.IsNullOrEmpty(toDecrypt)) return toDecrypt;

            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(pKey.MD5Encrypt());
            try
            {
                byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

                RijndaelManaged rDel = new RijndaelManaged();
                rDel.Key = keyArray;
                rDel.Mode = CipherMode.ECB;
                rDel.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = rDel.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// SQL特殊符转换 [%]，[_]，[']
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToSqlValue(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            str = str.Replace("%", "[%]");
            str = str.Replace("_", "[_]");
            str = str.Replace("'", "['']");
            return str;
        }



    }
}
