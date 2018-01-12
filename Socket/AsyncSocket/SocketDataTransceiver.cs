using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Diagnostics;
using System.Collections.Generic;
using Imps.Services.CommonV4;

namespace Imps.Fmc.ESMSAdapter
{
    public delegate void SocketSentCallBackDelegate(int sentSize, Exception sentException);
	public class SocketDataTransceiver
	{
		#region Private fields
        private static ITracing _tracing = TracingManager.GetTracing(typeof(SocketDataTransceiver));
        private int _recvBufferSize;
		private object _syncReceive = new object();
		private object _syncRoot = new object();
		private Socket _socket;
		private byte[] _recvBuffer;
        private SocketDataReceivedEventArgs _evenArgs;

        private DateTime _lastActiveTime = DateTime.Now;

        private DateTime _lastDataReceivedTime = DateTime.Now;

        public DateTime LastDataReceivedTime
        {
            get { return _lastDataReceivedTime; }
            set { _lastDataReceivedTime = value; }
        }

        public DateTime LastActiveTime
        {
            get { return _lastActiveTime; }
        }


		#endregion


		#region Public properties

		public object SyncRoot
		{
			get { return _syncRoot; }
		}

		public Socket InnerSocket
		{
			get { return _socket; }
		}

		public bool Connected
		{
			get
			{
				Socket s = _socket;
				if (s == null)
					return false;
				return s.Connected;
			}
		}

		#endregion


		#region Events

		public event SocketDataReceivedEventHandler DataReceived;
		public event EventHandler Disconnected;

		#endregion


		#region Constructor


        public SocketDataTransceiver(Socket socket):this(socket,8192)
        {         
        }


        public SocketDataTransceiver(Socket socket, int recvBufferSize)
		{
            if (socket == null)
                throw new ArgumentNullException("socket");
			_socket = socket;
            _recvBufferSize = recvBufferSize;
            _recvBuffer = new byte[_recvBufferSize];
            _evenArgs = new SocketDataReceivedEventArgs(_recvBuffer);
		}

		#endregion


		#region Public methods


		public void BeginReceive()
		{
			Socket s = _socket;
            if (s != null)
            {
                try
                {
                    s.BeginReceive(_recvBuffer, 0, _recvBufferSize, SocketFlags.None, new AsyncCallback(BeginReceiveCallback), _socket);
                }
                catch (SocketException ex)
                {
                    Socket sEx = _socket;
                    _tracing.ErrorFmt(ex, "SocketDataTransceiver接收数据发生异常，SocketErrorCode：{0}，RemoteEndPoint：{1}，SocketDataTransceiver将被迫关闭！", ex.SocketErrorCode, sEx == null ? "null" : ((IPEndPoint)(_socket.RemoteEndPoint)).ToString());
                    Close();
                }
                catch (ObjectDisposedException ex)
                {
                    _tracing.Error(ex, "因为socket已释放，SocketDataTransceiver接收数据发生异常，SocketDataTransceiver将被迫关闭！");
                    Close();
                }
                catch (Exception ex)
                {
                    _tracing.Error(ex, "SocketDataTransceiver接收数据发生异常，SocketDataTransceiver将被迫关闭！");
                    Close();
                }
            }
            
		}

		public void Receive()
		{
			Socket s = _socket;
			if (s == null)
				return;

            int size = s.Receive(_recvBuffer, 0, _recvBufferSize, SocketFlags.None);
			if (size < 1) {
				return;
			}
            _lastActiveTime = DateTime.Now;
            _lastDataReceivedTime = DateTime.Now;
			_evenArgs.SetSize(size);

			if (DataReceived != null)
				DataReceived(this, _evenArgs);
		}

        public void BeginSend(byte[] buffer,SocketSentCallBackDelegate sentCallBack)
        {
            Socket s = _socket;
            if (s != null)
            {
                try
                {
                    s.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(BeginSendCallback), new SocketStateContext(s, sentCallBack));
                }
                catch (Exception ex)
                {
                    try
                    {
                        sentCallBack(0, ex);
                    }
                    catch { }
                }
            }
        }

