using System;
using System.Collections.Generic;
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
    public partial class MainPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            string url = txtAddress.Text;
            string postData = txtContent.Text;
            string strAuth = txtAuth.Text.Trim();
            string strResult = "";

            DateTime before = DateTime.Now;

            for (int j = 0; j < 10000; j++)
            {


                if (rbtnSendType.SelectedValue == "2")
                {
                    Action<string, Exception> callback = new Action<string, Exception>((result, ex) =>
                    {
                        if (ex != null)
                        {
                            strResult = ex.Message;
                        }
                        else
                        {
                            strResult = result;
                        }
                    });
                    AysncPostRequest(postData, url, strAuth, 5000, callback);
                    for (int i = 0; i < 10000; i++)
                    {
                        if (string.IsNullOrEmpty(strResult))
                        {
                            Thread.Sleep(50);
                        }
                        else
                        {
                            txtResult.Text += strResult + "\r\n";
                            break;
                        }
                    }
                }
                else
                {
                    SendHttpRequest(postData, url);
                }
            }
        }

        public void SendHttpRequest(string postData, string url)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] data = encoding.GetBytes(postData);
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);

            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.ContentLength = data.Length;
            myRequest.Headers.Add("Authorization", txtAuth.Text);
            Stream newStream = myRequest.GetRequestStream();


            newStream.Write(data, 0, data.Length);
            newStream.Close();

            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            txtResult.Text = reader.ReadToEnd();
            reader.Close();
        }

        /// <summary>
        /// 异步发送POST请求
        /// </summary>
        /// <typeparam name="strRequestXml">要发送的XML字符串</typeparam>
        /// <param name="url">url</param>
        /// <param name="OnResponseGet"></param>
        public static void AysncPostRequest(string strRequestXml, string url, string strAuth, int timeout, Action<string, Exception> OnResponseGet)
        {
            try
            {
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] data = encoding.GetBytes(strRequestXml);

                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                myHttpWebRequest.Timeout = timeout;
                myHttpWebRequest.Method = "POST";
                myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                myHttpWebRequest.ContentLength = data.Length;
                myHttpWebRequest.Headers.Add("Authorization", strAuth);

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