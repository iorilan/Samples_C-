using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using log4net;
using NEC.ESBU.Ticketing.Client.ValidatorBase.CardCommands;
using NEC.ESBU.Ticketing.Client.ValidatorBase.Config;
using NEC.ESBU.Ticketing.Client.ValidatorBase.Helpers;

namespace NEC.ESBU.Ticketing.Client.ValidatorBase.Communicators
{
    public class CEPASCommunicator
    {
        #region single instance
        private CEPASCommunicator() {
            ListeningPort = CEPASValidatorEnv.ListeningPort;
            SendingPort = CEPASValidatorEnv.SendingPort;
            SendingIp = CEPASValidatorEnv.SendingIp;
        }
        static CEPASCommunicator()
        {
            
        }
        private static CEPASCommunicator _instance = new CEPASCommunicator();
        public static CEPASCommunicator Instance { get { return _instance; } }

        #endregion

        private ILog _log = LogManager.GetLogger(typeof(CEPASCommunicator));
        private int ListeningPort { get; set; }
        private int SendingPort { get; set; }
        private string SendingIp { get; set; }

        public Action<string> DebuggingHook { get; set; }

        public void ListenAsync()
        {
            Net45Task.Run(() =>
            {
                var done = false;
                var listener = new UdpClient(ListeningPort);
                var groupEP = new IPEndPoint(IPAddress.Any, ListeningPort);
                string received_data;
                byte[] receive_byte_array;

                _log.Error("############Service started###########");
                while (true)
                {
                    try
                    {
                        receive_byte_array = listener.Receive(ref groupEP);
                        received_data = Encoding.UTF8.GetString(receive_byte_array, 0, receive_byte_array.Length);
                        //_log.Debug("############## CARD COMMAND  ####################");
                        //_log.Debug(received_data);
                        //_log.Debug("################ CARD COMMAND  ####################");

                        ParseCommand(received_data);
                    }
                    catch (Exception e)
                    {
                        _log.Error(e);
                    }
                }
            });
        }

        private void ParseCommand(string commandText)
        {
            if (DebuggingHook != null)
            {
                DebuggingHook(commandText);
                return;
            }

            //_log.Debug("[Card Command parsing ] step 1");
            commandText = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" + commandText;
           // _log.Debug("[Card command parsing] before load");
            //_log.Debug(commandText);

            var doc = new XmlDocument();
            doc.LoadXml(commandText);

            if (HeartbeatCommand.TryParseNExecute(doc))
            {
                return;
            }

            TappingCardCommand tappingCardCommand;
            if (TappingCardCommand.TryParse(doc, out tappingCardCommand))
            {
                ProgramIdleChecker.StubActivity();
                if (!FrmScanning.SingleInstance.UpdateMsgWorker.Busy())
                {
                    FrmScanning.SingleInstance.UpdateMsgWorker.RunAsync();
                }

                tappingCardCommand.Execute();
                return;
            }

            AfterdebitCommand afterdebitCommand;
            if (AfterdebitCommand.TryParse(doc, out afterdebitCommand))
            {
                ProgramIdleChecker.StubActivity();
                afterdebitCommand.Execute();
                return;
            }

            _log.Error("########Fatal Error#########");
            throw new Exception("Unexpected command received:" + commandText);
        }

        public void SendCommandAsync(string xmlCmd)
        {
            Net45Task.Run(() =>
            {
                try
                {
                    var sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    var sending_end_point = new IPEndPoint(IPAddress.Parse(SendingIp), SendingPort);
                    var send_buffer = Encoding.UTF8.GetBytes(xmlCmd);
                    sending_socket.SendTo(send_buffer, sending_end_point);
                    _log.Info("[COMMAND SENT] : " + xmlCmd);
                }
                catch (Exception ex)
                {
                    _log.Error(ex);
                }
            });
            
        }
    }
}
