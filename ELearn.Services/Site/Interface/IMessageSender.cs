using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Services.Site.Interface
{
    public interface IMessageSender
    {
        void SendSms(string To, string Body);
        void SendEmail(string To, string Subject, string Body);
    }
}
