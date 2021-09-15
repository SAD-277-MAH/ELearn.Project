using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearn.Data.Dtos.Site.Users
{
    public class UserForRegisterTeacherDto
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

        [Display(Name = "روز تولد")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [Range(1, 31, ErrorMessage = "مقدار {0} باید بین {1} و {2} باشد")]
        public int BirthDay { get; set; }

        [Display(Name = "ماه تولد")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [Range(1, 12, ErrorMessage = "مقدار {0} باید بین {1} و {2} باشد")]
        public int BirthMonth { get; set; }

        [Display(Name = "سال تولد")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [Range(1300, 1500, ErrorMessage = "مقدار {0} نمی تواند کمتر از {1} باشد")]
        public int BirthYear { get; set; }

        [Display(Name = "آخرین مدرک تحصیلی")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string Degree { get; set; }

        [Display(Name = "زمینه تخصص")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string Specialty { get; set; }

        [Display(Name = "آدرس ایمیل")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        [EmailAddress(ErrorMessage = "آدرس ایمیل معتبر نیست")]
        public string Email { get; set; }

        [Display(Name = "تلفن ثابت")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string Phone { get; set; }

        [Display(Name = "آدرس")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public string Address { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }
    }
}
