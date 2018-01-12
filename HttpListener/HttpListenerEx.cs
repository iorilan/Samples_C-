using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Imps.Services.IDCService.Service
{


    public class HttpListenerEx
    {

       // private static ITracing _tracing = TracingManager.GetTracing("HttpListenerEx");

        private static int PerformenceCounter = 0;

        #region Members


        /// <summary>
        /// 当前监听HTTP请求的对象
        /// </summary>
        private HttpListener currentListener;


        /// <summary>
        /// 停止监听所有请求
        /// </summary>
        private bool StopAll = false;

        /// <summary>
        /// 接收到异步请求返回结果时，回调外部的方法
        /// </summary>
        private Func<HttpListenerContext, string> OnGetSyncRequest;
        #endregion


        #region Constructor


        /// <summary>
        /// 初始化时指定一个要监听的网络地址，以'/'结尾
        /// 例如："http://contoso.com:8080/index/".
        /// </summary>
        /// <param name="url">要监听的网络地址</param>
        public HttpListenerEx(string url)
        {
            this.currentListener = new HttpListener();
            this.currentListener.Prefixes.Add(url);
        }


        /// <summary>
        /// 初始化时指定多个地址，同时监听，地址以'/'结束
        /// 例如："http://contoso.com:8080/index/".
        /// </summary>
        /// <param name="urls">要监听的网络地址集合</param>
        public HttpListenerEx(string[] urls)
        {
            this.currentListener = new HttpListener();
            foreach (string s in urls)
            {
                this.currentListener.Prefixes.Add(s);
            }
        }


        #endregion


        #region Methods


        /// <summary>
        /// 监听HTTP请求（只处理第一次的请求，处理完之后就结束了）
        /// </summary>
        /// <param name="OnGetRequestStr">将接收到的请求字符串交给OnGetRequestStr处理</param>
        public void ListenForOnce(Func<HttpListenerContext, string> OnRequestGet)
        {
            if (!HttpListener.IsSupported)
            {
               // _tracing.ErrorFmt("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }


            ////开始监听
            this.currentListener.Start();


            ////处理请求
            HandleRequest(OnRequestGet);


            ////停止监听
            this.currentListener.Stop();
        }


        /// <summary>
        /// 监听HTTP请求，监听到之后，立刻返给客户端一个字符串
        /// </summary>
        /// <param name="OnGetRequestStr">将接收到的请求字符串交给OnGetRequestStr处理</param>
        public void StartListen(Func<HttpListenerContext, string> OnGetRequest)
        {
            if (!HttpListener.IsSupported)
            {
             //   _tracing.WarnFmt("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            ////开始监听
            this.currentListener.Start();
            // PerformenceCounter++;
          //  _tracing.InfoFmt("performence counter is {0} ", PerformenceCounter);

            ////处理请求
            while (!StopAll)
            {
                HandleRequest(OnGetRequest);
            }
        }

        /// <summary>
        /// 监听HTTP请求，监听到之后，以异步方式接收HTTP请求
        /// </summary>
        /// <param name="OnGetRequestStr">将接收到的请求字符串交给OnGetRequestStr处理</param>
        public void AsynStartListen(Func<HttpListenerContext, string> OnGetRequest)
        {
            if (!HttpListener.IsSupported)
            {
              //  _tracing.WarnFmt("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            this.OnGetSyncRequest = OnGetRequest;
            ////开始监听
            this.currentListener.Start();
            //PerformenceCounter++;
            //_tracing.InfoFmt("performence counter is {0} ", PerformenceCounter);

            ////处理请求
            //while (!StopAll)
            //{
            this.currentListener.BeginGetContext(EndGetRequest, this.currentListener);
            //}
        }

        /// <summary>
        /// 结束监听
        /// </summary>
        public void EndListen()
        {
            StopAll = true;
            this.currentListener.Abort();
        }


        #endregion


        #region Private Methods

        private void AsyncHandlerRequest(Func<HttpListenerContext, string> OnGetRequestStr, HttpListenerContext context)
        {
            if (context == null)
            {
               // _tracing.WarnFmt("error in httplisenerEx context is null.");
            }
            HttpListenerRequest request = context.Request;

            // Obtain a response object.
            HttpListenerResponse response = context.Response;
            //StreamReader sr = new StreamReader(request.InputStream);
            string outStr = OnGetRequestStr(context);
            // Construct a response.
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(outStr);
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            // You must close the output stream.
            output.Close();

           // context.Response.Close();

        }

        /// <summary>
        /// 处理请求
        /// </summary>
        /// <param name="OnGetRequestStr">将接收到的请求字符串交给OnGetRequestStr处理,处理完了返回给客户端</param>
        private void HandleRequest(Func<HttpListenerContext, string> OnGetRequestStr)
        {
            HttpListenerContext context = this.currentListener.GetContext();
            HttpListenerRequest request = context.Request;

            // Obtain a response object.
            HttpListenerResponse response = context.Response;
            //StreamReader sr = new StreamReader(request.InputStream);
            string outStr = OnGetRequestStr(context);
            // Construct a response.
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(outStr);
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            // You must close the output stream.
            output.Close();
        }



        /// <summary>
        /// 异步接收HTTP请求
        /// </summary>
        /// <param name="result"></param>
        private void EndGetRequest(IAsyncResult result)
        {

            HttpListenerContext context = null;
            HttpListener listener = null;
            try
            {
                listener = (HttpListener)result.AsyncState;
                context = listener.EndGetContext(result);
               // _tracing.InfoFmt("get new request in http listener ex!!!");

            }

            catch (Exception ex)
            {
               // _tracing.ErrorFmt(ex, "Exception in listener: {0}", ex.Message);
            }
            finally
            {
                if (listener != null && listener.IsListening)
                {
                    listener.BeginGetContext(EndGetRequest, listener);
                }
                this.AsyncHandlerRequest(OnGetSyncRequest, context);
            }
        }




        //private void HandleRequest(HttpListenerContext context)
        //{
        //    Stream s = context.Request.InputStream;
        //    StreamReader sr = new StreamReader(s, Encoding.Default);
        //    bool isScrape = context.Request.RawUrl.StartsWith("/scrape", StringComparison.OrdinalIgnoreCase);
        //    string responseStr = sr.ReadToEnd();
        //    sr.Close();
        //    Console.WriteLine("good");
        //    byte[] response = System.Text.Encoding.Default.GetBytes(responseStr);
        //    context.Response.ContentType = "text/plain"; //这里的类型随便你写.如果想返回HTML格式使用text/html
        //    context.Response.StatusCode = 200;
        //    context.Response.ContentLength64 = response.LongLength;
        //    context.Response.OutputStream.Write(response, 0, response.Length);
        //}


        #endregion
    }

}
