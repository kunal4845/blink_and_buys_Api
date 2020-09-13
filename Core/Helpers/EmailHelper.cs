using System;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helper
{
    public static class EmailHelper
    {
        public static async Task<bool> SendEmail(string templateName, SmtpCredentials _smtpCredentials)
        {
            try
            {
                var builder = new StringBuilder();
                using (var reader = File.OpenText("Templates\\" + templateName))
                {
                    builder.Append(reader.ReadToEnd());
                }

                builder.Replace("{{Url}}", "http://localhost:4200/reset-your-password/");

                MailMessage mail = new MailMessage();
                foreach (var toEmail in _smtpCredentials.To)
                {
                    mail.To.Add(toEmail);
                }

                mail.From = new MailAddress(_smtpCredentials.ServerAddress);
                mail.Subject = _smtpCredentials.Subject;
                mail.Body = builder.ToString();
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(_smtpCredentials.Host, _smtpCredentials.Port);
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(_smtpCredentials.ServerAddress, _smtpCredentials.Password);
                await smtp.SendMailAsync(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
