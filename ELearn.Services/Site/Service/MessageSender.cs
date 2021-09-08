using ELearn.Data.Context;
using ELearn.Repo.Infrastructure;
using ELearn.Services.Site.Interface;
using SmsIrRestfulNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Services.Site.Service
{
    public class MessageSender : IMessageSender
    {
        private readonly IUnitOfWork<DatabaseContext> _db;

        public MessageSender(IUnitOfWork<DatabaseContext> db)
        {
            _db = db;
        }

        public void SendEmail(string To, string Subject, string Body)
        {
            try
            {
                var setting = _db.SettingRepository.Get().LastOrDefault();

                MailMessage message = new MailMessage();
                SmtpClient client = new SmtpClient(setting.EmailSmtpClient);

                message.From = new MailAddress(setting.EmailAddress, setting.SiteName);
                message.To.Add(new MailAddress(To));
                message.Subject = Subject;
                message.Body = Body;
                message.IsBodyHtml = true;

                client.Port = setting.EmailPort;
                client.Credentials = new NetworkCredential(setting.EmailAddress, setting.EmailPassword);
                client.EnableSsl = setting.EmailEnableSsl;

                client.Send(message);
            }
            catch
            {
            }
        }

        public void SendSms(string To, string Body)
        {
            try
            {
                var setting = _db.SettingRepository.Get().LastOrDefault();

                var token = new Token().GetToken(setting.SmsAPIKey, setting.SmsSecurityKey);

                var messageSendObject = new MessageSendObject()
                {
                    Messages = new List<string> { Body }.ToArray(),
                    MobileNumbers = new List<string> { To }.ToArray(),
                    LineNumber = setting.SmsSender,
                    SendDateTime = null,
                    CanContinueInCaseOfError = false
                };

                MessageSendResponseObject messageSendResponseObject = new MessageSend().Send(token, messageSendObject);

                //if (messageSendResponseObject.IsSuccessful)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
            }
            catch
            {
                //return false;
            }
        }
    }
}
