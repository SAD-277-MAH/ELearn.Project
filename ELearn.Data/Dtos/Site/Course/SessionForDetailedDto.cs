using ELearn.Data.Dtos.Site.Exam;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearn.Data.Dtos.Site.Course
{
    public class SessionForDetailedDto
    {
        public string Id { get; set; }

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

        [Display(Name = "ویدئو جلسه")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public string VideoUrl { get; set; }

        [Display(Name = "فایل جلسه")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public string FileUrl { get; set; }

        [Display(Name = "زمان جلسه")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public string Time { get; set; }

        public ExamForStatusDto ExamResult { get; set; }
    }
}
