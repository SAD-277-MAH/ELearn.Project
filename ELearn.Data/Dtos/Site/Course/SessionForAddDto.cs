using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearn.Data.Dtos.Site.Course
{
    public class SessionForAddDto
    {
        [Display(Name = "عنوان جلسه")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string Title { get; set; }

        [Display(Name = "توضیحات جلسه")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public string Description { get; set; }

        [Display(Name = "شماره جلسه")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public int SessionNumber { get; set; }

        [Display(Name = "زمان جلسه")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5]?[0-9]$", ErrorMessage = "مقدار {0} را به صورت صحیح وارد کنید")]
        public string Time { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}
