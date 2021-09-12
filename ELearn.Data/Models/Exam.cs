using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELearn.Data.Models
{
    public class Exam : BaseEntity<string>
    {
        public Exam()
        {
            Id = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        [Display(Name = "عنوان آزمون")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string Title { get; set; }

        [Display(Name = "توضیحات آزمون")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public string Description { get; set; }

        [Display(Name = "نمره قبولی")]
        public int PassingGrade { get; set; }

        [Required]
        public string SessionId { get; set; }


        [ForeignKey("SessionId")]
        public virtual Session Session { get; set; }


        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; }
    }
}
