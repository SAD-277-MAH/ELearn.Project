using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearn.Data.Models
{
    public class Setting : BaseEntity<int>
    {
        public Setting()
        {
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        [Display(Name = "نام وبسایت")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیش تر از {1} کاراکتر باشد")]
        public string SiteName { get; set; }

        [Display(Name = "توضیحات مختصر")]
        [DataType(DataType.MultilineText)]
        public string SiteDesc { get; set; }

        [Display(Name = "کلمات کلیدی")]
        [DataType(DataType.MultilineText)]
        public string SiteKeyWords { get; set; }

        [Display(Name = "SMS APIKey")]
        public string SmsAPIKey { get; set; }

        [Display(Name = "SMS SecurityKey")]
        public string SmsSecurityKey { get; set; }

        [Display(Name = "شماره فرستنده")]
        [MaxLength(20, ErrorMessage = "مقدار {0} نباید بیش تر از {1} کاراکتر باشد")]
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
        public bool EmailEnableSsl { get; set; }
    }
}
