using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELearn.Data.Models
{
    public class Teacher : BaseEntity<string>
    {
        public Teacher()
        {
            Id = Guid.NewGuid().ToString();

            DateCreated = DateTime.Now;

            DateModified = DateTime.Now;
        }

        [Display(Name = "تاریخ تولد")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "آخرین مدرک تحصیلی")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string Degree { get; set; }

        [Display(Name = "تلفن ثابت")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} باشد")]
        public string Phone { get; set; }

        [Display(Name = "آدرس")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public string Address { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public string Description { get; set; }

        [Display(Name = "وضعیت مدارک")]
        [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
        public int Status { get; set; }

        [Required]
        public string UserId { get; set; }


        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
