using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SocketClient
{
    class SocketClient
    {
        private static byte[] result = new byte[1024];
        private Action<string> CallBack;
        public SocketClient(Action<string> callback)
        {
            this.CallBack = callback;
        }

        public void SendMsg()
        {
            //设定服务器IP地址  
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientSocket.Connect(new IPEndPoint(ip, 8885)); //配置服务器IP与端口  
            }
            catch
            {
                return;
            }
            //通过clientSocket接收数据
            int receiveLength = clientSocket.Receive(result);
            this.CallBack(Encoding.ASCII.GetString(result, 0, receiveLength));
            //通过 clientSocket 发送数据  
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    Thread.Sleep(1000);    //等待1秒钟  
                    string sendMessage = "client send Message Hellp" + DateTime.Now;
                    clientSocket.Send(Encoding.ASCII.GetBytes(sendMessage));
                    //Console.WriteLine("向服务器发送消息：{0}" + sendMessage);
                }
                catch
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    break;
                }
            }
        }
    }
}
