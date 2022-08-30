using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace GSES.BusinessLogic.Extensions
{
    public static class SmtpClientExtension
    {
        public static async Task SendMessagesOnEmails(this SmtpClient client,
            string sender,
            IEnumerable<string> emails,
            string subject,
            string message)
        {
            foreach (var email in emails)
            {
                try
                {
                    await client.SendMailAsync(new MailMessage(sender, email, subject, message));
                }
                catch (Exception)
                {
                    Console.WriteLine("Sending failed: {0}", email);
                }
            }
        }
    }
}
