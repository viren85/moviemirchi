
namespace DataStoreLib.Utils
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Mail;

    /// <summary>
    /// This class use to send mails
    /// </summary>
    public class MailManager
    {
        /// <summary>
        /// Send an email
        /// </summary>
        /// <param name="toAddress">mail message to-Address</param>
        /// <param name="fromAddress">mail message from-Address</param>
        /// <param name="subject">mail message subject</param>
        /// <param name="body">mail message body</param>
        public bool SendMail(string toAddress, string fromAddress, string subject, string body)
        {
            try
            {
                string user = ConfigurationManager.AppSettings["User"];
                string password = ConfigurationManager.AppSettings["Password"];
                string host = ConfigurationManager.AppSettings["Host"];
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                bool enableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                ////bool useDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]);

                MailMessage mailMessage = new MailMessage();
                mailMessage.IsBodyHtml = true;

                if (string.IsNullOrWhiteSpace(fromAddress))
                {
                    mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["fromAddress"]);  // Key from the config file
                }
                else
                {
                    mailMessage.From = new MailAddress(fromAddress);
                }

                mailMessage.To.Add(toAddress); // Key from the config file

                if (string.IsNullOrWhiteSpace(subject))
                {
                    mailMessage.Subject = ConfigurationManager.AppSettings["subject"]; // Key from the config file
                }
                else
                {
                    mailMessage.Subject = subject;
                }

                mailMessage.Body = body;
                SmtpClient smtpServer = new SmtpClient(host);  // Key the mail server
                smtpServer.Port = port;
                smtpServer.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["fromAddress"], ConfigurationManager.AppSettings["Password"]);
                smtpServer.EnableSsl = enableSsl;
                smtpServer.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Trace.TraceWarning("Sending mail failed: {0}", ex);
                return false;
            }

            return true;
        }
    }
}
