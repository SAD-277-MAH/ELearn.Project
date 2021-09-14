using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELearn.Data.Models
{
    public class ExamAnswer : BaseEntity<string>
    {
        public ExamAnswer()
        {
            Id = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        [Display(Name = "نمره")]
        [Range(0, int.MaxValue, ErrorMessage = "نمره آزمون باید بیشتر یا مساوی 0 باشد")]
        public int Grade { get; set; }

        [Display(Name = "وضعیت قبولی")]
        public bool Status { get; set; }

        public string UserId { get; set; }

        public string ExamId { get; set; }


        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("ExamId")]
        public virtual Exam Exam { get; set; }
    }
}
