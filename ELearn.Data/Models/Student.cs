using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELearn.Data.Models
{
    public class Student : BaseEntity<string>
    {
        public Student()
        {
            Id = Guid.NewGuid().ToString();

            DateCreated = DateTime.Now;

            DateModified = DateTime.Now;
        }

        [Display(Name = "مقطع تحصیلی")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string Grade { get; set; }

        [Display(Name = "رشته تحصیلی")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string Major { get; set; }

        [Required]
        public string UserId { get; set; }


        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
