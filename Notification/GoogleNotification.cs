using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using log4net;
using Newtonsoft.Json;

namespace BCMS.WebApp.Helper
{
    public static class GoogleNotification
    {

        public class GoogleNotificationRequestObj
        {
            public IList<string> registration_ids { get; set; }
            public Dictionary<string, string> notification { get; set; }
        }

        private static ILog _log = LogManager.GetLogger(typeof(GoogleNotification));

        public static string CallGoogleAPI(string receiverList, string title, string message, string messageId = "-1")
        {
            string result = "";

            string applicationId = ConfigurationManager.AppSettings["GcmAuthKey"];

            WebRequest wRequest;
            wRequest = WebRequest.Create("https://gcm-http.googleapis.com/gcm/send");
            wRequest.Method = "post";
            wRequest.ContentType = " application/json;charset=UTF-8";
            wRequest.Headers.Add(string.Format("Authorization: key={0}", applicationId));

            //sample Request :
            //postData = string.Format(@"{{ 
            //                                ""registration_ids"": [{0}],
            //                                ""notification"" : {{
            //                                        ""body"" : ""{1}"",
            //                                        ""title"" : ""{2}"",
            //                                        ""messageId"": ""{3}""
            //                                    }}
            //                                }}", receiverList, message, title, messageId);

            string postData;
            var obj = new GoogleNotificationRequestObj()
            {
                registration_ids = new List<string>() { receiverList },
                notification = new Dictionary<string, string>
                {
                    {"body", message},
                    {"title", title}
                }
            };

            if (messageId != "-1")
            {
                obj.notification.Add("messageId", messageId);
            }
            postData = JsonConvert.SerializeObject(obj);
            _log.Info(postData);

            Byte[] bytes = Encoding.UTF8.GetBytes(postData);
            wRequest.ContentLength = bytes.Length;

            Stream stream = wRequest.GetRequestStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();

            WebResponse wResponse = wRequest.GetResponse();

            stream = wResponse.GetResponseStream();

            StreamReader reader = new StreamReader(stream);

            String response = reader.ReadToEnd();

            HttpWebResponse httpResponse = (HttpWebResponse)wResponse;
            string status = httpResponse.StatusCode.ToString();

            reader.Close();
            stream.Close();
            wResponse.Close();

            if (status != "OK")
            {
                result = string.Format("{0} {1}", httpResponse.StatusCode, httpResponse.StatusDescription);
            }

            return result;
        }

    }
}