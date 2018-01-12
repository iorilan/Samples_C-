using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using NEC.ESBU.Ticketing.Client.ValidatorBase.Config;
using NEC.ESBU.Ticketing.Client.ValidatorBase.Helpers;
using NEC.ESBU.Ticketing.Client.ValidatorBase.ValidatorCommands;

namespace NEC.ESBU.Ticketing.Client.ValidatorBase.Communicators
{
    /// <summary>
    /// Listen the command from 'GateController' 
    /// entry/exit scannable : enable the scanner or no
    /// entry/exit : allow enter or no
    /// </summary>
    public class GateHTTPListener
    {
        #region single instance

        private GateHTTPListener()
        {
            currentListener = new HttpListener();
            currentListener.Prefixes.Add(ValidatorEnv.DeviceControllerHTTPListen);
        }
        static GateHTTPListener() { }
        private static GateHTTPListener _instance = new GateHTTPListener();
        public static GateHTTPListener Instance { get { return _instance; } }

        #endregion

        private ILog _log = LogManager.GetLogger(typeof(GateHTTPListener));
        private HttpListener currentListener;

        private bool StopAll;

        public void ListenNonBlockIO()
        {
            Net45Task.Run(() =>
            {

                if (!HttpListener.IsSupported)
                {
                    _log.Info("OS not support HTTP Listener");
                    return;
                }

                currentListener.Start();
                _log.Info("HTTP Listener started..");

                while (true)
                {
                    try
                    {
                        Listen();
                        Thread.Sleep(5000);
                    }
                    catch (Exception ex)
                    {
                        _log.Error(ex);
                    }
                }
            });
        }

        private void Listen()
        {
            var context = currentListener.GetContext();
            var request = context.Request;

            var response = context.Response;
            byte[] buffer = Encoding.UTF8.GetBytes("{status:200}");

            response.ContentLength64 = buffer.Length;
            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);

            output.Close();

            //_log.Debug("############## GATE COMMAND LISTENER START ##############");
            ParseCommand(request);
           // _log.Debug("############## GATE COMMAND LISTENER END ##############");
        }

        private void ParseCommand(HttpListenerRequest request)
        {

            var available = true;

            var entryScannble = true;
            var entryable = true;
            var exitScannable = true;
            var exitable = true;
            try
            {
                var queryStrs = request.QueryString;
                foreach (var key in queryStrs.AllKeys)
                {
                    // _log.Info(string.Format("key: {0} value:{1}", key, queryStrs[key]));

                    var lowerCase = key.ToLowerInvariant();
                    switch (lowerCase)
                    {
                        case "available":
                            available = bool.Parse(queryStrs[key]);
                            break;
                        case "entry":
                            entryable = bool.Parse(queryStrs[key]);
                            break;
                        case "entryscan":
                            entryScannble = bool.Parse(queryStrs[key]);
                            break;
                        case "exit":
                            exitable = bool.Parse(queryStrs[key]);
                            break;
                        case "exitscan":
                            exitScannable = bool.Parse(queryStrs[key]);
                            break;
                        default:
                            // do nothing
                            break;
                    }
                }
                if (!available)
                {
                    entryScannble = false;
                    entryable = false;
                    exitable = false;
                    exitScannable = false;
                }

                new ValidatorGateCommand(entryable, entryScannble,"").Execute();
                //_log.Info(string.Format("entry gate : AlwaysAllowEntry: [{0}] scannable:[{1}]", GateStatus.Instance.AlwaysAllowEntry, GateStatus.Instance.Scannable));

                // UDP the exit commands to Exit client 
                var jsonObj = new ValidatorGateCommand(exitable, exitScannable, ValidatorGateCommand.FORWARDING_MSGTYPE);

                var json = JsonConvert.SerializeObject(jsonObj);
                var sendingPort = ValidatorEnv.UDPEntrySendingPort; // always from entry to exit
                ValidatorCommunicator.Instance.SendCommandAsync(json, sendingPort);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }

    }
}
