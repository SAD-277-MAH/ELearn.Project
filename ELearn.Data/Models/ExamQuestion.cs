using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELearn.Data.Models
{
    public class ExamQuestion : BaseEntity<string>
    {
        public ExamQuestion()
        {
            Id = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        [Display(Name = "عنوان سوال")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string Title { get; set; }

        [Display(Name = "متن سوال")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public string Description { get; set; }

        [Display(Name = "فابل پیوست سوال")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(1000, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string FileUrl { get; set; }

        [Display(Name = "گزینه اول")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(1000, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string FirstChoice { get; set; }

        [Display(Name = "گزینه دوم")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(1000, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string SecondChoice { get; set; }

        [Display(Name = "گزینه سوم")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(1000, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string ThirdChoice { get; set; }

        [Display(Name = "گزینه چهارم")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(1000, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string FourthChoice { get; set; }

        [Display(Name = "گزینه صحیح")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public short CorrectAnswer { get; set; }

        [Required]
        public string ExamId { get; set; }


        [ForeignKey("ExamId")]
        public virtual Exam Exam { get; set; }
    }
}
