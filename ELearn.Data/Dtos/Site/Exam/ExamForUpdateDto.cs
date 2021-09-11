using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ELearn.Data.Dtos.Site.Exam
{
    public class ExamForUpdateDto
    {
        [Display(Name = "عنوان آزمون")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string Title { get; set; }

        [Display(Name = "توضیحات آزمون")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public string Description { get; set; }
    }
}
