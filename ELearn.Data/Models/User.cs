using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELearn.Data.Models
{
    public class User : IdentityUser
    {
        [Display(Name = "نام")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(20, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(20, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string LastName { get; set; }

        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(10, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        [MinLength(10, ErrorMessage = "مقدار {0} نمی تواند کمتر از {1} باشد")]
        public string NationalCode { get; set; }

        [Display(Name = "تصویر کاربر")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string PhotoUrl { get; set; }

        [Display(Name = "تاریخ عضویت")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public DateTime RegisterDate { get; set; }

        [Display(Name = "وضعیت")]
        [Required]
        public bool Status { get; set; }

        public string StudentId { get; set; }

        public string TeacherId { get; set; }


        [ForeignKey("StudentId")]
        public Student Student { get; set; }

        [ForeignKey("TeacherId")]
        public Teacher Teacher { get; set; }


        public virtual ICollection<Course> Courses { get; set; }

        public virtual ICollection<UserCourse> UserCourses { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
