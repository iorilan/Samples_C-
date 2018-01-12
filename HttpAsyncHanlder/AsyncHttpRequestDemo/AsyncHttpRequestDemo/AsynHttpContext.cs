using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace AsyncHttpRequestDemo
{
    public enum AsynStatus { Begin, TimeOut, Runing, Completed }

    public class AsynHttpContext : IDisposable
    {
        private HttpWebRequest _asynRequest;
        private HttpWebResponse _asynResponse;
        private DateTime _requestTime;
        private long _execTime;
        bool isDisposable = false;
        private byte[] _rspBytes;


        public long ExecTime
        {
            get { return _execTime; }
            set { _execTime = value; }
        }
        public byte[] ResponseBytes
        {
            get { return _rspBytes; }
        }

        public HttpWebRequest AsynRequest
        {
            get { return _asynRequest; }
        }
        public HttpWebResponse AsynResponse
        {
            get { return _asynResponse; }
            set { _asynResponse = value; }
        }
        public DateTime RequestTime
        {
            get { return _requestTime; }
            set { _requestTime = value; }
        }
        private AsynHttpContext() { }
        public AsynHttpContext(HttpWebRequest req)
        {
            _asynRequest = req;
        }
        public void SetResponseBytes(byte[] bytes)
        {
            _rspBytes = bytes;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void OnTimeOut()
        {
            if (_asynRequest != null)
                _asynRequest.Abort();
            this.Dispose();
        }
        private void Dispose(bool disposing)
        {
            if (!isDisposable)
            {
                if (disposing)
                {
                    if (_asynRequest != null)
                        _asynRequest.Abort();
                    if (_asynResponse != null)
                        _asynResponse.Close();
                }
            }
            isDisposable = true;
        }
        ~AsynHttpContext()
        {
            Dispose(false);
        }
    }
}
