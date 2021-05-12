using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELearn.Data.Models
{
    public class Session : BaseEntity<string>
    {
        public Session()
        {
            Id = Guid.NewGuid().ToString();

            DateCreated = DateTime.Now;

            DateModified = DateTime.Now;
        }

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

        [Display(Name = "زمان جلسه")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public TimeSpan Time { get; set; }

        [Required]
        public string CourseId { get; set; }


        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }
    }
}
