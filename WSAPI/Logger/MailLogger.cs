using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WSAPI.Logger
{
    public class MailLogger
    {
        private static SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
        public static void SendAuthorizationLog(string to, string ip)
        {
            string html = File.ReadAllText("HtmlDocs/Security.html");

            html = html.Replace("*|IP|*", ip).Replace("*|DATE|*", DateTime.Now.ToString("dd.MM.yyyy HH:mm"));

            MailMessage mail = new MailMessage();
           
            mail.From = new MailAddress("faceless.kaonasi.dev@gmail.com");
            mail.To.Add(to);
            mail.IsBodyHtml = true;
            mail.Subject = "Важное оповещение системы безопасности";
            mail.Body = html;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("faceless.kaonasi.dev@gmail.com", "Oliver15243@");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }

        public static void SendRegistrationLog(string email, string password)
        {
            string html = File.ReadAllText("HtmlDocs/Registration.html");

            html = html.Replace("USER-1000", email).Replace("FYTerg243okr", password).Replace("*|FNAME|*", email);

            MailMessage mail = new MailMessage();

            mail.From = new MailAddress("faceless.kaonasi.dev@gmail.com");
            mail.To.Add(email);
            mail.IsBodyHtml = true;
            mail.Subject = "Thanks for joining us!";
            mail.Body = html;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("faceless.kaonasi.dev@gmail.com", "Oliver15243@");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }
    }
}
