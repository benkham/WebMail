using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebMail.Core.Abstractions;
using WebMail.Core.Models;

namespace WebMail.MailGun
{
    public class MailGunSender : ISender
    {
        private readonly IConfiguration _configuration;
        private readonly SmtpClient _smtpClient;

        public MailGunSender(IConfiguration configuration)
        {
            _configuration = configuration;
            _smtpClient = GetSmtpClient();
        }

        private SmtpClient GetSmtpClient()
        {
            var host = _configuration.GetSection("MAILGUN_HOST").Value;
            var port = Convert.ToInt32(_configuration.GetSection("MAILGUN_PORT").Value);
            var user = _configuration.GetSection("MAILGUN_USER").Value;
            var password = _configuration.GetSection("MAILGUN_PASSWORD").Value;
            var credentials = new NetworkCredential(user, password);

            return new SmtpClient(host, port)
            {
                UseDefaultCredentials = false,
                Credentials = credentials,
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
        }

        public async Task<SendResponse> SendAsync(IEmail email, CancellationToken? token = null)
        {
            var response = new SendResponse();

            if (token?.IsCancellationRequested ?? false)
            {
                response.ErrorMessages.Add("Email was cancelled by the cancellation token.");
                return response;
            }

            var message = CreateMailMessage(email);

            try
            {
                using (_smtpClient)
                {
                    await _smtpClient.SendMailAsync(message);
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        private MailMessage CreateMailMessage(IEmail email)
        {
            var data = email.Data;

            MailMessage message = null;
            message = new MailMessage
            {
                Subject = data.Subject,
                Body = data.Body,
                IsBodyHtml = false,
                From = new MailAddress(data.FromAddress.EmailAddress, data.FromAddress.Name),
                BodyEncoding = Encoding.UTF8
            };

            data.ToAddresses.ForEach(x =>
            {
                message.To.Add(new MailAddress(x.EmailAddress, x.Name));
            });

            data.CcAddresses.ForEach(x =>
            {
                message.CC.Add(new MailAddress(x.EmailAddress, x.Name));
            });

            data.BccAddresses.ForEach(x =>
            {
                message.Bcc.Add(new MailAddress(x.EmailAddress, x.Name));
            });

            message.Priority = MailPriority.High;

            data.Attachments.ForEach(x =>
            {
                message.Attachments.Add(new System.Net.Mail.Attachment(x.Data, x.Filename, x.ContentType));
            });

            return message;
        }
    }
}
