using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm_Project_106.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly EmailSettings _emailsettings;
        public EmailSender(IConfiguration configuration, IOptions<EmailSettings> emailsettings)
        {
            _emailsettings = emailsettings.Value;
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Execute(email, subject, htmlMessage).Wait();
            return Task.FromResult(0);
        }
        public async Task Execute(string email, string subject, string message)
        {
            try
            {
                string toEmail = string.IsNullOrEmpty(email) ?
                    _emailsettings.ToEmail : email;
                MailMessage mailMessage = new MailMessage()
                {
                    From = new MailAddress(_emailsettings.UsernameEmail, "Book Shopping App"),
                };
                mailMessage.To.Add(toEmail);
                mailMessage.CC.Add(_emailsettings.CcEmail);
                mailMessage.Subject = "ShoppingApp:" + subject;
                mailMessage.Body = message;
                mailMessage.IsBodyHtml = true;
                mailMessage.Priority = MailPriority.High;
                using (SmtpClient smtpClient = new SmtpClient(_emailsettings.PrimaryDomain, _emailsettings.PrimaryPort))
                {
                    smtpClient.Credentials = new NetworkCredential(_emailsettings.UsernameEmail,_emailsettings.UsernamePassword);
                    smtpClient.EnableSsl = true;
                    await smtpClient.SendMailAsync(mailMessage);
                };
                
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }
    }
}
