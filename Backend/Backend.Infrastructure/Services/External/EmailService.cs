﻿
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Text;
using Backend.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Backend.Infrastructure.Interface.Email;

namespace Backend.Infrastructure.Services.External
{
    public class EmailService: IEmailService
    {

        private IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmail(EmailInfo emailInfo)
        {
            // Email configuration
            string senderEmail = _configuration.GetValue<string>("MailSettings:Email").Trim();
            string password = _configuration.GetValue<string>("MailSettings:Password").Trim();
            string host = _configuration.GetValue<string>("MailSettings:Host").Trim();
            int port = _configuration.GetValue<int>("MailSettings:Port");
            string displayName = _configuration.GetValue<string>("MailSettings:DisplayName");

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            using (MailMessage message = new MailMessage())
            using (SmtpClient smtpClient = new SmtpClient(host, port))
            {
                // Set sender email and display name
                message.From = new MailAddress(senderEmail, displayName, Encoding.UTF8);

                // Set recipient email(s)
                if (!string.IsNullOrEmpty(emailInfo.To))
                {
                    foreach (string toEmailId in emailInfo.To.Split(','))
                    {
                        message.To.Add(new MailAddress(toEmailId.Trim()));
                    }
                }

                // Set CC email(s)
                if (!string.IsNullOrEmpty(emailInfo.Cc))
                {
                    foreach (string ccEmailId in emailInfo.Cc.Split(','))
                    {
                        message.CC.Add(new MailAddress(ccEmailId.Trim()));
                    }
                }

                // Set Bcc email(s)
                if (!string.IsNullOrEmpty(emailInfo.Bcc))
                {
                    foreach (string bccEmailId in emailInfo.Bcc.Split(','))
                    {
                        message.Bcc.Add(new MailAddress(bccEmailId.Trim()));
                    }
                }

                // Set email body and subject
                message.Body = emailInfo.Body;
                message.IsBodyHtml = true;
                message.Subject = emailInfo.Subject;

                // Set email priority
                message.Priority = MailPriority.High;

                // Add attachments
                if (emailInfo.Files != null)
                {
                    foreach (var file in emailInfo.Files)
                    {
                        var attachment = new Attachment(file);
                        attachment.ContentDisposition.CreationDate = File.GetCreationTime(file);
                        attachment.ContentDisposition.ModificationDate = File.GetLastWriteTime(file);
                        attachment.ContentDisposition.ReadDate = File.GetLastAccessTime(file);
                        attachment.ContentDisposition.FileName = Path.GetFileName(file);
                        attachment.ContentDisposition.Size = new FileInfo(file).Length;
                        attachment.ContentDisposition.DispositionType = DispositionTypeNames.Attachment;
                        message.Attachments.Add(attachment);
                    }
                }

                // SMTP client configuration
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(senderEmail, password);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Timeout = 60 * 5 * 1000; // 5 minutes

                try
                {
                    await smtpClient.SendMailAsync(message);
                    return true;
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Failed to send email. Error: " + ex.Message);
                    return false;
                }
            }
        }


        public bool SendEmailTest(EmailInfo emailInfo)
        {
            return false ;
        }

       
    }
}
