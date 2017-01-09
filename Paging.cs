using System.Data;
using System.Text.RegularExpressions;
using System;
using System.Text;
namespace UtilityNet
{
    public sealed class Paging
    {
        public class LangText
        {
            private string _FirstPage = "首页";

            public string FirstPage
            {
                get { return _FirstPage; }
                set { _FirstPage = value; }
            }
            private string _LastPage = "上一页";

            public string LastPage
            {
                get { return _LastPage; }
                set { _LastPage = value; }
            }
            private string _NextPage = "下一页";

            public string NextPage
            {
                get { return _NextPage; }
                set { _NextPage = value; }
            }
            private string _EndPage = "尾页";

            public string EndPage
            {
                get { return _EndPage; }
                set { _EndPage = value; }
            }
        }

        /// <summary>
        /// 输出分页链接
        /// </summary>
        /// <param name="totalRows">总记录数</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="pagaIndex">当前页码</param>
        /// <returns></returns>
        public static string AjaxPageTurning(int totalRows, int pageCount, int pagaIndex, string fun)
        {
            string strHtml = "";
            pagaIndex = (pagaIndex <= 0) ? pagaIndex = 1 : pagaIndex;
            pagaIndex = (pagaIndex >= pageCount) ? pagaIndex = pageCount : pagaIndex;

            if (pagaIndex > 5)
            {
                strHtml += "<span class=\"first_link\"><a href=\"javascript:void(0);\" onclick=\"" + fun + "(1);return false;\">首页</a></span>";
                strHtml += "... ";
            }
            if (pagaIndex > 1)
            {
                strHtml += "<span class=\"up_link\"><a href=\"javascript:void(0);\" onclick=\"" + fun + "(" + (pagaIndex - 1).ToString() + ");return false;\">上一页</a></span>";
            }
            else
            {
                strHtml += "<span class=\"up_nolink\">上一页</span>";
            }
            int showNum = 10;
            int index = pagaIndex - (showNum / 2);
            int sumSetp = 1;
            index = (index < 1) ? 1 : index;
            while (sumSetp <= showNum)
            {
                if (index > pageCount) break;
                if (pagaIndex == index)
                {
                    strHtml += "<span class=\"currlink\">" + index.ToString() + "</span>";
                }
                else
                {
                    strHtml += "<span class=\"pagelink\"><a href=\"javascript:void(0);\" onclick=\"" + fun + "(" + index.ToString() + ");return false;\">" + index + "</a></span>";
                }
                sumSetp++;
                index++;
            }
            if (pagaIndex < pageCount)
            {
                strHtml += "<span class=\"next_link\"><a href=\"javascript:void(0);\" onclick=\"" + fun + "(" + (pagaIndex + 1).ToString() + ");return false;\">下一页</a></span>";
            }
            else
            {
                strHtml += "<span class=\"next_nolink\">下一页</span>";
            }
            if (pagaIndex < (pageCount - 5))
            {
                strHtml += "... ";
                strHtml += "<span class=\"last_link\"><a href=\"javascript:void(0);\" onclick=\"" + fun + "(" + pageCount.ToString() + ");return false;\">尾页</a></span>";
            }
            return strHtml;
        }

        /// <summary>
        /// 输出分页链接
        /// </summary>
        /// <param name="totalRows">总记录数</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="pagaIndex">当前页码</param>
        /// <param name="anyLink">其它链接参数（如：string=get&value=10）</param>
        /// <returns></returns>
        public static string PageTurning(int totalRows, int pageCount, int pagaIndex, string anyLink = null)
        {
            string strHtml = "";
            pagaIndex = (pagaIndex <= 0) ? pagaIndex = 1 : pagaIndex;
            pagaIndex = (pagaIndex >= pageCount) ? pagaIndex = pageCount : pagaIndex;
            anyLink = (!string.IsNullOrEmpty(anyLink)) ? ("&" + anyLink) : "";
            
            if (pagaIndex > 5)
            {
                strHtml += "<span class=\"first_link\"><a href=\"?page=1" + anyLink + "\">首页</a></span>";
                strHtml += "... ";
            }
            if (pagaIndex > 1)
            {
                strHtml += "<span class=\"up_link\"><a href=\"?page=" + (pagaIndex - 1).ToString() + anyLink + "\">上一页</a></span>";
            }
            else
            {
                strHtml += "<span class=\"up_nolink\">上一页</span>";
            }
            int showNum = 10;
            int index = pagaIndex - (showNum / 2);
            int sumSetp = 1;
            index = (index < 1) ? 1 : index;
            while (sumSetp <= showNum)
            {
                if (index > pageCount) break;
                if (pagaIndex == index)
                {
                    strHtml += "<span class=\"currlink\">" + index.ToString() + "</span>";
                }
                else
                {
                    strHtml += "<span class=\"pagelink\"><a href=\"?page=" + index.ToString() + anyLink + "\">" + index + "</a></span>";
                }
                sumSetp++;
                index++;
            }
            if (pagaIndex < pageCount)
            {
                strHtml += "<span class=\"next_link\"><a href=\"?page=" + (pagaIndex + 1).ToString() + anyLink + "\">下一页</a></span>";
            }
            else
            {
                strHtml += "<span class=\"next_nolink\">下一页</span>";
            }
            if (pagaIndex < (pageCount - 5))
            {
                strHtml += "... ";
                strHtml += "<span class=\"lastlink\"><a href=\"?page=" + pageCount.ToString() + "" + anyLink + "\">尾页</a></span>";
            }
            return strHtml;
        }

