using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.IO;
using Imps.Services.CommonV4;
using Imps.Services.EDMService;
using System.Threading;

namespace Imps.Services.EDMService
{
    /// <summary>
    /// Send mail.
    /// </summary>
    public class MailHelper : IDisposable
    {
        #region Fields
        private static ITracing tracing = TracingManager.GetTracing(typeof(MailHelper));
        #endregion

        #region Properties
        SmtpClient client;
        object syncObject = new object();
        public static int FinishedCount = 0;

        #endregion

        #region Methods

        public MailHelper(string host, int port, string username, string pwd)
        {
            client = new SmtpClient(host, port);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(username, pwd);
        }

        public MailHelper()
        {

        }

        public void Send(DefaultMailTemplate template)
        {
            template.Process();
            lock (syncObject)
            {
                Send(template.mailConfig.From, template.mailConfig.FromFriendlyName, template.ToList, template.Subject, template.tempBodyString, template.resourceIdDic);
            }
        }

        public void Send(string from, string fromFriendlyName, List<string> tos, string subject, string bodyString, Dictionary<string, byte[]> resourceIdDic)
        {

            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress(from, fromFriendlyName);

                foreach (string to in tos)
                {
                    message.To.Add(to);
                }

                message.Subject = subject;

                List<LinkedResource> resList = new List<LinkedResource>();
                List<MemoryStream> msList = new List<MemoryStream>();

                try
                {
                    foreach (string stub in resourceIdDic.Keys)
                    {
                        MemoryStream ms = new MemoryStream(resourceIdDic[stub]);
                        msList.Add(ms);
                        LinkedResource imageResource = new LinkedResource(ms, "image/jpeg");

                        resList.Add(imageResource);
                        bodyString = bodyString.Replace(stub, imageResource.ContentId);
                    }

                    using (AlternateView htmlView = AlternateView.CreateAlternateViewFromString(bodyString, null, "text/html"))
                    {
                        foreach (LinkedResource res in resList)
                        {
                            htmlView.LinkedResources.Add(res);
                        }

                        message.AlternateViews.Add(htmlView);
                        client.Send(message);
                    }
                }
                finally
                {
                    foreach (LinkedResource res in resList)
                    {
                        res.Dispose();
                    }

                    foreach (MemoryStream ms in msList)
                    {
                        ms.Dispose();
                    }
                }
            }
        }



