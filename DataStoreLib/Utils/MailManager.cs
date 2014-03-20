using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return true;
        }
    }
}
