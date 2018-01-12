using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyHandler
{
    public class TestHandler : HttpAsyncHandler
    {
        public override void BeginProcess(System.Web.HttpContext context)
        {
            try
            {
                StreamReader sr = new StreamReader(context.Request.InputStream);

                string reqStr = sr.ReadToEnd();
                context.Response.Write("get your input : " + reqStr + " at " + DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                context.Response.Write("exception eccurs ex info : " + ex.Message);
            }
            finally
            {
                EndProcess();
            }

        }
    }
}
