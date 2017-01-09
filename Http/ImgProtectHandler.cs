using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using UtilityNet.FileIO;

namespace UtilityNet.Http
{
    /// <summary>
    /// 图片防盗链
    /// <httpHandlers>
    /// <add verb="*" path="*.jpg,*.jpeg,*.gif,*.png,*.bmp" type="UtilityNet.ImgProtectHandler"/>
    /// </httpHandlers>
    /// </summary>
    public class ImgProtectHandler : IHttpHandler, IRequiresSessionState
    {
        public bool IsReusable
        {
            get { return true; }
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            string path = context.Request.PhysicalPath;
            string serverHost = context.Request.Url.Host;
            Uri refUrl = context.Request.UrlReferrer;
            if (refUrl == null || refUrl.Host.ToLower() != serverHost.ToLower())
            {
                var wimg = VirtualPath.GetAbsolutelyPath("/urlwrning.png");
                if (!File.Exists(wimg))
                {
                    Bitmap bmp = new Bitmap(146, 146);
                    var g = Graphics.FromImage(bmp);
                    g.Clear(Color.Black);
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    var title = "不允许引用图片";
                    var big = new Font("微软雅黑", 12F);
                    var ts = g.MeasureString(title, big);
                    g.DrawString(title, big, new SolidBrush(Color.White), new PointF(bmp.Width / 2F - ts.Width / 2F, bmp.Height / 2F - ts.Height / 2F));
                    var small = new Font("宋体", 9F);
                    var copy = g.MeasureString(serverHost, small);
                    g.DrawString(serverHost, small, new SolidBrush(Color.FromArgb(115, 52, 43)), new PointF(bmp.Width - copy.Width - 5, bmp.Height - copy.Height - 5));
                    g.Dispose();
                    bmp.Save(wimg, ImageFormat.Png);
                    bmp.Dispose();
                }

                context.Response.WriteFile(wimg);
            }
            else
            {
                context.Response.WriteFile(path);
            }
        }
    }
}
