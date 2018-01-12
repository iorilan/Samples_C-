using System.Configuration;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PushSharp.Apple;

namespace BCMS.WebApp.Helper
{
    public class PayLoadEntity
    {
//        JObject.Parse("{" +
//                                        "\"aps\":{\"alert\":\"Hello\"}" +
//
//                                        "}")
        public Aps aps { get; set; }
    }

    public class Aps
    {
        public string alert { get; set; }
    }
    public class AppleNotification
    {
        private static ILog _log = LogManager.GetLogger(typeof(AppleNotification));

        public static bool Push(string apnToken, string message)
        {
            _log.DebugFormat("[Apns] step 1");

            _log.DebugFormat("Token = " + apnToken);

            var config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Sandbox,
                ConfigurationManager.AppSettings["ApnsCertificate"].ToString(), ConfigurationManager.AppSettings["ApnsPassword"].ToString());

            // Create a new broker
            var apnsBroker = new ApnsServiceBroker(config);
            _log.DebugFormat("[Apns] step 2");
            // Wire up events
            apnsBroker.OnNotificationFailed += (notification, aggregateEx) =>
            {
                _log.DebugFormat("[Apns] step 3");
                aggregateEx.Handle(ex =>
                {
                    _log.DebugFormat("[Apns] step 4");
                    // See what kind of exception it was to further diagnose
                    if (ex is ApnsNotificationException)
                    {
                        _log.DebugFormat("[Apns] step 5");
                        var notificationException = (ApnsNotificationException)ex;
                        _log.DebugFormat("[Apns] step 6");
                        // Deal with the failed notification
                        var apnsNotification = notificationException.Notification;
                        var statusCode = notificationException.ErrorStatusCode;

                        _log.ErrorFormat($"Apple Notification Failed: ID={apnsNotification.Identifier}, Code={statusCode}");
                        return false;
                    }
                    else
                    {
                        // Inner exception might hold more useful information like an ApnsConnectionException			
                        _log.ErrorFormat($"Apple Notification Failed for some unknown reason : {ex.InnerException}");
                        return false;
                    }

                    // Mark it as handled
                    //return true;
                });
            };
            _log.DebugFormat("[Apns] step 7");
            apnsBroker.OnNotificationSucceeded += (notification) =>
            {
                _log.InfoFormat("Apple Notification Sent!");
            };

            _log.DebugFormat("[Apns] step 8");
            // Start the broker
            apnsBroker.Start();
            _log.DebugFormat("[Apns] step 9");

            // Queue a notification to send
            var apnsObj = new PayLoadEntity()
            {
                aps = new Aps()
                {
                    alert = message
                }
            };
            var apnsStr = JsonConvert.SerializeObject(apnsObj);
            _log.DebugFormat("[Apns] step 9.1");
            _log.DebugFormat(apnsStr);
            apnsBroker.QueueNotification(new ApnsNotification
            {
                DeviceToken = apnToken,
                Payload = JObject.Parse(apnsStr)
            });

            _log.DebugFormat("[Apns] step 10");

            // Stop the broker, wait for it to finish   
            // This isn't done after every message, but after you're
            // done with the broker
            apnsBroker.Stop();
            _log.DebugFormat("[Apns] step 11");
            return true;
        }
		
		
		static async void TokenBasedAuthenticationAPNsPush(string message, string token)
        {
            string algorithm = "ES256";

            string apnsKeyId = "XXXX";
            string teamId = "XXXX";
            string authKeyPath = "xxx.p8";

            string bundleId = "XXXX";
            string registrationId = token;


            var privateKeyContent = System.IO.File.ReadAllText(authKeyPath);
            var privateKey = privateKeyContent.Split('\n')[1];

            var secretKeyFile = Convert.FromBase64String(privateKey);
            var secretKey = CngKey.Import(secretKeyFile, CngKeyBlobFormat.Pkcs8PrivateBlob);

            var expiration = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var expirationSeconds = (long)expiration.TotalSeconds;

            var payload = new Dictionary<string, object>()
            {
                { "iss", teamId },
                { "iat", expirationSeconds }
            };
            var header = new Dictionary<string, object>()
            {
                { "alg", algorithm },
                { "kid", apnsKeyId }
            };

            string accessToken = Jose.JWT.Encode(payload, secretKey, JwsAlgorithm.ES256, header);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

            //Development server:api.development.push.apple.com:443
            //Production server:api.push.apple.com:443

            string host = "api.development.push.apple.com";
            //string host = "api.push.apple.com";
            int port = 443;

            // Uri to request
            var uri = new Uri(string.Format("https://{0}:{1}/3/device/{2}", host, port, registrationId));

            var payloadData = JObject.FromObject(new
            {
                aps = new
                {
                    alert = message
                }
            });

            byte[] data = System.Text.Encoding.UTF8.GetBytes(payloadData.ToString());

            var handler = new Http2MessageHandler();
            var httpClient = new HttpClient(handler);
            var requestMessage = new HttpRequestMessage();
            requestMessage.RequestUri = uri;
            requestMessage.Headers.Add("authorization", string.Format("bearer {0}", accessToken));
            requestMessage.Headers.Add("apns-id", Guid.NewGuid().ToString());
            requestMessage.Headers.Add("apns-expiration", "0");
            requestMessage.Headers.Add("apns-priority", "10");
            requestMessage.Headers.Add("apns-topic", bundleId);
            requestMessage.Method = HttpMethod.Post;
            requestMessage.Content = new ByteArrayContent(data);

            try
            {
                var responseMessage = await httpClient.SendAsync(requestMessage);

                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseUuid = string.Empty;
                    IEnumerable<string> values;
                    if (responseMessage.Headers.TryGetValues("apns-id", out values))
                    {
                        responseUuid = values.First();
                    }
                    Console.WriteLine(string.Format("\n\r*******Send Success [{0}]", responseUuid));
                }
                else
                {
                    var body = await responseMessage.Content.ReadAsStringAsync();
                    var json = new JObject();
                    json = JObject.Parse(body);

                    var reasonStr = json.Value<string>("reason");
                    Console.WriteLine("\n\r*******Failure reason => " + reasonStr);
                }

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                var info = "";
                DumpAllInfoOfException(ex, ref info);
                Console.WriteLine("\n\r*******Exception message => " + ex.Message);
                Console.WriteLine(info);
            }
        }

    }
}