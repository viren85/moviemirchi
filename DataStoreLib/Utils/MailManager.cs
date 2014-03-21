using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using System.Net.Mail;

namespace DataStoreLib.Utils
{
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


                string sUser = ConfigurationManager.AppSettings["User"];
                string sPassword = ConfigurationManager.AppSettings["Password"];
                string sHost = ConfigurationManager.AppSettings["Host"];
                int iPort = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                bool bEnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                // bool bUseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]);

                MailMessage mailMessage = new MailMessage();
                mailMessage.IsBodyHtml = true;

                if (fromAddress == "")
                {
                    mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["fromAddress"]);  // Key from the config file
                }
                else
                {
                    mailMessage.From = new MailAddress(fromAddress);
                }

                mailMessage.To.Add(toAddress); // Key from the config file

                if (subject == "")
                {
                    mailMessage.Subject = ConfigurationManager.AppSettings["subject"]; // Key from the config file
                }
                else
                {
                    mailMessage.Subject = subject;
                }

                mailMessage.Body = body;
                SmtpClient smtpServer = new SmtpClient(sHost);  // Key the mail server
                smtpServer.Port = iPort;
                smtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["fromAddress"], ConfigurationManager.AppSettings["Password"]);
                smtpServer.EnableSsl = bEnableSsl;
                smtpServer.Send(mailMessage);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
