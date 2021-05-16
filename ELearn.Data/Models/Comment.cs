using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELearn.Data.Models
{
    public class Comment : BaseEntity<string>
    {
        public Comment()
        {
            Id = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        [Display(Name = "متن پیام")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string Text { get; set; }

        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public int Status { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string CourseId { get; set; }


        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }
    }
}
