using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using GitManager.Connection;
using GitManager.DAO;
using NLog;

namespace GitManager
{
    public class MailNotification : INotify
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IConfig _config;

        public MailNotification(IConfig config)
        {
            _config = config;
        }

        public bool Notify(List<MergeRequest> info)
        {
            var mail = new MailMessage(_config.FromMail, _config.ToMail);

            var client = new SmtpClient
            {
                Port = 587,
                Credentials = new NetworkCredential(_config.Username, _config.Password),
                EnableSsl = true,

                Host = _config.Host
            };

            mail.Subject = "Self approved merge request detected.";

            var body = new StringBuilder();

            foreach (var request in info)
            {
                body.Append($"{request.Id},{request.CreateAt},{request.Assignee?.UserName},{request.Title}\n");
            }

            mail.Body = body.ToString();
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Fail to send mail notification.");
                return false;
            }
            return true;
        }
    }
}