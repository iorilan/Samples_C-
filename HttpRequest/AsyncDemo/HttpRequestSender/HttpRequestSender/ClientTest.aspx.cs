using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Net;
using System.IO;
using Imps.Services.IDCService.Utility;
using System.Threading;

namespace HttpRequestSender
{
    public partial class ClientTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnTest_Click(object sender, EventArgs e)
        {
            string strRequestUrl = txtUrl.Text.Trim();
            string strSsic = txtSSIC.Text.Trim();
            string strRequestXML = txtRequestXML.Text.Trim();

            //int success = 0;
            //int fail = 0;
            //string ret = "";
            //DateTime before = DateTime.Now;
            //for (int i = 0; i < 500; i++)
            //{
            //    AysncPostRequest(strRequestXML, strRequestUrl, 5000, (s, ex) =>
            //    {
            //        if (ex == null)
            //        {
            //            ret = s;
            //            success++;
            //        }
            //        else
            //        {
            //            fail++;
            //        }
            //    });
            //}
            //while (true)
            //{

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strRequestUrl);
                request.Method = "POST";
                byte[] data = Encoding.UTF8.GetBytes(strRequestXML);
                request.CookieContainer = new CookieContainer();
                CookieContainer cookies = request.CookieContainer; //保存cookies
                cookies.Add(request.RequestUri, new Cookie("ssic", strSsic));
                cookies.Add(request.RequestUri, new Cookie("domain", txtDomain.Text.Trim()));
                request.CookieContainer = cookies;
                request.Headers.Add("ssic", strSsic);
                request.ContentType = "text/xml";
                request.ContentLength = data.Length;
                Stream myStream = request.GetRequestStream();
                myStream.Write(data, 0, data.Length);
                myStream.Close();
                HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string content = reader.ReadToEnd();
                txtResultXML.Text = content;

               // Thread.Sleep(1000);
         //   }
            //while (success + fail < 2000)
            //{
            //    ;
            //}
            //DateTime after = DateTime.Now;
            //double total = (after - before).TotalSeconds;
            //txtResultXML.Text = "success : " + success.ToString() + "\r\n" + "failed: "

            //    + fail.ToString() + "\r\n sec: "
            //    + total;

        }

        /// <summary>
        /// 异步发送POST请求
        /// </summary>
        /// <typeparam name="strRequestXml">要发送的XML字符串</typeparam>
        /// <param name="url">url</param>
        /// <param name="OnResponseGet"></param>
        public void AysncPostRequest(string strRequestXml, string url, int timeout, Action<string, Exception> OnResponseGet)
        {
            try
            {
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] data = encoding.GetBytes(strRequestXml);

                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                myHttpWebRequest.Timeout = timeout;
                myHttpWebRequest.Method = "POST";
                myHttpWebRequest.ContentType = "application/json;charset=utf-8";
                myHttpWebRequest.ContentLength = data.Length;
                // myHttpWebRequest.Headers.Add("Authorization", strAuth);
                myHttpWebRequest.CookieContainer = new CookieContainer();
                CookieContainer cookies = myHttpWebRequest.CookieContainer; //保存cookies
                cookies.Add(myHttpWebRequest.RequestUri, new Cookie("ssic", txtSSIC.Text));
                // cookies.Add(myHttpWebRequest.RequestUri, new Cookie("domain", txtDomain.Text.Trim()));
                myHttpWebRequest.CookieContainer = cookies;
                myHttpWebRequest.Headers.Add("ssic", txtSSIC.Text);


                AsynHttpContext asynContext = new AsynHttpContext(myHttpWebRequest);
                string tranKey = TransactionManager<AsynHttpContext>.Instance.Register(asynContext);

                AysncHttpRequestHelperV2.Post(asynContext, data, new Action<AsynHttpContext, Exception>((httpContext, ex) =>
                {
                    TransactionManager<AsynHttpContext>.Instance.Unregister(tranKey);
                    if (ex != null)
                    {
                        OnResponseGet(null, ex);
                    }
                    else
                    {
                        string rspStr = Encoding.UTF8.GetString(httpContext.ResponseBytes);
                        OnResponseGet(rspStr, null);
                    }
                }));
            }
            catch (Exception ex)
            {
                OnResponseGet(null, ex);
            }
        }
    }
}