using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using NEC.ESBU.Ticketing.Client.ValidatorBase.Config;
using NEC.ESBU.Ticketing.Client.ValidatorBase.Helpers;

namespace NEC.ESBU.Ticketing.Client.ValidatorBase.Communicators
{
    public class COMPortListener
    {
        private static ILog logger = LogManager.GetLogger(typeof(COMPortListener));

        public static string LastScanned { get; private set; }

        #region single instance
        private COMPortListener()
        {
        }
        static COMPortListener()
        {

        }
        private static COMPortListener _instance = new COMPortListener();
        public static COMPortListener Instance { get { return _instance; } }

        #endregion



        public Action<string> OnDataReceived;


        public void SerialPortListenAsync()
        {
            if (OnDataReceived == null)
            {
                throw new InvalidOperationException("must set callback [OnDataReceived] first.");
            }

            Net45Task.Run(() =>
            {
                logger.Info("Starting Open Virtual COM");

                var mySerialPort = new SerialPort(ValidatorEnv.COM_PORT);

                mySerialPort.BaudRate = 115200;
                mySerialPort.Parity = Parity.None;
                mySerialPort.StopBits = StopBits.One;
                mySerialPort.DataBits = 8;
                mySerialPort.Handshake = Handshake.None;
                mySerialPort.RtsEnable = true;
                mySerialPort.DtrEnable = true;

                mySerialPort.ReadTimeout = 500;

                mySerialPort.ErrorReceived += (sender, args) =>
                {
                    logger.Error("######error");
                    logger.Error(args.EventType);
                };

                // keep trying until open the com
                var failOpen = true;
                while (failOpen)
                {
                    try
                    {
                        
                        mySerialPort.Open();
                        Thread.Sleep(1000);
                        failOpen = false;
                    }
                    catch
                    {
                        logger.Error("### COM OPEN FAILED ###  - TRYING AGAIN .");
                    }
                }
                
                logger.Info("####COM PORT opened...");
                while (true)
                {
                    try
                    {
                        string message = mySerialPort.ReadLine();

                        logger.Debug("################  COM LISTENER START  #############");
                        logger.Debug(message);
                        logger.Debug("################  COM LISTENER END  #############");

                        LastScanned = message;

                        ProgramIdleChecker.StubActivity();
                        if (!FrmScanning.SingleInstance.UpdateMsgWorker.Busy())
                        {
                            FrmScanning.SingleInstance.UpdateMsgWorker.RunAsync();
                        }
                        OnDataReceived(message);
                        

                        Thread.Sleep(500);
                    }
                    catch (TimeoutException ex)
                    {
                        //COM is idle
                    }
                }

            });

        }
    }
}
