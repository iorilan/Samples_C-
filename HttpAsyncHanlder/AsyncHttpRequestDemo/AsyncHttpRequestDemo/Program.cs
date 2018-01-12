using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;

namespace AsyncHttpRequestDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Action<string, Exception> OnResponseGet = new Action<string, Exception>((s, ex) =>
            {
                if (ex != null)
                {
                    string exInfo = ex.Message;
                }
                string ret = s;
            });
            try
            {
                string strRequestXml = "hello";
                int timeout = 15000;
                string contentType = "text/xml";
                string url = "http://192.168.110.191:8092/getcontentinfo/";
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] data = encoding.GetBytes(strRequestXml);

                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                myHttpWebRequest.Timeout = timeout;
                myHttpWebRequest.Method = "POST";
                myHttpWebRequest.ContentType = contentType;
                myHttpWebRequest.ContentLength = data.Length;

                //if (headers != null && headers.Count > 0)
                //{
                //    foreach (var eachheader in headers)
                //    {
                //        myHttpWebRequest.Headers.Add(eachheader.Key, eachheader.Value);
                //    }
                //}

             
                
                AsynHttpContext asynContext = new AsynHttpContext(myHttpWebRequest);
                // string tranKey = TransactionManager<AsynHttpContext>.Instance.Register(asynContext);

                AysncHttpRequestHelperV2.Post(asynContext, data, new Action<AsynHttpContext, Exception>((httpContext, ex) =>
                {
                    try
                    {
                        //       TransactionManager<AsynHttpContext>.Instance.Unregister(tranKey);


                        if (ex != null)
                        {
                            //    _tracing.Error(ex, "Exception Occurs On AysncHttpRequestHelperV2 Post Request.");
                            OnResponseGet(null, ex);
                        }
                        else
                        {
                            string rspStr = Encoding.UTF8.GetString(httpContext.ResponseBytes);
                            //  _tracing.InfoFmt("Aysnc Get Response Content : {0}", rspStr);
                            OnResponseGet(rspStr, null);
                        }
                    }
                    catch (Exception ex2)
                    {
                        //  _tracing.ErrorFmt(ex2, "");
                        OnResponseGet(null, ex2);
                    }
                }));
            }

            catch (Exception ex)
            {
                // _tracing.Error(ex, "Exception Occurs On Aysnc Post Request.");
                OnResponseGet(null, ex);
            }

            Thread.Sleep(5000);
        }
    }
}
