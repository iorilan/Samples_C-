using System;
using System.IO;
using System.Media;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace Communicators
{
    public class GateAppClient
    {
        #region single instance
        private GateAppClient() { }
        static GateAppClient() { }
        private static GateAppClient _instance = new GateAppClient();
        public static GateAppClient Instance { get { return _instance; } }

        #endregion

        /// <summary>
        /// the url to open gate or show LED
        /// </summary>
        private string gateUrl = ValidatorEnv.DeviceControllerConnectUrl;

        private ILog _log = LogManager.GetLogger(typeof(GateAppClient));

        /// <summary>
        /// how many seconds the LED will be showing
        /// </summary>
        private readonly int _timeoutSec = ValidatorEnv.LEDDisplaySec;


        /// <summary>
        /// this value will be set by CEPAS reader heartbeat command every X seconds
        /// </summary>
        public bool EntryReader_Status { get; set; }

        /// <summary>
        /// this value will be set by CEPAS reader heartbeat command every X seconds
        /// </summary>
        public bool ExitReader_Status { get; set; }

        public bool PicoReader_Status { get; set; }


        public void HeartbeatServiceNonBlockIO()
        {
            var baseUrl_wirecard = ValidatorEnv.DeviceControllerHeartbeatUrl;
            var interval = ValidatorEnv.DeviceControllerHeartbeatIntervalSec;
            var baseUrl_pico = ValidatorEnv.DeviceControllerPincodeHeartbeatUrl;
            Net45Task.Run(() =>
            {
                while (true)
                {
                    if (ValidatorEnv.IsExit())
                    {
                        try
                        {
                            var url = baseUrl_wirecard + "?status=" + ExitReader_Status.ToString().ToLowerInvariant()
                                                + "&type=A";
                            //_log.Debug("[Heartbeat Exit Reader] : " + url);
                            HttpSync("", url, "POST", false);

                        }
                        catch (Exception ex)
                        {
                            //_log.Error(ex);
                        }
                    }
                    else
                    {
                        try
                        {
                            var url = baseUrl_wirecard + "?status=" + EntryReader_Status.ToString().ToLowerInvariant()
                                                + "&type=B";
                            //_log.Debug("[Heartbeat Entry Reader] : " + url);
                            HttpSync("", url, "POST", false);

                        }
                        catch (Exception ex)
                        {
                            //_log.Error(ex);
                        }
                        try
                        {
                            var url = baseUrl_pico + "?status=" + PicoReader_Status.ToString().ToLowerInvariant();
                            //_log.Debug("[Heartbeat Pico Reader] : " + url);
                            HttpSync("", url, "POST", false);
                        }
                        catch (Exception ex)
                        {
                            //_log.Error(ex);
                        }

                    }

                    Thread.Sleep(1000 * interval);
                }
            });
        }

        private void HttpSync(string messageBody, string url, string method = "POST", bool log = true)
        {
            if (log)
            {
                _log.Debug("##############  GATE COMMAND [SENDER] START ##############");
                _log.Debug(messageBody);
                _log.Debug("############## GATE COMMAND [SENDER] END ######");
            }


            HttpWebRequest httpWebRequest;
            HttpWebResponse httpWebResponse;
            try
            {
                httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/xml";
                httpWebRequest.ContentLength = Encoding.UTF8.GetByteCount(messageBody);
                httpWebRequest.Method = method;
                httpWebRequest.Timeout = 3000;

                Stream str = httpWebRequest.GetRequestStream();
                StreamWriter writer = new StreamWriter(str);


                writer.Write(messageBody);
                writer.Close();

                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader reader = new StreamReader(httpWebResponse.GetResponseStream());
                string resp = reader.ReadToEnd();

                reader.Close();
            }
            catch (Exception e)
            {
                //_log.Error("#### HTTP Error ####");
                //_log.Error(messageBody);
                //_log.Error(e);
            }
        }


        private const string Entry = "turnstileB";
        private const string Exit = "turnstileA";

        private const string EntryLightBlue = "exitblue";
        private const string ExitLightBlue = "entryblue";
        private const string EntryLightRed = "exitred";
        private const string ExitLightRed = "entryred";

        public void OpenAndGreenAsync()
        {
            if (ValidatorEnv.IsExit())
            {
                Net45Task.Run(() =>
                {
                    ExitAndGreenSync();
                    PlayOkSongAsync();
                });
                return;
            }

            int timeout = _timeoutSec;
            string cmd = "<commands>" +
                            "<command>" +
                                "<name>SetLED</name>" +
                                "<params>" +
                                    "<param><key>"+Entry+"</key><value>on</value></param>" +
                                    "<param><key>timeout</key><value>" + timeout * 1000 + "</value></param>" +
                                "</params>" +
                            "</command>" +
                            "<command>" +
                            "<name>SetLED</name>" +
                            "<params>" +
                                "<param><key>"+ EntryLightBlue+"</key><value>on</value></param>" +
                                "<param><key>timeout</key><value>" + timeout * 1000 + "</value></param>" +
                            "</params>" +
                         "</command>" +
                        "</commands>";
            Net45Task.Run(() =>
            {
                HttpSync(cmd, gateUrl);
            });

            PlayOkSongAsync();
        }

        public void OffAllLights()
        {
            string cmd = "<commands>" +
                            "<command>" +
                                "<name>SetLED</name>" +
                                "<params>" +
                                    "<param><key>" + ExitLightRed + "</key><value>off</value></param>" +
                                    "<param><key>timeout</key><value>" + 1 * 1000 + "</value></param>" +
                                "</params>" +
                             "</command>" +
                             "<command>" +
                                "<name>SetLED</name>" +
                                "<params>" +
                                    "<param><key>" + EntryLightRed + "</key><value>off</value></param>" +
                                    "<param><key>timeout</key><value>" + 1 * 1000 + "</value></param>" +
                                "</params>" +
                             "</command>" +
                             "<command>" +
                                "<name>SetLED</name>" +
                                "<params>" +
                                    "<param><key>" + ExitLightBlue + "</key><value>off</value></param>" +
                                    "<param><key>timeout</key><value>" + 1 * 1000 + "</value></param>" +
                                "</params>" +
                             "</command>" +
                             "<command>" +
                                "<name>SetLED</name>" +
                                "<params>" +
                                    "<param><key>" + EntryLightBlue + "</key><value>off</value></param>" +
                                    "<param><key>timeout</key><value>" + 1 * 1000 + "</value></param>" +
                                "</params>" +
                             "</command>" +
                         "</commands>";
            Net45Task.Run(() =>
            {
                HttpSync(cmd, gateUrl);
            });
        }

        public void RedAsync()
        {
            string color = ValidatorEnv.IsExit() ? ExitLightRed : EntryLightRed;
            int timeout = _timeoutSec;
            string cmd = "<commands>" +
                            "<command>" +
                                "<name>SetLED</name>" +
                                "<params>" +
                                    "<param><key>" + color + "</key><value>on</value></param>" +
                                    "<param><key>timeout</key><value>" + timeout * 1000 + "</value></param>" +
                                "</params>" +
                             "</command>" +
                         "</commands>";
            Net45Task.Run(() =>
            {
                HttpSync(cmd, gateUrl);
            });

            PlayErrorSongAsync();


            // TODO no need it for now
            //whenever a 'normal' command executed ,reset the 'long open/close' command
            //_toggleOpenCloseCommand.Reset();
            // TODO no need it for now
        }

        public void ExitAndGreenSync()
        {
            int timeout = _timeoutSec;
            string cmd = "<commands>" +
                            "<command>" +
                                "<name>SetLED</name>" +
                                "<params>" +
                                    "<param><key>"+Exit+"</key><value>on</value></param>" +
                                    "<param><key>timeout</key><value>" + timeout * 1000 + "</value></param>" +
                                "</params>" +
                            "</command>" +
                            "<command>" +
                            "<name>SetLED</name>" +
                            "<params>" +
                                "<param><key>"+ExitLightBlue+"</key><value>on</value></param>" +
                                "<param><key>timeout</key><value>" + timeout * 1000 + "</value></param>" +
                            "</params>" +
                         "</command>" +
                        "</commands>";
            HttpSync(cmd, gateUrl);
        }





        public void PlayOkSongAsync()
        {
            Net45Task.Run(() =>
            {
                SoundPlayer simpleSound = new SoundPlayer(Properties.Resources.correct1);
                simpleSound.Play();
            });
        }

        private void PlayErrorSongAsync()
        {
            Net45Task.Run(() =>
            {
                var simpleSound = new SoundPlayer(Properties.Resources.wrong1);
                simpleSound.Play();
            });
        }

        #region  turn this feature on  until they need it
        // private GateStaffModeCommand _toggleOpenCloseCommand = new GateStaffModeCommand();
        //public void LongToggleOpenClose()
        //{
        //    var xml = _toggleOpenCloseCommand.ToggleCommandXml();
        //    Task.Run(() =>
        //    {
        //        HttpSync(xml, gateUrl);
        //    });

        //    PlayOkSongAsync();
        //}
        #endregion

    }

    /// <summary>
    /// this command is for staff to make the gate long time open/close.
    /// </summary>
    internal class GateStaffModeCommand
    {
        public GateStaffModeCommand()
        {
        }

        private const int OPEN = 1;
        private const int CLOSE = 0;

        private int _status;

        public void Reset()
        {
            _status = CLOSE;
        }

        public string ToggleCommandXml()
        {
            if (_status == CLOSE)
            {
                _status = OPEN;
                if (ValidatorEnv.IsExit())
                {
                    // open exit (B)
                    string cmd = "<commands>" +
                                    "<command><name>SetLED</name>" +
                                        "<params>" +
                                            "<param><key>gateB</key><value>on</value></param>" +
                                            "<param><key>timeout</key><value>-1</value></param>" +
                                        "</params>" +
                                    "</command>" +
                                 "</commands>";
                    return cmd;
                }
                else
                {
                    var cmd = "<commands>" +
                                "<command>" +
                                    "<name>SetLED</name>" +
                                    "<params>" +
                                        "<param><key>gateA</key><value>on</value>" +
                                        "</param><param><key>timeout</key><value>-1</value></param>" +
                                     "</params>" +
                                "</command>" +
                             "</commands>";
                    // open entry (A)
                    return cmd;
                }

            }
            else
            {
                _status = CLOSE;
                if (ValidatorEnv.IsExit())
                {
                    // close exit (B)
                    string cmd = "<commands>" +
                                    "<command><name>SetLED</name>" +
                                        "<params>" +
                                            "<param><key>gateB</key><value>off</value></param>" +
                                            "<param><key>timeout</key><value>-1</value></param>" +
                                        "</params>" +
                                    "</command>" +
                                 "</commands>";
                    return cmd;
                }
                else
                {
                    // close entry   (A)
                    string cmd = "<commands>" +
                                    "<command><name>SetLED</name>" +
                                        "<params>" +
                                            "<param><key>gateA</key><value>off</value></param>" +
                                            "<param><key>timeout</key><value>-1</value></param>" +
                                        "</params>" +
                                    "</command>" +
                                 "</commands>";
                    return cmd;
                }

            }
        }
    }
}
