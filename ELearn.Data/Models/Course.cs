using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELearn.Data.Models
{
    public class Course : BaseEntity<string>
    {
        public Course()
        {
            Id = Guid.NewGuid().ToString();

            DateCreated = DateTime.Now;

            DateModified = DateTime.Now;

            VerifiedAt = DateTime.Now;
        }

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

        [Display(Name = "قیمت")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [Range(0, int.MaxValue, ErrorMessage = "مقدار {0} نمی تواند کمتر از {1} باشد")]
        public int Price { get; set; }

        [Display(Name = "وضعیت دوره")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public int Status { get; set; }

        [Display(Name = "تاریخ تأیید")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public DateTime VerifiedAt { get; set; }

        [Required]
        public string TeacherId { get; set; }

        [Required]
        public int CategoryId { get; set; }


        [ForeignKey("TeacherId")]
        public virtual User Teacher { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }


        public virtual ICollection<Session> Sessions { get; set; }

        public virtual ICollection<UserCourse> UserCourses { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
