using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearn.Data.Dtos.Site.Course
{
    public class CourseForAdminDetailedDto
    {
        [Required]
        public string Id { get; set; }

        [Display(Name = "عنوان دوره")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string Title { get; set; }

        [Display(Name = "توضیحات دوره")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public string Description { get; set; }

        [Display(Name = "تصویر دوره")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public string PhotoUrl { get; set; }

        [Display(Name = "وضعیت دوره")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public int Status { get; set; }

        [Display(Name = "نام مدرس")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public string TeacherName { get; set; }


        [Display(Name = "تعداد جلسات")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public int SessionCount { get; set; }
    }
}
