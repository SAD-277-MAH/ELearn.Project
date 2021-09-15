using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearn.Data.Dtos.Site.Users
{
    public class UserForRegisterAdminDto
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(15, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        [Phone(ErrorMessage = "شماره موبایل وارد شده صحیح نمی‌باشد")]
        public string UserName { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [StringLength(maximumLength: 100, MinimumLength = 4, ErrorMessage = "رمز عبور باید بین ۴ تا ۱۰۰ کاراکتر باشد")]
        public string Password { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(20, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(20, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string LastName { get; set; }

        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(10, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        [MinLength(10, ErrorMessage = "مقدار {0} نمی تواند کمتر از {1} باشد")]
        public string NationalCode { get; set; }
    }
}
