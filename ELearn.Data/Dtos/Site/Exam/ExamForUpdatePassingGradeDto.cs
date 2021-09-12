using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELearn.Data.Dtos.Site.Exam
{
    public class ExamForUpdatePassingGradeDto
    {
        [Display(Name = "نمره قبولی")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [Range(0, int.MaxValue, ErrorMessage = "مقدار {0} باید بیشتر یا مساوی 0 باشد")]
        public int PassingGrade { get; set; }
    }
}
