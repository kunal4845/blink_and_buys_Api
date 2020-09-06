using Microsoft.Extensions.Options;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helper
{
    public static class EmailHelper
    {
        public static async Task<bool> SendEmail(EmailModel emailModel, string templateName, SmtpCredentials _smtpCredentials)
        {
            try
            {
                dynamic template = GetEmailTemplate(templateName);
                string emailBody = Engine.Razor.RunCompile(template.Template as string, "templateNameInTheCache", null, emailModel);

                using (SmtpClient client = new SmtpClient(_smtpCredentials.Host))
                {
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(_smtpCredentials.From, _smtpCredentials.Alias);
                    mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                    mailMessage.To.Add(emailModel.To);
                    mailMessage.Body = emailBody;
                    mailMessage.Subject = emailModel.Subject;
                    mailMessage.IsBodyHtml = true;

                    await client.SendMailAsync(mailMessage);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the email template.
        /// </summary>
        /// <param name="templateName">Name of the template.</param>
        /// <returns>Returns the e-mail template.</returns>
        private static dynamic GetEmailTemplate(string templateName)
        {
            string masterTemplateContents = GetTemplateFileContents("VerifiedTemplate.cshtml");
            string templateContents = GetTemplateFileContents(templateName + ".html.cshtml");

            return new { Layout = masterTemplateContents, Template = templateContents };
        }

        /// <summary>
        /// Gets the template file contents.
        /// </summary>
        /// <param name="templateFileName">The name of the template file.</param>
        /// <returns>Returns the contents of the template file.</returns>
        private static string GetTemplateFileContents(string templateFileName)
        {
            return GetEmailFileContents("Templates", templateFileName);
        }

        /// <summary>
        /// Gets the email file contents.
        /// </summary>
        /// <param name="lastNamespaceToken">The last namespace token.</param>
        /// <param name="templateFileName">The name of the template file.</param>
        /// <returns>
        /// Returns the contents of the template file.
        /// </returns>
        private static string GetEmailFileContents(string lastNamespaceToken, string templateFileName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            if (assembly != null)
            {
                StringBuilder sb = new StringBuilder();

                using (StreamReader sr = new StreamReader(assembly.GetManifestResourceStream(String.Format("MyApp.BusinessLogic.Communication.{0}.{1}", lastNamespaceToken, templateFileName))))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();

                        if (!line.StartsWith("@model"))
                        {
                            sb.AppendLine(line);
                        }
                    }
                }

                return sb.ToString();
            }

            return null;
        }
    }
}
