using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearn.Data.Dtos.Site.Course
{
    public class CourseForDetailedDto
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string DateCreated { get; set; }

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

        [Display(Name = "پیشنیاز")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public bool HasPrerequisites { get; set; }

        [Display(Name = "قیمت")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [Range(0, int.MaxValue, ErrorMessage = "مقدار {0} نمی تواند کمتر از {1} باشد")]
        public int Price { get; set; }

        [Display(Name = "وضعیت دوره")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public int Status { get; set; }

        [Display(Name = "تاریخ تأیید")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public string VerifiedAt { get; set; }

        [Required]
        public string TeacherName { get; set; }

        [Required]
        public int CategoryName { get; set; }

        public string PrerequisitesTitle { get; set; }
    }
}
