using Microsoft.Extensions.Options;
using System;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helper {
    public class EmailHelper {
        private readonly SmtpCredentials _smtpCredentials;
        public EmailHelper(IOptions<SmtpCredentials> smtpCredentials) {
            _smtpCredentials = smtpCredentials.Value;
        }
        public async Task<bool> SendEmail(EmailModel emailModel) {
            try {
                using (SmtpClient client = new SmtpClient(_smtpCredentials.Host)) {
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(_smtpCredentials.From, _smtpCredentials.Alias);
                    mailMessage.BodyEncoding = Encoding.UTF8;
                    mailMessage.To.Add(emailModel.To);
                    mailMessage.Body = emailModel.Message;
                    mailMessage.Subject = emailModel.Subject;
                    mailMessage.IsBodyHtml = emailModel.IsBodyHtml;
                    await client.SendMailAsync(mailMessage);
                    return true;
                }
            }
            catch (Exception ex) {
                throw ex;
            }
        }
    }
}
