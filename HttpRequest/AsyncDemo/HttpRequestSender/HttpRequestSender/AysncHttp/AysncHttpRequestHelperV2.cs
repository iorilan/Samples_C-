using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Imps.Services.CommonV4;
using System.Diagnostics;
using System.Net;
using System.IO;

namespace Imps.Services.IDCService.Utility
{
    public class AysncHttpRequestHelperV2
    {
        //public static readonly ITracing _logger = TracingManager.GetTracing(typeof(AysncHttpRequestHelperV2));

        private AsynHttpContext _context;
        private byte[] buffer;
        private const int DEFAULT_LENGTH = 1024 * 512;
        private const int DEFAULT_POS_LENGTH = 1024 * 16;
        private bool notContentLength = false;
        private Action<AsynHttpContext, Exception> _action;
        private Stopwatch _watch = new Stopwatch();
        private byte[] requestBytes;

        private AysncHttpRequestHelperV2(HttpWebRequest request, Action<AsynHttpContext, Exception> callBack)
            : this(new AsynHttpContext(request), callBack)
        {
        }
        private AysncHttpRequestHelperV2(AsynHttpContext context, Action<AsynHttpContext, Exception> callBack)
        {
            this._context = context;
            this._action = callBack;
            this._watch = new Stopwatch();
        }

        public static void Get(HttpWebRequest request, Action<AsynHttpContext, Exception> callBack)
        {
            AysncHttpRequestHelperV2 proxy = new AysncHttpRequestHelperV2(request, callBack);
            proxy.SendRequest();
        }
        public static void Get(AsynHttpContext context, Action<AsynHttpContext, Exception> callBack)
        {
            AysncHttpRequestHelperV2 proxy = new AysncHttpRequestHelperV2(context, callBack);
            proxy.SendRequest();
        }

        public static void Post(HttpWebRequest request, byte[] reqBytes, Action<AsynHttpContext, Exception> callBack)
        {
            AysncHttpRequestHelperV2 proxy = new AysncHttpRequestHelperV2(request, callBack);
            proxy.SendRequest(reqBytes);
        }
        public static void Post(AsynHttpContext context, byte[] reqBytes, Action<AsynHttpContext, Exception> callBack)
        {
            AysncHttpRequestHelperV2 proxy = new AysncHttpRequestHelperV2(context, callBack);
            proxy.SendRequest(reqBytes);
        }
        public static void Post(HttpWebRequest request, string reqStr, Action<AsynHttpContext, Exception> callBack)
        {
            AysncHttpRequestHelperV2 proxy = new AysncHttpRequestHelperV2(request, callBack);
            proxy.SendRequest(Encoding.UTF8.GetBytes(reqStr));
        }
        public static void Post(AsynHttpContext context, string reqStr, Action<AsynHttpContext, Exception> callBack)
        {
            AysncHttpRequestHelperV2 proxy = new AysncHttpRequestHelperV2(context, callBack);
            proxy.SendRequest(Encoding.UTF8.GetBytes(reqStr));
        }

        /// <summary>
        /// 用于http get
        /// </summary>
        /// <param name="req">HttpWebRequest</param>
        /// <param name="action"></param>
        private void SendRequest()
        {
            try
            {
                _watch.Start();
               
                _context.RequestTime = DateTime.Now;
                IAsyncResult asyncResult = _context.AsynRequest.BeginGetResponse(RequestCompleted, _context);
            }
            catch (Exception e)
            {
                //_logger.Error(e, "send request exception.");
                EndInvoke(_context, e);
            }
        }

        /// <summary>
        /// 用于POST
        /// </summary>
        /// <param name="req">HttpWebRequest</param>
        /// <param name="reqBytes">post bytes</param>
        /// <param name="action">call back</param>
        private void SendRequest(byte[] reqBytes)
        {
            try
            {
                _watch.Start();
                requestBytes = reqBytes;
                _context.AsynRequest.Method = "POST";
                _context.RequestTime = DateTime.Now;

                IAsyncResult asyncRead = _context.AsynRequest.BeginGetRequestStream(asynGetRequestCallBack, _context.AsynRequest);
            }
            catch (Exception e)
            {
                //_logger.Error(e, "send request exception.");
                EndInvoke(_context, e);
            }
        }

