using System;

namespace DesignPyttern.Proxy
{
    public interface ISend
    {
        void Send(object data);
    }

    public class ReqSend : ISend
    {
        public void Send(object data)
        {
            Console.WriteLine("send data in reqSender data: {0}", data);
        }
    }

    public class SendProxy : ISend
    {
        ReqSend _reqSend;
        public void Send(object data)
        {
            _reqSend = new ReqSend();
            _reqSend.Send(data);
        }
    }

}
