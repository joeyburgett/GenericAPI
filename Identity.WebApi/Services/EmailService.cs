#region Using Directives

using System.Configuration;
using Microsoft.AspNet.Identity;
using SendGrid;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
#endregion

namespace Identity.WebApi.Services
{
    /// <summary>
    /// EmailService
    /// </summary>
    /// <remarks>
    /// This uses the free servce (SendGrid) @ https://sendgrid.com/
    /// </remarks>
    public class EmailService 
        : IIdentityMessageService
    {
        #region Public Methods

        /// <summary>
        /// This method should send the message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendAsync(IdentityMessage message)
        {
            await configureSendGridAsync(message);
        }

        #endregion

        #region Private Methods

        private async Task configureSendGridAsync(IdentityMessage message)
        {
            var msg = new SendGridMessage
            {
                From = new MailAddress(ConfigurationManager.AppSettings["mail_from_address"], ConfigurationManager.AppSettings["mail_from"]),
                Subject = message.Subject,
                Text = message.Body,
                Html = message.Body
            };

            msg.AddTo(message.Destination);

            var credentials = new NetworkCredential(ConfigurationManager.AppSettings["sendgrid_username"], ConfigurationManager.AppSettings["sendgrid_password"]);
            var transport = new Web(credentials);

            await transport.DeliverAsync(msg);
        }

        #endregion
    }
}