using System;
using System.Configuration;
using System.Net.Mail;
using System.Net;

namespace DNC.WEB.Services
{
    public class EmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpEmail;
        private readonly string _smtpPassword;
        private readonly bool _smtpEnableSsl;

        public EmailService()
        {
            _smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
            _smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
            _smtpEmail = ConfigurationManager.AppSettings["SmtpEmail"];
            _smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
            _smtpEnableSsl = bool.Parse(ConfigurationManager.AppSettings["SmtpEnableSsl"]);
        }

        public bool SendEmail(string to, string subject, string body)
        {
            try
            {
                using (var smtpClient = new SmtpClient(_smtpServer, _smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(_smtpEmail, _smtpPassword);
                    smtpClient.EnableSsl = _smtpEnableSsl;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_smtpEmail),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };
                    mailMessage.To.Add(to);

                    smtpClient.Send(mailMessage);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
