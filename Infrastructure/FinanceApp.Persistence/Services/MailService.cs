using FinanceApp.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace FinanceApp.Persistence.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration configuration;

        public MailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendMailAsync(
          string to,
          string subject,
          string body,
          bool isBodyHtml = true,
          List<(Stream Stream, string FileName)> attachments = null)
        {
            var mail = new MailMessage()
            {
                From = new MailAddress(configuration["Mail:Username"], "FinStatsApp", Encoding.UTF8),
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml
            };
            mail.To.Add(to);

            // Dosya ekleme işlemi
            if (attachments != null)
            {
                foreach (var (stream, fileName) in attachments)
                {
                    // stream -> dosya içeriği, fileName -> kullanıcıya görünen isim
                    mail.Attachments.Add(new Attachment(stream, fileName));
                }
            }

            using var smtp = new SmtpClient(configuration["Mail:Host"], int.Parse(configuration["Mail:Port"]))
            {
                Credentials = new NetworkCredential(configuration["Mail:Username"], configuration["Mail:Password"]),
                EnableSsl = true,
            };

            await smtp.SendMailAsync(mail);
        }

    }
}
