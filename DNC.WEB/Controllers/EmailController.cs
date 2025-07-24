using System.Net.Mail;
using System.Net;
using System;
using System.Web.Mvc;
using DNC.WEB.Models;
using System.Configuration;

namespace DNC.WEB.Controllers
{
    public class EmailController : Controller
    {
        [HttpPost]
        public ActionResult SendEmail(EmailModel model)
        {
            try
            {
                string smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
                int smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
                string smtpEmail = ConfigurationManager.AppSettings["SmtpEmail"];
                string smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
                bool smtpEnableSsl = bool.Parse(ConfigurationManager.AppSettings["SmtpEnableSsl"]);

                // Cấu hình SMTP
                var smtpClient = new SmtpClient(smtpServer)
                {
                    Port = smtpPort,
                    Credentials = new NetworkCredential(smtpEmail, smtpPassword),
                    EnableSsl = smtpEnableSsl,
                };

                // Tạo email
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpEmail),
                    Subject = model.Subject,
                    Body = model.Body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(model.To);

                // Gửi email
                smtpClient.Send(mailMessage);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}