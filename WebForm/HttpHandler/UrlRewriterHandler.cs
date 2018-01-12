using System;
using System.Collections.Generic;
using System.Web;

namespace WebCode.asp.net.HttpHandler
{
    public class UrlRewriterHandler : IHttpAsyncHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Url.OriginalString.Contains("ExploreRedirect.aspx"))
            {
                var newUrl = context.Request.Url.OriginalString.Replace("ExploreRedirect.aspx",
                                                                        "ExploreRedirect.aspx?SN=" + Guid.NewGuid());
                context.Server.Transfer(newUrl);
            }
            
        }

        private Action<HttpContext> _process;
        public bool IsReusable { get; private set; }

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            _process = ProcessRequest;
            return _process.BeginInvoke(context, cb, extraData);
        }

        public void EndProcessRequest(IAsyncResult result)
        {
            _process.EndInvoke(result);
        }
    }
}
