using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Communicators
{
    /// <summary>
    /// the two validators communicate using JSON format and UDP protoco
    /// </summary>
    public class ValidatorCommunicator
    {
        #region single instance
        private ValidatorCommunicator() { }
        static ValidatorCommunicator() { }
        private static ValidatorCommunicator _instance = new ValidatorCommunicator();
        public static ValidatorCommunicator Instance { get { return _instance; } }

        #endregion

        private ILog _log = LogManager.GetLogger(typeof(ValidatorCommunicator));
        private const string LocalIP = "127.0.0.1";

        public void ListenNonBlock(int port)
        {
            Net45Task.Run(() =>
            {
                var done = false;
                var listener = new UdpClient(port);
                var groupEP = new IPEndPoint(IPAddress.Any, port);
                string received_data;
                byte[] receive_byte_array;

                _log.Error("############Service started###########");
                while (true)
                {
                    try
                    {
                        receive_byte_array = listener.Receive(ref groupEP);
                        
                        received_data = Encoding.UTF8.GetString(receive_byte_array, 0, receive_byte_array.Length);

                        ParseJSONCommand(received_data);
                        
                    }
                    catch (Exception e)
                    {
                        _log.Error(e);
                        Console.WriteLine(e.ToString());
                    }
                }
            });
        }

        private void ParseJSONCommand(string commandText)
        {
            ValidatorGateCommand gateCmd;
            if (ValidatorGateCommand.TryParse(commandText, out gateCmd))
            {
                gateCmd.Execute();
                return;
            }

            TicketForwardingCommand ticketCmd;
            if (TicketForwardingCommand.TryParse(commandText, out ticketCmd))
            {
                ticketCmd.Execute();
            }

            ValidatorMessageCommand messageCommand;
            if (ValidatorMessageCommand.TryParse(commandText, out messageCommand))
            {
                messageCommand.Execute();
            }
        }


        public void SendCommandAsync(string commandText, int sendingPort)
        {
            Net45Task.Run(() =>
            {
                try
                {
                    var sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    var sending_end_point = new IPEndPoint(IPAddress.Parse(LocalIP), sendingPort);
                    var send_buffer = Encoding.UTF8.GetBytes(commandText);
                    sending_socket.SendTo(send_buffer, sending_end_point);
                    // _log.Info("[Validator COMMAND Sender] : " + commandText);
                }
                catch (Exception ex)
                {
                    _log.Error(ex);
                }
            });
        }
    }
}
