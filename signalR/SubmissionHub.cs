using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SampleVerificationApp.Hub
{
    [HubName("submissionHub")]
    public class SubmissionHub : Microsoft.AspNet.SignalR.Hub
    {
        private static ILog log = LogManager.GetLogger(typeof(SubmissionHub));

        public static Dictionary<string, string> GuestConnections = new Dictionary<string, string>();

        public static string ConnectionId(string guestName)
        {
            if (!GuestConnections.ContainsKey(guestName))
            {
                throw new ApplicationException("no signalR connection found for guest :" + guestName);
            }

            return GuestConnections[guestName];
        }
        /*
            Client
        */
        public void HiServer(string guestName)
        {
            if (!GuestConnections.ContainsKey(guestName))
            {
                GuestConnections.Add(guestName, Context.ConnectionId);
            }
            else
            {
                GuestConnections[guestName] = Context.ConnectionId;
            }

            Clients.All.commandText("hi from client:" + guestName + ", connection : " + Context.ConnectionId);
        }

        public static void SendCommand(string guestName, string command)
        {
            IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<SubmissionHub>();
            if (SubmissionHub.GuestConnections.ContainsKey(guestName))
            {
                try
                {
                    var connection = SubmissionHub.ConnectionId(guestName);
                    hubContext.Clients.Client(connection).commandText(command);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
                
            }
        }
    }
}