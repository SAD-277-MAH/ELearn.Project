using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearn.Data.Dtos.Site.Setting
{
    public class SettingForMessageSenderDto
    {
        [Display(Name = "API")]
        public string SmsApi { get; set; }

        [Display(Name = "شماره فرستنده")]
        [MaxLength(15, ErrorMessage = "مقدار {0} نباید بیش تر از {1} کاراکتر باشد")]
        public string SmsSender { get; set; }

        [Display(Name = "ایمیل فرستنده")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیش تر از {1} کاراکتر باشد")]
        public string EmailAddress { get; set; }

        [Display(Name = "کلمه عبور ایمیل")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیش تر از {1} کاراکتر باشد")]
        public string EmailPassword { get; set; }

        [Display(Name = "Email SmtpClient")]
        [MaxLength(200, ErrorMessage = "مقدار {0} نباید بیش تر از {1} کاراکتر باشد")]
        public string EmailSmtpClient { get; set; }

        [Display(Name = "Email Port")]
        public int EmailPort { get; set; }

        [Display(Name = "Email EnableSsl")]
        public int EmailEnableSsl { get; set; }
    }
}