        /// <summary>
        /// Send mail to user.
        /// </summary>
        /// <param name="template"></param>
        public void SendMail(DefaultMailTemplate template)
        {
            template.Process();

            using (SmtpClient client = new SmtpClient(template.mailConfig.Host, template.mailConfig.Port))
            {
                client.Timeout = EDMMSConfigSection.Current.MailClientTimeout;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(template.mailConfig.UserName, template.mailConfig.Password);

                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress(template.mailConfig.From, template.mailConfig.FromFriendlyName);
                    List<LinkedResource> resList = new List<LinkedResource>();
                    List<MemoryStream> msList = new List<MemoryStream>();
                    foreach (string to in template.ToList)
                    {
                        message.To.Add(to);
                    }

                    message.Subject = template.Subject;

                    string bodyString = template.tempBodyString;
                    try
                    {
                        foreach (string stub in template.resourceIdDic.Keys)
                        {
                            MemoryStream ms = new MemoryStream(template.resourceIdDic[stub]);
                            msList.Add(ms);
                            LinkedResource imageResource = new LinkedResource(ms, "image/jpeg");

                            resList.Add(imageResource);
                            bodyString = bodyString.Replace(stub, imageResource.ContentId);
                        }

                        using (AlternateView htmlView = AlternateView.CreateAlternateViewFromString(bodyString, null, "text/html"))
                        {
                            foreach (LinkedResource res in resList)
                            {
                                htmlView.LinkedResources.Add(res);
                            }

                            message.AlternateViews.Add(htmlView);
                            //
                            client.Send(message);
                        }
                    }
                    catch (Exception)
                    {
                        foreach (LinkedResource res in resList)
                        {
                            res.Dispose();
                        }

                        foreach (MemoryStream ms in msList)
                        {
                            ms.Dispose();
                        }

                        throw;
                    }
                }
            }
        }


        /// <summary>
        /// Send mail to user.
        /// </summary>
        /// <param name="template"></param>
        public void SendMailAsync(DefaultMailTemplate template, UserTask taskItem, Action<Exception, UserTask> callback)
        {
            List<LinkedResource> resList = new List<LinkedResource>();
            List<IDisposable> resourceList = new List<IDisposable>();

            try
            {
                taskItem.beginTicks = DateTime.Now.Ticks;
                template.Process();

                SmtpClient client = new SmtpClient(template.mailConfig.Host, template.mailConfig.Port);
                resourceList.Add(client);
                client.Timeout = EDMMSConfigSection.Current.MailClientTimeout;

                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(template.mailConfig.UserName, template.mailConfig.Password);

                MailMessage message = new MailMessage();
                resourceList.Add(message);
                message.From = new MailAddress(template.mailConfig.From, template.mailConfig.FromFriendlyName);

                foreach (string to in template.ToList)
                {
                    message.To.Add(to);
                }

                message.Subject = template.Subject;

                string bodyString = template.tempBodyString;

                foreach (string stub in template.resourceIdDic.Keys)
                {
                    MemoryStream ms = new MemoryStream(template.resourceIdDic[stub]);
                    resourceList.Add(ms);
                    LinkedResource imageResource = new LinkedResource(ms, "image/jpeg");
                    resourceList.Add(imageResource);
                    resList.Add(imageResource);
                    bodyString = bodyString.Replace(stub, imageResource.ContentId);
                }

                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(bodyString, null, "text/html");
                resourceList.Add(htmlView);
                foreach (LinkedResource res in resList)
                {
                    htmlView.LinkedResources.Add(res);
                }

                //
                message.AlternateViews.Add(htmlView);

                client.SendCompleted += (s, e) =>
                    {
                        try
                        {
                            MailMessage msg = e.UserState as MailMessage;
                            if (msg != null)
                            {
                                msg.Dispose();
                            }

                            SmtpClient mailClient = s as SmtpClient;
                            if (mailClient != null)
                            {
                                mailClient.Dispose();
                            }

                            if (callback != null)
                            {
                                callback(e.Error, taskItem);
                            }
                        }
                        catch (Exception inEx)
                        {
                            tracing.ErrorFmt(inEx, "client.SendCompleted failed with unexpected error.");
                        }
                    };

                client.SendAsync(message, message);
            }
            catch (Exception)
            {
                foreach (IDisposable item in resourceList)
                {
                    item.Dispose();
                }

                throw;
            }
        }






        public void SendSimple(string host, int port, string userName, string password,
            string from, string to, string subject, string bodyString, bool isHtml)
        {
            using (SmtpClient client = new SmtpClient(host, port))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(userName, password);

                for (int i = 0; i < 1; i++)
                {
                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress(from);

                        string[] toList = to.Split(new char[] { ';' });
                        foreach (string item in toList)
                        {
                            message.To.Add(item);
                        }
                        
                        message.IsBodyHtml = isHtml;
                        message.Subject = subject;
                        message.Body = bodyString;
                      
                        client.Send(message);
                    }
                }
            }
        }

        public void SendSimpleAsyncCallback(string host, int port, string userName, string password,
        string from, string to, string subject, string bodyString, bool isHtml, object userToken, Action<Exception, object> callback)
        {
            SmtpClient client = new SmtpClient(host, port);
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(userName, password);

                MailMessage message = new MailMessage();
                {
                    message.From = new MailAddress(from);

                    string[] toList = to.Split(new char[] { ';' });
                    foreach (string item in toList)
                    {
                        message.To.Add(item);
                    }

                    message.Subject = subject;
                    message.Body = bodyString;

                    MailTask task = userToken as MailTask;
                    task.msg = message;

                    client.SendCompleted += (s, e) =>
                        {

                            message.Dispose();
                            client.Dispose();
                            try
                            {
                                throw new ApplicationException("test ex");
                            }
                            finally
                            {
                                callback(e.Error, userToken);
                            }
                        };
                    //throw new ApplicationException("test ex");
                    client.SendAsync(message, userToken);
                }
            }
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:丢失范围之前释放对象")]
        public void SendSimpleAsync(string host, int port, string userName, string password,
          string from, string to, string subject, string bodyString, bool isHtml, object userToken, Action<Exception, object> callback)
        {
            SmtpClient client = new SmtpClient(host, port);

            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(userName, password);

            MailMessage message = new MailMessage();

            message.From = new MailAddress(from);

            string[] toList = to.Split(new char[] { ';' });
            foreach (string item in toList)
            {
                message.To.Add(item);
            }

            message.Subject = subject;
            message.Body = bodyString;

            MailTask task = userToken as MailTask;
            task.msg = message;
            client.SendCompleted += new SendCompletedEventHandler(client_SendCompleted);
            client.SendAsync(message, userToken);
        }

        void client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MailTask mailTask = e.UserState as MailTask;
            mailTask.msg.Dispose();
            SmtpClient client = sender as SmtpClient;
            client.Dispose();

            if (e.Error != null)
            {

            }

            Interlocked.Increment(ref FinishedCount);

            if (mailTask.totalCount == FinishedCount)
            {
                mailTask.ev.Set();
            }
        }


        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (client != null)
                {
                    client.Dispose();
                }
            }
        }

        #endregion

    }
}

