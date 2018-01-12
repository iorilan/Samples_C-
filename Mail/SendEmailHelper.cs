using System;
using System.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using log4net;

//
namespace SeletarPortal
{
    public class Emailer
    {
        public string Address { get; set; }
        public string Name { get; set; }

        public Emailer(string address , string name)
        {
            Address = address;
            Name = name;
        }
    }

    public class APDEmail
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string MediaType { get; set; }
        public Attachment MailAttachment { get; set; }

        public APDEmail(string title, string content, Attachment attachment)
        {
            Title = title;
            Content = content;
            MediaType = MediaTypeNames.Text.Plain;
            MailAttachment = attachment;
        }
    }
    public static class SendEmailHelper
    {
        private static string EmailServerHost ;
        private static int EmailServerPort;
        private static string EmailUserName;
        private static string EmailPassword;



        private static ILog _log = LogManager.GetLogger(typeof(SendEmailHelper));
        static SendEmailHelper()
        {
            EmailServerHost = ConfigurationManager.AppSettings["EmailServerAddress"];
            if (EmailServerHost == "")
            {
                _log.Error("### Email initailize Failed . [EmailServerAddress] setting not found ###");
                return;
            }

            if (!int.TryParse(ConfigurationManager.AppSettings["EmailServerPort"], out EmailServerPort))
            {
                _log.Error("### Email initailize Failed . [EmailServerPort] setting not found ###");
                return;
            }

            EmailUserName = ConfigurationManager.AppSettings["EmailUserName"];
            if (EmailUserName == "")
            {
                _log.Error("### Email initailize Failed . [EmailUserName] setting not found ###");
                return;
            }

            EmailPassword = ConfigurationManager.AppSettings["EmailPassword"];
            if (EmailPassword == "")
            {
                _log.Error("### Email initailize Failed . [EmailPassword] setting not found ###");
                return;
            }
        }
        
        public static bool Send(Emailer to, APDEmail email)
        {
            try
            {
                MailMessage mailMsg = new MailMessage();

                // To
                mailMsg.To.Add(new MailAddress(to.Address, to.Name));

                // From
                mailMsg.From = new MailAddress(EmailUserName, "your name");

                if (email.MailAttachment != null)
                {
                    mailMsg.Attachments.Add(email.MailAttachment);
                }
                
                // Subject and multipart/alternative Body
                mailMsg.Subject = email.Title;
                string content = email.Content;
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(content, null, email.MediaType));

                // Init SmtpClient and send

                SmtpClient smtpClient = 
                new SmtpClient(EmailServerHost, EmailServerPort);

                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(EmailUserName, EmailPassword);
                smtpClient.Credentials = credentials;
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMsg);

                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.FormatException());
                return false;
            }
        }
    }
}
