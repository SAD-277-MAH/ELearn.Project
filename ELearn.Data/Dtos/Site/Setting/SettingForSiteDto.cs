using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearn.Data.Dtos.Site.Setting
{
    public class SettingForSiteDto
    {
        [Display(Name = "نام وبسایت")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیش تر از {1} کاراکتر باشد")]
        public string SiteName { get; set; }

        [Display(Name = "توضیحات مختصر")]
        [DataType(DataType.MultilineText)]
        public string SiteDesc { get; set; }

        [Display(Name = "کلمات کلیدی")]
        [DataType(DataType.MultilineText)]
        public string SiteKeyWords { get; set; }
    }
}
