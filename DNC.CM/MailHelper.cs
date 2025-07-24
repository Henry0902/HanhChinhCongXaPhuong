using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
namespace DNC.CM
{
    public  class MailHelper
    {
        private string mailServer = "";
        private string emailSender = "";
        private string passEmailSender = "";
        private int portserver=587;
       
        public string SendMail(string to, string subject, string body, string attachments, string cc, string bcc,string mailForm, string passSentMail)
        {
            try
            {
                mailServer = "mail.fpt.com.vn";
                portserver = 587;
                MailMessage mail = new MailMessage();
                SmtpClient smtp = new SmtpClient(mailServer);
                mail.IsBodyHtml = true;
                mail.From = new MailAddress(mailForm);
                mail.Subject = subject;
                mail.Body = body;
                smtp.Port = portserver;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateCertificate);
                smtp.Credentials = new NetworkCredential(mailForm, passSentMail);
                string toAdds = to + ',' + cc + ',' + bcc;
                toAdds.Replace(';', ',');
                string[] t = toAdds.Split(',');
                if (!String.IsNullOrEmpty(attachments))
                {
                    string[] fileAttachments = attachments.Replace(';', ',').Split(',');
                    //foreach (string file in fileAttachments)
                    //{
                    //    if (String.IsNullOrEmpty(file)) continue;
                    //    File f = Directory.GetFiles.GetFile(file);
                    //    Attachment data = new Attachment(f.OpenBinaryStream(), f.Name);
                    //    ContentDisposition disposition = data.ContentDisposition;
                    //    disposition.CreationDate = File.GetCreationTime(file);
                    //    disposition.ModificationDate = File.GetLastWriteTime(file);
                    //    disposition.ReadDate = File.GetLastAccessTime(file);
                    //    mail.Attachments.Add(data);
                    //}
                }
                foreach (string s in t)
                {
                    if (string.IsNullOrEmpty(s)) continue;
                    mail.To.Add(s.Trim());                    
                    smtp.Send(mail);
                    mail.To.RemoveAt(0);
                }
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        static bool ValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
        public string SendAlertEmail(string UserReceiver, string emailTo, string subject, string userSender, string module, string link, string action, string vbid, string content,string mailForm,string passSentMail)
        {
            try
            {
                if (string.IsNullOrEmpty(subject))
                    subject = "[Hệ thống EISO - " + module + @"] Bạn có 1 thông báo mới!";
                string body = @"<p>Xin chào <a href='javascript:' style='font-weight: bold'>" + UserReceiver + @"</a> !</p>
                <p><strong>" + userSender + @"</strong> <em><span style='color:#2059D3'> " + action + @"</span></em> 1 tài liệu.</p>
                <p>Thông tin tài liệu:</p>
                <p>- Module: <em><span style='color:#2059D3'>" + module + @"</span>.</em></p>
                <p>- Mã: <em><span style='color:#2059D3'>" + vbid + @"</span>.</em></p>
                <p>- Nội dung tóm tắt: <em><span style='color:#2059D3'>" + content + @"</span>.</em></p>
                <p>Thông tin chi tiết vui lòng <a href='" + link + "'>đăng nhập hệ thống tại đây</a>.</p></br><img src='https://www.fpt.com.vn/Content/home/images/icon/ic-logo.png'></img></br>";
                if (!string.IsNullOrEmpty(emailTo))
                    return SendMail(emailTo, subject, body, "", "", "",mailForm,passSentMail);
                else return "";
            }
            catch (Exception ex)
            { return "Lỗi sendMail:" + ex.ToString(); }
        }
    }
}
