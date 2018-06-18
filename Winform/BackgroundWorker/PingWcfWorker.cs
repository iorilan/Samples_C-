using System;
using System.ComponentModel;
using System.ServiceModel;
using  .Client.ValidatorBase.Config;
using  .Client.ValidatorBase.Factory;
using  .Client.ValidatorBase.Helpers;

namespace  .Client.ValidatorBase.BackgroundWorkers
{
    public class PingWcfWorker : WorkerBase
    {
        public PingWcfWorker()
        {
            _worker.DoWork += CheckWcfConnection;
        }

        public static bool IsBlockout { get; private set; }
        public static bool IsOnline { get; private set; }

        public string OnlineStatusStr
        {
            get { return IsOnline ? "Online" : "Offline"; }
        }

        private static TicketWcfServerClient _heartbeatClient = WCFClientFactory.CreateTicketingClient();
        public static void CheckWcfConnection(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (_heartbeatClient.State != CommunicationState.Opened ||
                    _heartbeatClient.State != CommunicationState.Opening ||
                    _heartbeatClient.State != CommunicationState.Created)
                {
                    _heartbeatClient.Abort();
                    _heartbeatClient = new TicketWcfServerClient("NetTcpBinding_ITicketWcfServer");
                    _heartbeatClient.Open();

                    IsBlockout = _heartbeatClient.IsAttractionInBlockout(ValidatorEnv.LocalFacilityInfo.FacilityId,
                        ValidatorEnv.LocalFacilityInfo.OperationId);

                    if (IsBlockout)
                    {
                        ProgramIdleChecker.StubActivity();
                    }
                }

                //IsOnline = false;
                IsOnline = true;
            }
            catch (Exception ex)
            {
                IsOnline = false;
                // ignore WCF connection exception 
            }
        }

        public static void PingSync()
        {
            CheckWcfConnection(null, null);
        }
    }
}
