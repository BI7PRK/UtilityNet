using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;

namespace UtilityNet
{
    public class RewriterHandler : IHttpHandler, IRequiresSessionState
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {

            var obj = RewriterModule.GetExecutionPath(context);
            if (!string.IsNullOrEmpty(obj.RewritePath))
            {
                context.RewritePath(obj.RewritePath, obj.PathInfo, obj.QueryString);

                Page aspxHandler = (Page)PageParser.GetCompiledPageInstance(obj.RewritePath, context.Server.MapPath(obj.RewritePath), context);
                aspxHandler.PreRenderComplete += new EventHandler((object sender, EventArgs e) =>
                {
                    context.RewritePath(obj.RewritePath, obj.PathInfo, obj.QueryString);
                });
                aspxHandler.ProcessRequest(context);
            }
            else if (!string.IsNullOrEmpty(obj.RedirectUrl))
            {
                context.Response.Redirect(obj.RedirectUrl, true);
            }
            else if (!File.Exists(context.Server.MapPath(context.Request.Path)))
            {
               throw new HttpException(404, "Page Not Found");
            }
        }
    }
}