		public int Send(byte[] buffer)
		{
			if (buffer == null || buffer.Length < 1)
				return 0;

			Socket s = _socket;
			if (s == null)
				throw new ObjectDisposedException("_socket","when send data occurred the exception! (socket is null!)");
   
			int sendCount = s.Send(buffer, 0, buffer.Length, SocketFlags.None);
            _lastActiveTime = DateTime.Now;
            return sendCount;
		}

		public void Close()
		{
            if (InnerClose())
            {
                OnDisconnected();
            }
		}


		/// <summary>
        /// close connection without raise event
		/// </summary>
		/// <returns>true表示本次关闭成功，false表示之前已经关闭</returns>
		public bool InnerClose()
		{
			Socket s = _socket;
			if (s == null)
				return false;
			lock (_syncRoot) {
				if (_socket == null)
					return false;
				_socket = null;
			}
			s.Close();
            return true;
		}

		#endregion


		#region Private methods

        private void BeginReceiveCallback(IAsyncResult result)
        {
            Socket s = result.AsyncState as Socket;
            int size;
            try
            {
                if (!s.Connected)
                    return;

                size = s.EndReceive(result);
                if (size < 1)
                {
                   // Close();
                    lock (_syncReceive)
                    {
                        if (_socket != null)
                            BeginReceive();
                    }
                }
                else
                {
                    _lastActiveTime = DateTime.Now;
                    _lastDataReceivedTime = DateTime.Now;
                    _evenArgs.SetSize(size);

                    lock (_syncReceive)
                    {
                        if (DataReceived != null)
                            DataReceived(this, _evenArgs);
                        if (_socket != null)
                            BeginReceive();
                    }
                }
                
            }
            catch (SocketException ex)
            {
                Socket sEx = _socket;
                if (sEx != null)
                {
                    _tracing.ErrorFmt(ex, "SocketDataTransceiver接收数据发生异常，SocketErrorCode：{0}，RemoteEndPoint：{1}，SocketDataTransceiver将被迫关闭！", ex.SocketErrorCode, sEx == null ? "null" : ((IPEndPoint)(sEx.RemoteEndPoint)).ToString());
                }
                else
                {
                    _tracing.WarnFmt(ex, "SocketDataTransceiver接收数据发生异常，SocketErrorCode：{0}，SocketDataTransceiver将被迫关闭！", ex.SocketErrorCode);
             
                }
                Close();
            }
            catch (ObjectDisposedException ex)
            {
                _tracing.Error(ex, "因为socket已释放，SocketDataTransceiver接收数据发生异常，SocketDataTransceiver将被迫关闭！");
                Close();
            }
            catch (Exception ex)
            {
                _tracing.Error(ex, "SocketDataTransceiver接收数据发生异常，SocketDataTransceiver将被迫关闭！");
                Close();
            }
        }

        private void BeginSendCallback(IAsyncResult result)
        {
            SocketStateContext context = result.AsyncState as SocketStateContext;
            Socket s = context.WorkSocket;
            int size;
            try
            {
                if (!s.Connected)
                {
                    context.SentCallBack(0,new ObjectDisposedException("WorkSocket"));
                    return;
                }
                size = s.EndSend(result);
               
            }
            catch (Exception ex)
            {
                context.SentCallBack(0, ex);
                return;
            }
            try
            {
                context.SentCallBack(size, null);
            }
            catch { };
            
        }
		private void OnDisconnected()
		{
			if (Disconnected != null)
				Disconnected(this, EventArgs.Empty);
		}

		#endregion
	}

    public class SocketStateContext
    {
        private SocketSentCallBackDelegate sentCallBack;

        public SocketSentCallBackDelegate SentCallBack
        {
            get { return sentCallBack; }
            set { sentCallBack = value; }
        }

        private Socket workSocket;

        public Socket WorkSocket
        {
            get { return workSocket; }
            set { workSocket = value; }
        }

        public SocketStateContext(Socket workSocket, SocketSentCallBackDelegate sentCallBack)
        {
            this.workSocket = workSocket;
            this.sentCallBack = sentCallBack;
        }
    }

}
