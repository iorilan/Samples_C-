using System;
using System.Web;

namespace WebCode.asp.net.HttpModule
{
    public class UrlRewriterModule : IHttpModule
    {

        public void Dispose()
        {
            //clean-up code here.
        }

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += ContextOnAuthenticateRequest;
            context.BeginRequest += ContextOnBeginRequest;
            context.AuthorizeRequest += context_AuthorizeRequest;
            context.PreRequestHandlerExecute += ContextOnPreRequestHandlerExecute;
            context.PostRequestHandlerExecute += ContextOnPostRequestHandlerExecute;
            context.EndRequest += ContextOnEndRequest;
        }

        #region before handler
        private void ContextOnPreRequestHandlerExecute(object sender, EventArgs eventArgs)
        {

        }

        private void context_AuthorizeRequest(object sender, EventArgs e)
        {

        }

        private void ContextOnBeginRequest(object sender, EventArgs eventArgs)
        {

            //test URL rewritten 
            var context = ((HttpApplication)sender).Context;

            if (context.Request.RawUrl.Contains("ExploreRedirect"))
            {
                context.RewritePath("ExploreRedirect.aspx","","SN=" + Guid.NewGuid());
            }
        }

        private void ContextOnAuthenticateRequest(object sender, EventArgs eventArgs)
        {

        }

        #endregion

        #region after handler

        private void ContextOnEndRequest(object sender, EventArgs eventArgs)
        {

        }

        private void ContextOnPostRequestHandlerExecute(object sender, EventArgs eventArgs)
        {

        }

        #endregion
    }
}