        /// <summary>
        /// URL重写的分页链接 
        /// </summary>
        /// <param name="totalRows">总记录数</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="pagaIndex">当前页码</param>
        /// <param name="anyLink">{0} 定义页码参数</param>
        /// <param name="lang">多语言版本时，指定语言</param>
        /// <returns></returns>
        public static string RewirterPage(int totalRows, int pageCount, int pagaIndex, string anyLink, LangText lang = null)
        {
            if (lang == null)
                lang = new LangText();

            pagaIndex = (pagaIndex <= 0) ? pagaIndex = 1 : pagaIndex;
            pagaIndex = (pagaIndex >= pageCount) ? pagaIndex = pageCount : pagaIndex;
            var sb = new StringBuilder();

            if (pagaIndex > 5)
            {
                sb.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", anyLink, lang.FirstPage);
            }
            if (pagaIndex > 1)
            {
                sb.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", string.Format(anyLink, (pagaIndex - 1)), lang.LastPage);
            }
            else
            {
                sb.AppendFormat("<li class=\"disabled\"><a href=\"javascript:;\">{0}</a></li>", lang.FirstPage);
            }
            int showNum = 10;
            int index = pagaIndex - (showNum / 2);
            int sumSetp = 1;
            index = (index < 1) ? 1 : index;
            while (sumSetp <= showNum)
            {
                if (index > pageCount) break;
                if (pagaIndex == index)
                {
                    sb.AppendFormat("<li class=\"active\"><a href=\"javascript:;\">{0}</a></li>", index);
                }
                else
                {
                    sb.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", string.Format(anyLink, index), index);
                }
                sumSetp++;
                index++;
            }
            if (pagaIndex < pageCount)
            {
                sb.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", string.Format(anyLink, (pagaIndex + 1)), lang.NextPage);
            }
            else
            {
                sb.AppendFormat("<li class=\"disabled\"><a href=\"javascript:;\">{0}</a></li>", lang.NextPage);
             
            }
            if (pagaIndex < (pageCount - 5))
            {
                sb.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", string.Format(anyLink, pageCount), lang.EndPage);
            }
            return sb.ToString();
        }

        public static string MinPageTurning(int totalRows, int pageCount, int pagaIndex, string anyLink)
        {
            pagaIndex = (pagaIndex <= 0) ? pagaIndex = 1 : pagaIndex;
            pagaIndex = (pagaIndex >= pageCount) ? pagaIndex = pageCount : pagaIndex;
            anyLink = (!string.IsNullOrEmpty(anyLink)) ? ("&" + anyLink) : "";
            string strHtml = "<span class=\"firstlink\"><a href=\"?page=1" + anyLink + "\">首页</a></span>";
            if (pagaIndex > 1)
            {
                strHtml += "<span class=\"up_link\"><a href=\"?page=" + (pagaIndex - 1).ToString() + anyLink + "\">上一页</a></span>";
            }
            else
            {
                strHtml += "<span class=\"up_nolink\">上一页</span>";
            }
           
            if (pagaIndex < pageCount)
            {
                strHtml += "<span class=\"next_link\"><a href=\"?page=" + (pagaIndex + 1).ToString() + anyLink + "\">下一页</a></span>";
            }
            else
            {
                strHtml += "<span class=\"next_nolink\">下一页</span>";
            }
            strHtml += "<span class=\"lastlink\"><a href=\"?page=" + pageCount.ToString() + "" + anyLink + "\">尾页</a></span>";
            return strHtml;
        }
    }
}
