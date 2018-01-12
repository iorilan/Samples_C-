using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Imps.Services.CommonV4;

namespace Imps.Fmc.ESMSAdapter
{

    public delegate void SocketAcceptedHandler(AsynSocketListener listener,Socket acceptSocket);
   
    public class AsynSocketListener
    {
        private static ITracing _tracing = TracingManager.GetTracing(typeof(AsynSocketListener));

        private SocketAsyncEventArgs accpetEventArgs;

        public event SocketAcceptedHandler SocketAccepted;

        private IPEndPoint localEP;
        /// <summary>
        /// 获取监听的网络端点
        /// </summary>
        public IPEndPoint LocalEP
        {
            get { return localEP; }
        }

        private bool isListening;
        /// <summary>
        /// 获取是否在监听
        /// </summary>
        public bool IsListening
        {
            get { return isListening; }
        }

        private Socket listener;
        /// <summary>
        ///  获取Listener的Socket元数据
        /// </summary>
        public Socket Listener
        {
            get { return listener; }
        }

        /// <summary>
        /// socket异步监听器
        /// </summary>
        /// <param name="port">监听端口</param>
        public AsynSocketListener(int port)
            : this(new IPEndPoint(IPAddress.Any, port))
        { 
        }

        /// <summary>
        /// socket异步监听器
        /// </summary>
        /// <param name="localEP">本地终端</param>
        public AsynSocketListener(IPEndPoint localEP)
        {
            this.localEP = localEP;
        }

        /// <summary>
        /// 开始监听
        /// </summary>
        /// <param name="backlog"></param>
        public void StartListen(int backlog)
        {
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEP);
            listener.Listen(backlog);
            isListening = true;
            accpetEventArgs = new SocketAsyncEventArgs();
            accpetEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(AccpetEventArgs_Completed);
            StartAccept();
            
        }


        private void AccpetEventArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }
       
        /// <summary>
        /// 启动接受客户端连接
        /// </summary>
        private void StartAccept()
        {
            if (isListening)
            {
                bool asyAccept = false;
                try
                {
                    asyAccept = listener.AcceptAsync(accpetEventArgs);

                }
                catch (SocketException ex)
                {
                    _tracing.ErrorFmt(ex, "AcceptAsync发生Sokcet异常，ErrorCode：{0}，StartAccept被迫中止", ex.ErrorCode);
                    return;
                }
                catch (ObjectDisposedException ex)
                {
                    _tracing.Error(ex, "AcceptAsync发生异常，监听socket已释放，StartAccept被迫中止");
                    return;
                }
                catch (Exception ex)
                {
                    _tracing.Error(ex, "AcceptAsync发生异常，StartAccept被迫中止");
                    return;
                }

                if (!asyAccept)
                {
                    ProcessAccept(accpetEventArgs);
                }
            }
        }

        /// <summary>
        ///  处理新连接事件
        /// </summary>
        /// <param name="e">异步套接字操作对象</param>
        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            Socket acceptSocket = e.AcceptSocket;
            accpetEventArgs.AcceptSocket = null;
            if (e.SocketError != SocketError.Success)
            {    
                StartAccept();
                if (acceptSocket != null)
                {
                    acceptSocket.Close();
                }
                return;
            }
            if (SocketAccepted != null)
            {
                SocketAccepted(this, acceptSocket);
            }
            StartAccept();
        }
       
        /// <summary>
        /// 停止监听
        /// </summary>
        public void StopListen()
        {
            if (isListening)
            {
                accpetEventArgs.Completed -= new EventHandler<SocketAsyncEventArgs>(AccpetEventArgs_Completed);
                if (listener != null)
                {
                    try
                    {
                        listener.Shutdown(SocketShutdown.Both);
                    }
                    catch (Exception ex)
                    {

                    };
                    isListening = false;
                    listener.Close();
                    listener = null;
                }
            }
        }
    }
}
