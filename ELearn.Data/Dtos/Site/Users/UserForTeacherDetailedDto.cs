using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearn.Data.Dtos.Site.Users
{
    public class UserForTeacherDetailedDto
    {
        public string UserId { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(15, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string PhoneNumber { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string Email { get; set; }

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

        [Display(Name = "سن")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public int Age { get; set; }

        [Display(Name = "آخرین مدرک تحصیلی")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string Degree { get; set; }

        [Display(Name = "تلفن ثابت")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string Phone { get; set; }

        [Display(Name = "آدرس")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public string Address { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public string Description { get; set; }

        [Display(Name = "وضعیت مدارک")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public int Status { get; set; }
    }
}