        private void asynGetRequestCallBack(IAsyncResult asyncRead)
        {
            try
            {
                HttpWebRequest request = asyncRead.AsyncState as HttpWebRequest;
                Stream requestStream = request.EndGetRequestStream(asyncRead);
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();
                SendRequest();
            }
            catch (Exception e)
            {
                //_logger.Error(e, "GetRequestCallBack exception.");
                EndInvoke(_context, e);
            }
        }
        private void asynGetRequestCallBack2(IAsyncResult asyncRead)
        {
            try
            {
                HttpWebRequest request = asyncRead.AsyncState as HttpWebRequest;
                Stream requestStream = request.EndGetRequestStream(asyncRead);
                requestStream.BeginWrite(requestBytes, 0, requestBytes.Length, endStreamWrite, requestStream);
            }
            catch (Exception e)
            {
                //_logger.Error(e, "GetRequestCallBack exception.");
                EndInvoke(_context, e);
            }
        }
        private void endStreamWrite(IAsyncResult asyncWrite)
        {
            try
            {
                Stream requestStream = asyncWrite.AsyncState as Stream;
                requestStream.EndWrite(asyncWrite);
                requestStream.Close();
                SendRequest();
            }
            catch (Exception e)
            {
                //_logger.Error(e, "GetRequestCallBack exception.");
                EndInvoke(_context, e);
            }
        }

        private void RequestCompleted(IAsyncResult asyncResult)
        {
            AsynHttpContext _context = asyncResult.AsyncState as AsynHttpContext;
            try
            {
                if (asyncResult == null) return;
                _context.AsynResponse = (HttpWebResponse)_context.AsynRequest.EndGetResponse(asyncResult);
            }
            catch (WebException e)
            {
                //_logger.Error(e, "RequestCompleted WebException.");
                _context.AsynResponse = (HttpWebResponse)e.Response;
                if (_context.AsynResponse == null)
                {
                    EndInvoke(_context, e);
                    return;
                }
            }
            catch (Exception e)
            {
                //_logger.Error(e, "RequestCompleted exception.");
                EndInvoke(_context, e);
                return;
            }
            try
            {
                AsynStream stream = new AsynStream(_context.AsynResponse.GetResponseStream());
                stream.Offset = 0;
                long length = _context.AsynResponse.ContentLength;
                if (length < 0)
                {
                    length = DEFAULT_LENGTH;
                    notContentLength = true;
                }
                buffer = new byte[length];
                stream.UnreadLength = buffer.Length;
                IAsyncResult asyncRead = stream.NetStream.BeginRead(buffer, 0, stream.UnreadLength, new AsyncCallback(ReadCallBack), stream);
            }
            catch (Exception e)
            {
                //_logger.Error(e, "BeginRead exception.");
                EndInvoke(_context, e);
            }
        }

        private void ReadCallBack(IAsyncResult asyncResult)
        {
            try
            {
                AsynStream stream = asyncResult.AsyncState as AsynStream;
                int read = 0;
                if (stream.UnreadLength > 0)
                    read = stream.NetStream.EndRead(asyncResult);
                if (read > 0)
                {
                    stream.Offset += read;
                    stream.UnreadLength -= read;
                    stream.Count++;
                    if (notContentLength && stream.Offset >= buffer.Length)
                    {
                        Array.Resize<byte>(ref buffer, stream.Offset + DEFAULT_POS_LENGTH);
                        stream.UnreadLength = DEFAULT_POS_LENGTH;
                    }
                    IAsyncResult asyncRead = stream.NetStream.BeginRead(buffer, stream.Offset, stream.UnreadLength, new AsyncCallback(ReadCallBack), stream);
                }
                else
                {
                    _watch.Stop();
                    stream.NetStream.Dispose();
                    if (buffer.Length != stream.Offset)
                        Array.Resize<byte>(ref buffer, stream.Offset);
                    _context.ExecTime = _watch.ElapsedMilliseconds;
                    _context.SetResponseBytes(buffer);
                    _action.Invoke(_context, null);
                }
            }
            catch (Exception e)
            {
                //_logger.Error(e, "ReadCallBack exception.");
                EndInvoke(_context, e);
            }
        }

        private void EndInvoke(AsynHttpContext _context, Exception e)
        {
            try
            {
                _watch.Stop();
                _context.ExecTime = _watch.ElapsedMilliseconds;
                _action.Invoke(_context, e);
            }
            catch (Exception ex)
            {
                //_logger.Error(ex, "EndInvoke exception.");
                _action.Invoke(null, ex);
            }
        }

    }
}
